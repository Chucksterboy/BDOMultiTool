using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace BDOMultiTool;

internal sealed class MarketDatabase
{
	private readonly string connectionString;

	public MarketDatabase(string path)
	{
		connectionString = new SqliteConnectionStringBuilder
		{
			DataSource = path,
			Mode = SqliteOpenMode.ReadWriteCreate,
			Cache = SqliteCacheMode.Shared
		}.ToString();
	}

	public async Task InitializeAsync(CancellationToken cancellationToken)
	{
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		string commandText = "PRAGMA journal_mode=WAL;\nPRAGMA foreign_keys=ON;\nCREATE TABLE IF NOT EXISTS settings (\n    key TEXT PRIMARY KEY,\n    value TEXT NOT NULL\n);\nCREATE TABLE IF NOT EXISTS tracked_items (\n    item_id INTEGER NOT NULL,\n    enhancement INTEGER NOT NULL,\n    region TEXT NOT NULL,\n    name TEXT NOT NULL,\n    grade INTEGER NOT NULL,\n    main_category INTEGER NOT NULL DEFAULT 0,\n    sub_category INTEGER NOT NULL DEFAULT 0,\n    created_utc TEXT NOT NULL,\n    last_price INTEGER,\n    last_stock INTEGER,\n    last_trade_count INTEGER,\n    last_updated_utc TEXT,\n    PRIMARY KEY (item_id, enhancement, region)\n);\nCREATE TABLE IF NOT EXISTS snapshots (\n    snapshot_id INTEGER PRIMARY KEY AUTOINCREMENT,\n    item_id INTEGER NOT NULL,\n    enhancement INTEGER NOT NULL,\n    region TEXT NOT NULL,\n    captured_utc TEXT NOT NULL,\n    price INTEGER NOT NULL,\n    stock INTEGER,\n    trade_count INTEGER,\n    order_book_min INTEGER,\n    order_book_max INTEGER,\n    order_book_average REAL,\n    source TEXT NOT NULL,\n    UNIQUE(item_id, enhancement, region, captured_utc, source)\n);\nCREATE INDEX IF NOT EXISTS ix_snapshots_item_time\n    ON snapshots(item_id, enhancement, region, captured_utc);\nCREATE TABLE IF NOT EXISTS outfit_catalog (\n    item_id INTEGER NOT NULL,\n    region TEXT NOT NULL,\n    name TEXT NOT NULL,\n    grade INTEGER NOT NULL,\n    sub_category INTEGER NOT NULL,\n    price INTEGER NOT NULL,\n    stock INTEGER NOT NULL,\n    last_catalog_sync_utc TEXT NOT NULL,\n    last_detailed_utc TEXT,\n    PRIMARY KEY(item_id, region)\n);\nCREATE TABLE IF NOT EXISTS outfit_snapshots (\n    snapshot_id INTEGER PRIMARY KEY AUTOINCREMENT,\n    item_id INTEGER NOT NULL,\n    region TEXT NOT NULL,\n    captured_utc TEXT NOT NULL,\n    price INTEGER NOT NULL,\n    stock INTEGER NOT NULL,\n    trade_count INTEGER,\n    preorder_count INTEGER,\n    source TEXT NOT NULL\n);\nCREATE INDEX IF NOT EXISTS ix_outfit_snapshots_item_time\n    ON outfit_snapshots(item_id, region, captured_utc);";
		await using SqliteCommand command = connection.CreateCommand();
		command.CommandText = commandText;
		await command.ExecuteNonQueryAsync(cancellationToken);
	}

	public async Task<MarketSettings> GetSettingsAsync(CancellationToken cancellationToken)
	{
		Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		MarketSettings result2;
		await using (SqliteConnection connection = await OpenAsync(cancellationToken))
		{
			MarketSettings marketSettings2;
			await using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT key, value FROM settings;";
				MarketSettings marketSettings;
				await using (SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
				{
					while (await reader.ReadAsync(cancellationToken))
					{
						values[reader.GetString(0)] = reader.GetString(1);
					}
					string value;
					string region = values.TryGetValue("region", out value) ? NormalizeRegion(value) : MarketSettings.Default.Region;
					string value2;
					int result;
					int intervalMinutes = ((values.TryGetValue("intervalMinutes", out value2) && int.TryParse(value2, out result)) ? Math.Clamp(result, 5, 1440) : MarketSettings.Default.IntervalMinutes);
					marketSettings = new MarketSettings(region, intervalMinutes);
				}
				marketSettings2 = marketSettings;
			}
			result2 = marketSettings2;
		}
		return result2;
	}

	public async Task SaveSettingsAsync(MarketSettings settings, CancellationToken cancellationToken)
	{
		settings = settings with { Region = NormalizeRegion(settings.Region) };
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);
		KeyValuePair<string, string>[] array = new KeyValuePair<string, string>[2]
		{
			new KeyValuePair<string, string>("region", settings.Region),
			new KeyValuePair<string, string>("intervalMinutes", settings.IntervalMinutes.ToString())
		};
		for (int i = 0; i < array.Length; i++)
		{
			KeyValuePair<string, string> keyValuePair = array[i];
			await using SqliteCommand command = connection.CreateCommand();
			command.Transaction = (SqliteTransaction)transaction;
			command.CommandText = "INSERT INTO settings(key,value) VALUES($key,$value) ON CONFLICT(key) DO UPDATE SET value=excluded.value;";
			command.Parameters.AddWithValue("$key", keyValuePair.Key);
			command.Parameters.AddWithValue("$value", keyValuePair.Value);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}
		await transaction.CommitAsync(cancellationToken);
	}

	public async Task AddTrackedItemAsync(MarketItem item, string region, CancellationToken cancellationToken)
	{
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using SqliteCommand command = connection.CreateCommand();
		command.CommandText = "INSERT INTO tracked_items(\n    item_id, enhancement, region, name, grade, main_category, sub_category,\n    created_utc, last_price, last_stock, last_trade_count, last_updated_utc)\nVALUES($id,$enhancement,$region,$name,$grade,$main,$sub,$created,$price,$stock,$trades,$updated)\nON CONFLICT(item_id,enhancement,region) DO UPDATE SET\n    name=excluded.name, grade=excluded.grade,\n    main_category=excluded.main_category, sub_category=excluded.sub_category;";
		command.Parameters.AddWithValue("$id", item.ItemId);
		command.Parameters.AddWithValue("$enhancement", item.Enhancement);
		command.Parameters.AddWithValue("$region", region);
		command.Parameters.AddWithValue("$name", item.Name);
		command.Parameters.AddWithValue("$grade", item.Grade);
		command.Parameters.AddWithValue("$main", item.MainCategory);
		command.Parameters.AddWithValue("$sub", item.SubCategory);
		command.Parameters.AddWithValue("$created", DateTimeOffset.UtcNow.ToString("O"));
		command.Parameters.AddWithValue("$price", item.CurrentPrice);
		command.Parameters.AddWithValue("$stock", item.Stock);
		command.Parameters.AddWithValue("$trades", item.TradeCount);
		command.Parameters.AddWithValue("$updated", DateTimeOffset.UtcNow.ToString("O"));
		await command.ExecuteNonQueryAsync(cancellationToken);
	}

	public async Task RemoveTrackedItemAsync(long itemId, int enhancement, string region, CancellationToken cancellationToken)
	{
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using SqliteCommand command = connection.CreateCommand();
		command.CommandText = "DELETE FROM tracked_items WHERE item_id=$id AND enhancement=$enhancement AND region=$region;";
		command.Parameters.AddWithValue("$id", itemId);
		command.Parameters.AddWithValue("$enhancement", enhancement);
		command.Parameters.AddWithValue("$region", region);
		await command.ExecuteNonQueryAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<TrackedItem>> GetTrackedItemsAsync(string region, CancellationToken cancellationToken)
	{
		List<TrackedItem> items = new List<TrackedItem>();
		IReadOnlyList<TrackedItem> result;
		await using (SqliteConnection connection = await OpenAsync(cancellationToken))
		{
			IReadOnlyList<TrackedItem> readOnlyList2;
			await using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT item_id, enhancement, region, name, grade, main_category, sub_category,\n       last_price, last_stock, last_trade_count, last_updated_utc\nFROM tracked_items WHERE region=$region ORDER BY name, enhancement;";
				command.Parameters.AddWithValue("$region", region);
				IReadOnlyList<TrackedItem> readOnlyList;
				await using (SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
				{
					while (await reader.ReadAsync(cancellationToken))
					{
						items.Add(new TrackedItem(reader.GetInt64(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.IsDBNull(7) ? ((long?)null) : new long?(reader.GetInt64(7)), reader.IsDBNull(8) ? ((long?)null) : new long?(reader.GetInt64(8)), reader.IsDBNull(9) ? ((long?)null) : new long?(reader.GetInt64(9)), reader.IsDBNull(10) ? ((DateTimeOffset?)null) : new DateTimeOffset?(DateTimeOffset.Parse(reader.GetString(10)))));
					}
					readOnlyList = items;
				}
				readOnlyList2 = readOnlyList;
			}
			result = readOnlyList2;
		}
		return result;
	}

	public async Task<DateTimeOffset?> GetLatestMarketSampleUtcAsync(string region, CancellationToken cancellationToken)
	{
		DateTimeOffset? latest = null;
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using SqliteCommand command = connection.CreateCommand();
		command.CommandText = @"
SELECT MAX(captured_utc) FROM (
    SELECT captured_utc FROM snapshots WHERE region=$region AND source='local-snapshot'
    UNION ALL
    SELECT captured_utc FROM outfit_snapshots WHERE region=$region
);";
		command.Parameters.AddWithValue("$region", NormalizeRegion(region));
		object? value = await command.ExecuteScalarAsync(cancellationToken);
		if (value != null && value != DBNull.Value)
		{
			string text = Convert.ToString(value) ?? string.Empty;
			if (DateTimeOffset.TryParse(text, out DateTimeOffset parsed))
			{
				latest = parsed;
			}
		}
		return latest;
	}

	public async Task SaveSnapshotAsync(TrackedItem item, MarketItem variant, MarketSnapshot snapshot, CancellationToken cancellationToken)
	{
		DateTimeOffset now = DateTimeOffset.UtcNow;
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);
		foreach (ProviderHistoryPoint item2 in snapshot.History)
		{
			await InsertSnapshotAsync(connection, (SqliteTransaction)transaction, item, item2.Timestamp, item2.Price, null, null, item2.Price, item2.Price, item2.Price, "provider-history", cancellationToken);
		}
		await InsertSnapshotAsync(connection, (SqliteTransaction)transaction, item, now, snapshot.Price, snapshot.Stock, variant.TradeCount, snapshot.OrderBookMin, snapshot.OrderBookMax, snapshot.OrderBookAverage, "local-snapshot", cancellationToken);
		await using SqliteCommand update = connection.CreateCommand();
		update.Transaction = (SqliteTransaction)transaction;
		update.CommandText = "UPDATE tracked_items SET\n    name=$name, grade=$grade, last_price=$price, last_stock=$stock,\n    last_trade_count=$trades, last_updated_utc=$updated\nWHERE item_id=$id AND enhancement=$enhancement AND region=$region;";
		update.Parameters.AddWithValue("$name", variant.Name);
		update.Parameters.AddWithValue("$grade", variant.Grade);
		update.Parameters.AddWithValue("$price", snapshot.Price);
		update.Parameters.AddWithValue("$stock", snapshot.Stock);
		update.Parameters.AddWithValue("$trades", variant.TradeCount);
		update.Parameters.AddWithValue("$updated", now.ToString("O"));
		update.Parameters.AddWithValue("$id", item.ItemId);
		update.Parameters.AddWithValue("$enhancement", item.Enhancement);
		update.Parameters.AddWithValue("$region", item.Region);
		await update.ExecuteNonQueryAsync(cancellationToken);
		await transaction.CommitAsync(cancellationToken);
	}

	public async Task<ItemAnalytics?> GetAnalyticsAsync(long itemId, int enhancement, string region, int days, CancellationToken cancellationToken)
	{
		TrackedItem item = (await GetTrackedItemsAsync(region, cancellationToken)).FirstOrDefault((TrackedItem x) => x.ItemId == itemId && x.Enhancement == enhancement);
		if ((object)item == null)
		{
			return null;
		}
		DateTimeOffset since = DateTimeOffset.UtcNow.AddDays(-Math.Clamp(days, 1, 365));
		List<PricePoint> points = new List<PricePoint>();
		ItemAnalytics result;
		await using (SqliteConnection connection = await OpenAsync(cancellationToken))
		{
			await using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT captured_utc, price, stock, trade_count\nFROM snapshots\nWHERE item_id=$id AND enhancement=$enhancement AND region=$region\n  AND captured_utc >= $since\nORDER BY captured_utc;";
				command.Parameters.AddWithValue("$id", itemId);
				command.Parameters.AddWithValue("$enhancement", enhancement);
				command.Parameters.AddWithValue("$region", region);
				command.Parameters.AddWithValue("$since", since.ToString("O"));
				await using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					points.Add(new PricePoint(DateTimeOffset.Parse(reader.GetString(0)), reader.GetInt64(1), reader.IsDBNull(2) ? ((long?)null) : new long?(reader.GetInt64(2)), reader.IsDBNull(3) ? ((long?)null) : new long?(reader.GetInt64(3))));
				}
			}
			long[] prices = points.Select((PricePoint x) => x.Price).ToArray();
			long current = item.LastPrice ?? prices.LastOrDefault();
			double? trend = ((prices.Length > 1 && prices[0] != 0L) ? new double?((double)(current - prices[0]) * 100.0 / (double)prices[0]) : ((double?)null));
			List<SalesWindow> sales = new List<SalesWindow>();
			(string, TimeSpan)[] array = new(string, TimeSpan)[4]
			{
				("24 hours", TimeSpan.FromHours(24.0)),
				("3 days", TimeSpan.FromDays(3.0)),
				("7 days", TimeSpan.FromDays(7.0)),
				("30 days", TimeSpan.FromDays(30.0))
			};
			for (int num = 0; num < array.Length; num++)
			{
				(string, TimeSpan) tuple = array[num];
				List<SalesWindow> list = sales;
				list.Add(await GetSalesWindowAsync(connection, itemId, enhancement, region, tuple.Item1, tuple.Item2, cancellationToken));
			}
			result = new ItemAnalytics(item, (current == 0L) ? ((long?)null) : new long?(current), (prices.Length == 0) ? ((long?)null) : new long?(prices.Min()), (prices.Length == 0) ? ((long?)null) : new long?(prices.Max()), (prices.Length == 0) ? ((double?)null) : new double?(((IEnumerable<long>)prices).Average((Func<long, double>)((long x) => x))), trend, sales, points);
		}
		return result;
	}

	public async Task ExportCsvAsync(string region, string path, CancellationToken cancellationToken)
	{
		await using StreamWriter writer = new StreamWriter(path, append: false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
		await writer.WriteLineAsync("dataset,item_id,enhancement,region,name,captured_utc,price,stock,cumulative_sales,preorders,order_book_min,order_book_max,order_book_average,source");
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using SqliteCommand command = connection.CreateCommand();
		command.CommandText = "SELECT 'tracked_item',s.item_id,s.enhancement,s.region,t.name,s.captured_utc,s.price,s.stock,\n       s.trade_count,NULL,s.order_book_min,s.order_book_max,s.order_book_average,s.source\nFROM snapshots s\nJOIN tracked_items t ON t.item_id=s.item_id AND t.enhancement=s.enhancement AND t.region=s.region\nWHERE s.region=$region\nUNION ALL\nSELECT 'outfit',s.item_id,0,s.region,c.name,s.captured_utc,s.price,s.stock,\n       s.trade_count,s.preorder_count,NULL,NULL,NULL,s.source\nFROM outfit_snapshots s\nJOIN outfit_catalog c ON c.item_id=s.item_id AND c.region=s.region\nWHERE s.region=$region\nORDER BY 5,2,3,6;";
		command.Parameters.AddWithValue("$region", region);
		SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
		try
		{
			while (await reader.ReadAsync(cancellationToken))
			{
				IEnumerable<string> values = from i in Enumerable.Range(0, reader.FieldCount)
					select (!reader.IsDBNull(i)) ? EscapeCsv(Convert.ToString(reader.GetValue(i)) ?? "") : "";
				await writer.WriteLineAsync(string.Join(",", values));
			}
		}
		finally
		{
			if (reader != null)
			{
				await reader.DisposeAsync();
			}
		}
	}

	public async Task<int> PruneOldMarketSamplesAsync(TimeSpan retention, CancellationToken cancellationToken)
	{
		DateTimeOffset cutoff = DateTimeOffset.UtcNow.Subtract(retention);
		int removed = 0;
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);
		string[] tables = ["snapshots", "outfit_snapshots"];
		foreach (string table in tables)
		{
			await using SqliteCommand command = connection.CreateCommand();
			command.Transaction = (SqliteTransaction)transaction;
			command.CommandText = $"DELETE FROM {table} WHERE captured_utc < $cutoff;";
			command.Parameters.AddWithValue("$cutoff", cutoff.ToString("O"));
			removed += await command.ExecuteNonQueryAsync(cancellationToken);
		}
		await transaction.CommitAsync(cancellationToken);
		return removed;
	}

	public async Task SyncOutfitCatalogAsync(IReadOnlyList<MarketItem> items, string region, CancellationToken cancellationToken)
	{
		DateTimeOffset now = DateTimeOffset.UtcNow;
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);
		foreach (MarketItem item in items)
		{
			await using (SqliteCommand command = connection.CreateCommand())
			{
				command.Transaction = (SqliteTransaction)transaction;
				command.CommandText = "INSERT INTO outfit_catalog(\n    item_id,region,name,grade,sub_category,price,stock,last_catalog_sync_utc)\nVALUES($id,$region,$name,$grade,$sub,$price,$stock,$sync)\nON CONFLICT(item_id,region) DO UPDATE SET\n    name=excluded.name,grade=excluded.grade,sub_category=excluded.sub_category,\n    price=excluded.price,stock=excluded.stock,last_catalog_sync_utc=excluded.last_catalog_sync_utc;";
				command.Parameters.AddWithValue("$id", item.ItemId);
				command.Parameters.AddWithValue("$region", region);
				command.Parameters.AddWithValue("$name", item.Name);
				command.Parameters.AddWithValue("$grade", item.Grade);
				command.Parameters.AddWithValue("$sub", item.SubCategory);
				command.Parameters.AddWithValue("$price", item.CurrentPrice);
				command.Parameters.AddWithValue("$stock", item.Stock);
				command.Parameters.AddWithValue("$sync", now.ToString("O"));
				await command.ExecuteNonQueryAsync(cancellationToken);
			}
			await using SqliteCommand snapshot = connection.CreateCommand();
			snapshot.Transaction = (SqliteTransaction)transaction;
			snapshot.CommandText = "INSERT INTO outfit_snapshots(\n    item_id,region,captured_utc,price,stock,trade_count,preorder_count,source)\nVALUES($id,$region,$captured,$price,$stock,NULL,NULL,'catalog');";
			snapshot.Parameters.AddWithValue("$id", item.ItemId);
			snapshot.Parameters.AddWithValue("$region", region);
			snapshot.Parameters.AddWithValue("$captured", now.ToString("O"));
			snapshot.Parameters.AddWithValue("$price", item.CurrentPrice);
			snapshot.Parameters.AddWithValue("$stock", item.Stock);
			await snapshot.ExecuteNonQueryAsync(cancellationToken);
		}
		await transaction.CommitAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<MarketItem>> GetOutfitsDueAsync(string region, int limit, CancellationToken cancellationToken)
	{
		DateTimeOffset now = DateTimeOffset.UtcNow;
		List<(MarketItem Item, DateTimeOffset? LastDetailed, long Preorders, long Trades)> items = new List<(MarketItem, DateTimeOffset?, long, long)>();
		IReadOnlyList<MarketItem> result;
		await using (SqliteConnection connection = await OpenAsync(cancellationToken))
		{
			IReadOnlyList<MarketItem> readOnlyList2;
			await using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT c.item_id,c.name,c.grade,c.price,c.stock,c.sub_category,c.last_detailed_utc,\n       COALESCE((SELECT s.preorder_count FROM outfit_snapshots s\n                 WHERE s.item_id=c.item_id AND s.region=c.region AND s.preorder_count IS NOT NULL\n                 ORDER BY s.captured_utc DESC LIMIT 1),0),\n       COALESCE((SELECT s.trade_count FROM outfit_snapshots s\n                 WHERE s.item_id=c.item_id AND s.region=c.region AND s.trade_count IS NOT NULL\n                 ORDER BY s.captured_utc DESC LIMIT 1),0)\nFROM outfit_catalog c WHERE c.region=$region;";
				command.Parameters.AddWithValue("$region", region);
				IReadOnlyList<MarketItem> readOnlyList;
				await using (SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
				{
					while (await reader.ReadAsync(cancellationToken))
					{
						DateTimeOffset? lastDetailed = reader.IsDBNull(6) ? null : DateTimeOffset.Parse(reader.GetString(6));
						items.Add((new MarketItem(reader.GetInt64(0), 0, reader.GetString(1), reader.GetInt32(2), reader.GetInt64(3), reader.GetInt64(4), 0L, 55, reader.GetInt32(5)), lastDetailed, reader.GetInt64(7), reader.GetInt64(8)));
					}
					readOnlyList = (from item in items
						let missingDetail = item.LastDetailed.HasValue ? 0 : 1
						let sampleAgeHours = item.LastDetailed.HasValue ? Math.Max(0.0, (now - item.LastDetailed.Value).TotalHours) : double.MaxValue
						let preorderBonus = Math.Min(24.0, Math.Log10((double)Math.Max(0L, item.Preorders) + 1.0) * 8.0)
						let stockBonus = item.Item.Stock <= 0 ? 6.0 : 0.0
						let movementBonus = item.Trades > 0 ? 2.0 : 0.0
						orderby missingDetail descending, sampleAgeHours + preorderBonus + stockBonus + movementBonus descending, item.Item.Name
						select item.Item).Take(Math.Clamp(limit, 1, 600)).ToList();
				}
				readOnlyList2 = readOnlyList;
			}
			result = readOnlyList2;
		}
		return result;
	}

	public async Task SaveOutfitDetailAsync(MarketItem item, MarketItem variant, MarketSnapshot snapshot, string region, CancellationToken cancellationToken)
	{
		DateTimeOffset now = DateTimeOffset.UtcNow;
		await using SqliteConnection connection = await OpenAsync(cancellationToken);
		await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);
		await using (SqliteCommand command = connection.CreateCommand())
		{
			command.Transaction = (SqliteTransaction)transaction;
			command.CommandText = "INSERT INTO outfit_snapshots(\n    item_id,region,captured_utc,price,stock,trade_count,preorder_count,source)\nVALUES($id,$region,$captured,$price,$stock,$trades,$preorders,'detail');";
			command.Parameters.AddWithValue("$id", item.ItemId);
			command.Parameters.AddWithValue("$region", region);
			command.Parameters.AddWithValue("$captured", now.ToString("O"));
			command.Parameters.AddWithValue("$price", snapshot.Price);
			command.Parameters.AddWithValue("$stock", snapshot.Stock);
			command.Parameters.AddWithValue("$trades", variant.TradeCount);
			command.Parameters.AddWithValue("$preorders", snapshot.PreorderCount);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}
		await using (SqliteCommand command = connection.CreateCommand())
		{
			command.Transaction = (SqliteTransaction)transaction;
			command.CommandText = "UPDATE outfit_catalog SET name=$name,price=$price,stock=$stock,last_detailed_utc=$updated\nWHERE item_id=$id AND region=$region;";
			command.Parameters.AddWithValue("$name", variant.Name);
			command.Parameters.AddWithValue("$price", snapshot.Price);
			command.Parameters.AddWithValue("$stock", snapshot.Stock);
			command.Parameters.AddWithValue("$updated", now.ToString("O"));
			command.Parameters.AddWithValue("$id", item.ItemId);
			command.Parameters.AddWithValue("$region", region);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}
		await transaction.CommitAsync(cancellationToken);
	}

	public async Task<OutfitReport> GetOutfitReportAsync(string region, CancellationToken cancellationToken)
	{
		List<(long Id, string Name, long Price, long Stock, DateTimeOffset Sync, DateTimeOffset? Detail)> catalog = new List<(long, string, long, long, DateTimeOffset, DateTimeOffset?)>();
		Dictionary<long, List<(DateTimeOffset Time, long Trades, long Preorders)>> samples = new Dictionary<long, List<(DateTimeOffset, long, long)>>();
		OutfitReport result;
		await using (SqliteConnection connection = await OpenAsync(cancellationToken))
		{
			await using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT item_id,name,price,stock,last_catalog_sync_utc,last_detailed_utc\nFROM outfit_catalog WHERE region=$region;";
				command.Parameters.AddWithValue("$region", region);
				await using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					catalog.Add((reader.GetInt64(0), reader.GetString(1), reader.GetInt64(2), reader.GetInt64(3), DateTimeOffset.Parse(reader.GetString(4)), reader.IsDBNull(5) ? ((DateTimeOffset?)null) : new DateTimeOffset?(DateTimeOffset.Parse(reader.GetString(5)))));
				}
			}
			await using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT item_id,captured_utc,trade_count,preorder_count\nFROM outfit_snapshots\nWHERE region=$region AND trade_count IS NOT NULL\nORDER BY item_id,captured_utc;";
				command.Parameters.AddWithValue("$region", region);
				await using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					long @int = reader.GetInt64(0);
					if (!samples.TryGetValue(@int, out List<(DateTimeOffset, long, long)> value))
					{
						value = (samples[@int] = new List<(DateTimeOffset, long, long)>());
					}
					value.Add((DateTimeOffset.Parse(reader.GetString(1)), reader.GetInt64(2), reader.IsDBNull(3) ? 0 : reader.GetInt64(3)));
				}
			}
			List<OutfitOpportunity> list2 = new List<OutfitOpportunity>();
			foreach (var item2 in catalog)
			{
				samples.TryGetValue(item2.Id, out List<(DateTimeOffset, long, long)> value2);
				if (value2 == null)
				{
					value2 = new List<(DateTimeOffset, long, long)>();
				}
				long? num = CalculateOutfitSalesWindow(value2, TimeSpan.FromHours(24.0));
				long? num2 = CalculateOutfitSalesWindow(value2, TimeSpan.FromDays(3.0));
				long? num3 = CalculateOutfitSalesWindow(value2, TimeSpan.FromDays(7.0));
				double? num4 = EstimateOutfitSalesPerDay(value2, num, num2, num3);
				double? sevenDayChancePercent = null;
				long? obj;
				if (value2.Count != 0)
				{
					List<(DateTimeOffset, long, long)> list3 = value2;
					obj = list3[list3.Count - 1].Item3;
				}
				else
				{
					obj = null;
				}
				long? preorderCount = obj;
				long? obj2;
				if (value2.Count != 0)
				{
					List<(DateTimeOffset, long, long)> list4 = value2;
					obj2 = list4[list4.Count - 1].Item2;
				}
				else
				{
					obj2 = null;
				}
				long? lifetimeSales = obj2;
				double? estimatedQueueDays = null;
				double? demandMomentumPercent = null;
				if (num4 > 0.0)
				{
					double num5 = (double)Math.Max(0L, preorderCount.GetValueOrDefault()) + 1.0;
					estimatedQueueDays = num5 / num4.Value;
					sevenDayChancePercent = (1.0 - Math.Exp((0.0 - num4.Value) * 7.0 / num5)) * 100.0;
				}
				if (num.HasValue && num3.HasValue && num3 > 0)
				{
					double num6 = (double)num3.Value / 7.0;
					demandMomentumPercent = ((double)num.Value - num6) * 100.0 / num6;
				}
				bool flag = num >= 3 || num2 >= 8 || num3 >= 15;
				bool flag2 = !num3.HasValue || num3 <= 2 || (num2.GetValueOrDefault() <= 2 && num.GetValueOrDefault() == 0);
				bool flag3 = value2.Count >= 2 && num4.HasValue && num4.Value > 0.0 && sevenDayChancePercent.HasValue && (flag || preorderCount.GetValueOrDefault() > 0) && !flag2;
				double num9 = Math.Min(1.0, Math.Sqrt(((double)(num3 ?? (long)Math.Round(num4.GetValueOrDefault() * 7.0))) / 30.0));
				double num8 = CalculateOutfitConfidence(value2, item2.Detail, num, num2, num3);
				double confidencePercent = num8 * 100.0;
				double preorderPressure = Math.Min(1.0, Math.Log10((double)Math.Max(0L, preorderCount.GetValueOrDefault()) + 1.0) / 2.0);
				double stockPressure = 1.0 - Math.Min(1.0, Math.Log10((double)Math.Max(1L, item2.Stock) + 1.0) / 3.0);
				double liveSignal = 0.65 + preorderPressure * 0.25 + stockPressure * 0.1;
				double score = (flag3 ? (sevenDayChancePercent.Value / 100.0 * num9 * num8 * liveSignal) : 0.0);
				list2.Add(new OutfitOpportunity(item2.Id, item2.Name, item2.Price, item2.Stock, preorderCount, lifetimeSales, num, num2, num3, num4, sevenDayChancePercent, estimatedQueueDays, demandMomentumPercent, confidencePercent, score, flag3, value2.Count, item2.Detail));
			}
			int num10 = catalog.Count<(long, string, long, long, DateTimeOffset, DateTimeOffset?)>(delegate((long Id, string Name, long Price, long Stock, DateTimeOffset Sync, DateTimeOffset? Detail) x)
			{
				DateTimeOffset? item = x.Detail;
				return item.HasValue;
			});
			result = new OutfitReport(catalog.Count, num10, (catalog.Count == 0) ? 0.0 : ((double)num10 * 100.0 / (double)catalog.Count), (catalog.Count == 0) ? ((DateTimeOffset?)null) : new DateTimeOffset?(catalog.Max<(long, string, long, long, DateTimeOffset, DateTimeOffset?), DateTimeOffset>(((long Id, string Name, long Price, long Stock, DateTimeOffset Sync, DateTimeOffset? Detail) x) => x.Sync)), (from x in list2
				orderby x.Score descending, x.SalesPerDay ?? (-1.0) descending, x.PreorderCount ?? long.MaxValue, x.Name
				select x).ToArray());
		}
		return result;
	}

	private static long? CalculateOutfitSalesWindow(IReadOnlyList<(DateTimeOffset Time, long Trades, long Preorders)> samples, TimeSpan window)
	{
		if (samples.Count < 2)
		{
			return null;
		}
		(DateTimeOffset Time, long Trades, long Preorders) last = samples[samples.Count - 1];
		DateTimeOffset cutoff = last.Time - window;
		(DateTimeOffset Time, long Trades, long Preorders)? before = null;
		(DateTimeOffset Time, long Trades, long Preorders)? after = null;
		foreach ((DateTimeOffset Time, long Trades, long Preorders) sample in samples.Take(samples.Count - 1))
		{
			if (sample.Time <= cutoff)
			{
				before = sample;
			}
			if (sample.Time >= cutoff)
			{
				after = sample;
				break;
			}
		}
		if (before.HasValue && after.HasValue)
		{
			double baseline = InterpolateTradeCount(before.Value, after.Value, cutoff);
			if (double.IsNaN(baseline) || last.Trades < baseline)
			{
				return null;
			}
			return (long)Math.Round(last.Trades - baseline);
		}
		(DateTimeOffset Time, long Trades, long Preorders) first = samples[0];
		double coverageHours = (last.Time - first.Time).TotalHours;
		double minimumCoverageHours = Math.Min(24.0, Math.Max(6.0, window.TotalHours * 0.35));
		if (coverageHours < minimumCoverageHours || last.Trades < first.Trades)
		{
			return null;
		}
		return (long)Math.Round((double)(last.Trades - first.Trades) * window.TotalHours / coverageHours);
	}

	private static double? EstimateOutfitSalesPerDay(IReadOnlyList<(DateTimeOffset Time, long Trades, long Preorders)> samples, long? sales24Hours, long? sales3Days, long? sales7Days)
	{
		double weightedRate = 0.0;
		double totalWeight = 0.0;
		if (sales24Hours.HasValue)
		{
			weightedRate += sales24Hours.Value * 0.55;
			totalWeight += 0.55;
		}
		if (sales3Days.HasValue)
		{
			weightedRate += ((double)sales3Days.Value / 3.0) * 0.3;
			totalWeight += 0.3;
		}
		if (sales7Days.HasValue)
		{
			weightedRate += ((double)sales7Days.Value / 7.0) * 0.15;
			totalWeight += 0.15;
		}
		double? smoothedRate = EstimateSmoothedOutfitSalesPerDay(samples);
		if (totalWeight > 0.0)
		{
			double windowRate = weightedRate / totalWeight;
			return smoothedRate.HasValue ? (windowRate * 0.7 + smoothedRate.Value * 0.3) : windowRate;
		}
		if (smoothedRate.HasValue)
		{
			return smoothedRate;
		}
		if (samples.Count < 2)
		{
			return null;
		}
		(DateTimeOffset, long, long) first = samples[0];
		(DateTimeOffset, long, long) last = samples[samples.Count - 1];
		double hours = (last.Item1 - first.Item1).TotalHours;
		if (hours < 6.0)
		{
			return null;
		}
		return Math.Max(0L, last.Item2 - first.Item2) * 24.0 / hours;
	}

	private static double InterpolateTradeCount((DateTimeOffset Time, long Trades, long Preorders) before, (DateTimeOffset Time, long Trades, long Preorders) after, DateTimeOffset target)
	{
		double hours = (after.Time - before.Time).TotalHours;
		if (hours <= 0.0 || after.Trades < before.Trades)
		{
			return double.NaN;
		}
		double progress = Math.Clamp((target - before.Time).TotalHours / hours, 0.0, 1.0);
		return before.Trades + (after.Trades - before.Trades) * progress;
	}

	private static double? EstimateSmoothedOutfitSalesPerDay(IReadOnlyList<(DateTimeOffset Time, long Trades, long Preorders)> samples)
	{
		if (samples.Count < 2)
		{
			return null;
		}
		double? smoothedRate = null;
		for (int i = 1; i < samples.Count; i++)
		{
			(DateTimeOffset Time, long Trades, long Preorders) previous = samples[i - 1];
			(DateTimeOffset Time, long Trades, long Preorders) current = samples[i];
			double hours = (current.Time - previous.Time).TotalHours;
			long delta = current.Trades - previous.Trades;
			if (hours < 1.0 || hours > 96.0 || delta < 0)
			{
				continue;
			}
			double rate = delta * 24.0 / hours;
			smoothedRate = smoothedRate.HasValue ? (smoothedRate.Value * 0.55 + rate * 0.45) : rate;
		}
		return smoothedRate;
	}

	private static double CalculateOutfitConfidence(IReadOnlyList<(DateTimeOffset Time, long Trades, long Preorders)> samples, DateTimeOffset? lastDetailedUtc, long? sales24Hours, long? sales3Days, long? sales7Days)
	{
		if (samples.Count < 2)
		{
			return 0.0;
		}
		double sampleFactor = Math.Min(1.0, Math.Sqrt((double)samples.Count / 12.0));
		double windowFactor = (sales24Hours.HasValue ? 0.4 : 0.0) + (sales3Days.HasValue ? 0.35 : 0.0) + (sales7Days.HasValue ? 0.25 : 0.0);
		double ageFactor = 0.0;
		if (lastDetailedUtc.HasValue)
		{
			double ageHours = Math.Max(0.0, (DateTimeOffset.UtcNow - lastDetailedUtc.Value).TotalHours);
			ageFactor = Math.Clamp(1.0 - Math.Max(0.0, ageHours - 6.0) / 42.0, 0.0, 1.0);
		}
		double spacingFactor = CalculateSampleSpacingQuality(samples);
		return Math.Clamp(sampleFactor * 0.35 + windowFactor * 0.35 + ageFactor * 0.2 + spacingFactor * 0.1, 0.0, 1.0);
	}

	private static double CalculateSampleSpacingQuality(IReadOnlyList<(DateTimeOffset Time, long Trades, long Preorders)> samples)
	{
		if (samples.Count < 3)
		{
			return 0.5;
		}
		List<double> intervals = new List<double>();
		for (int i = 1; i < samples.Count; i++)
		{
			double hours = (samples[i].Time - samples[i - 1].Time).TotalHours;
			if (hours > 0.0)
			{
				intervals.Add(hours);
			}
		}
		if (intervals.Count == 0)
		{
			return 0.0;
		}
		double average = intervals.Average();
		double deviation = intervals.Average(x => Math.Abs(x - average));
		return Math.Clamp(1.0 - deviation / Math.Max(1.0, average), 0.0, 1.0);
	}

	private static string NormalizeRegion(string region)
	{
		return string.Equals(region, "na", StringComparison.OrdinalIgnoreCase) ? "na" : "eu";
	}

	private static async Task InsertSnapshotAsync(SqliteConnection connection, SqliteTransaction transaction, TrackedItem item, DateTimeOffset timestamp, long price, long? stock, long? tradeCount, long min, long max, double average, string source, CancellationToken cancellationToken)
	{
		await using SqliteCommand command = connection.CreateCommand();
		command.Transaction = transaction;
		command.CommandText = "INSERT OR IGNORE INTO snapshots(\n    item_id,enhancement,region,captured_utc,price,stock,trade_count,\n    order_book_min,order_book_max,order_book_average,source)\nVALUES($id,$enhancement,$region,$captured,$price,$stock,$trades,$min,$max,$average,$source);";
		command.Parameters.AddWithValue("$id", item.ItemId);
		command.Parameters.AddWithValue("$enhancement", item.Enhancement);
		command.Parameters.AddWithValue("$region", item.Region);
		command.Parameters.AddWithValue("$captured", timestamp.ToString("O"));
		command.Parameters.AddWithValue("$price", price);
		command.Parameters.AddWithValue("$stock", ((object)stock) ?? DBNull.Value);
		command.Parameters.AddWithValue("$trades", ((object)tradeCount) ?? DBNull.Value);
		command.Parameters.AddWithValue("$min", min);
		command.Parameters.AddWithValue("$max", max);
		command.Parameters.AddWithValue("$average", average);
		command.Parameters.AddWithValue("$source", source);
		await command.ExecuteNonQueryAsync(cancellationToken);
	}

	private static async Task<SalesWindow> GetSalesWindowAsync(SqliteConnection connection, long itemId, int enhancement, string region, string label, TimeSpan window, CancellationToken cancellationToken)
	{
		DateTimeOffset cutoff = DateTimeOffset.UtcNow.Subtract(window);
		List<(DateTimeOffset Time, long Count)> samples = new List<(DateTimeOffset, long)>();
		SalesWindow result;
		await using (SqliteCommand command = connection.CreateCommand())
		{
			command.CommandText = "SELECT captured_utc, trade_count FROM snapshots\nWHERE item_id=$id AND enhancement=$enhancement AND region=$region\n  AND trade_count IS NOT NULL\nORDER BY captured_utc;";
			command.Parameters.AddWithValue("$id", itemId);
			command.Parameters.AddWithValue("$enhancement", enhancement);
			command.Parameters.AddWithValue("$region", region);
			SalesWindow salesWindow;
			await using (SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
			{
				while (await reader.ReadAsync(cancellationToken))
				{
					samples.Add((DateTimeOffset.Parse(reader.GetString(0)), reader.GetInt64(1)));
				}
				if (samples.Count < 2)
				{
					salesWindow = new SalesWindow(label, null, Complete: false, 0.0);
				}
				else
				{
					(DateTimeOffset, long) tuple = samples[samples.Count - 1];
					(DateTimeOffset, long) tuple2 = samples.LastOrDefault(((DateTimeOffset Time, long Count) x) => x.Time <= cutoff);
					(DateTimeOffset, long) tuple3 = tuple2;
					if (tuple3.Item1 == default(DateTimeOffset) && tuple3.Item2 == 0L)
					{
						double coverageHours = Math.Max(0.0, (tuple.Item1 - samples[0].Time).TotalHours);
						salesWindow = new SalesWindow(label, null, Complete: false, coverageHours);
					}
					else
					{
						salesWindow = new SalesWindow(label, Math.Max(0L, tuple.Item2 - tuple2.Item2), Complete: true, Math.Max(0.0, (tuple.Item1 - tuple2.Item1).TotalHours));
					}
				}
			}
			result = salesWindow;
		}
		return result;
	}

	private async Task<SqliteConnection> OpenAsync(CancellationToken cancellationToken)
	{
		SqliteConnection connection = new SqliteConnection(connectionString);
		await connection.OpenAsync(cancellationToken);
		return connection;
	}

	private static string EscapeCsv(string value)
	{
		if (value.IndexOfAny(new char[4] { ',', '"', '\r', '\n' }) < 0)
		{
			return value;
		}
		return "\"" + value.Replace("\"", "\"\"") + "\"";
	}
}

