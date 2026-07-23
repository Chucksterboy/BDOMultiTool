using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

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
		return await AtomicFile.ReadJsonAsync<AppBehaviorSettings>(
			paths.AppBehaviorSettingsPath,
			JsonOptions,
			cancellationToken) ?? Default;
	}

	public static async Task<AppBehaviorSettings> SaveAsync(AppPaths paths, AppBehaviorSettings settings, CancellationToken cancellationToken)
	{
		await AtomicFile.WriteAllTextAsync(paths.AppBehaviorSettingsPath, JsonSerializer.Serialize(settings, JsonOptions), cancellationToken);
		return settings;
	}
}

