using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOTradeDistanceCalculator;

internal sealed record AppBehaviorSettings(bool MinimizeToTray)
{
	public static AppBehaviorSettings Default => new AppBehaviorSettings(true);

	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true,
		WriteIndented = true
	};

	public static async Task<AppBehaviorSettings> LoadAsync(AppPaths paths, CancellationToken cancellationToken)
	{
		try
		{
			if (!File.Exists(paths.AppBehaviorSettingsPath))
				return Default;

			return JsonSerializer.Deserialize<AppBehaviorSettings>(
				await File.ReadAllTextAsync(paths.AppBehaviorSettingsPath, cancellationToken),
				JsonOptions) ?? Default;
		}
		catch
		{
			return Default;
		}
	}

	public static async Task<AppBehaviorSettings> SaveAsync(AppPaths paths, AppBehaviorSettings settings, CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(paths.Root);
		await File.WriteAllTextAsync(paths.AppBehaviorSettingsPath, JsonSerializer.Serialize(settings, JsonOptions), cancellationToken);
		return settings;
	}
}
