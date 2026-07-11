using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class UpdateCheckerService : IDisposable
{
	private static readonly JsonSerializerOptions ManifestJsonOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	private readonly AppLogger logger;
	private readonly HttpClient http;

	public UpdateCheckerService(AppLogger logger)
	{
		this.logger = logger;
		http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
		http.DefaultRequestHeaders.UserAgent.ParseAdd("BDO-Multi-Tool/" + AppVersion.Current);
	}

	public async Task<UpdateCheckResult> CheckAsync(CancellationToken cancellationToken)
	{
		UpdateCheckResult? manifestResult = await TryCheckManifestAsync(cancellationToken);
		if (manifestResult != null)
			return manifestResult;

		UpdateCheckResult? releaseResult = await TryCheckLatestReleaseAsync(cancellationToken);
		if (releaseResult != null)
			return releaseResult;

		return new UpdateCheckResult(
			UpdateAvailable: false,
			CurrentVersion: AppVersion.Current,
			LatestVersion: AppVersion.Current,
			Url: AppVersion.ReleasesUrl,
			RepositoryUrl: AppVersion.RepositoryUrl,
			Message: "Could not check for updates right now.",
			CheckFailed: true);
	}

	private async Task<UpdateCheckResult?> TryCheckManifestAsync(CancellationToken cancellationToken)
	{
		try
		{
			using HttpResponseMessage response = await http.GetAsync(AppVersion.ManifestUrl, cancellationToken);
			if (!response.IsSuccessStatusCode)
				return null;

			UpdateManifest? manifest = await response.Content.ReadFromJsonAsync<UpdateManifest>(ManifestJsonOptions, cancellationToken);
			if (manifest == null || string.IsNullOrWhiteSpace(manifest.Version))
				return null;

			string latest = NormalizeVersion(manifest.Version);
			bool updateAvailable = IsNewerVersion(latest, AppVersion.Current);
			string url = FirstNonBlank(manifest.DownloadUrl, manifest.ReleaseUrl, AppVersion.ReleasesUrl);
			return new UpdateCheckResult(
				updateAvailable,
				AppVersion.Current,
				latest,
				url,
				AppVersion.RepositoryUrl,
				updateAvailable ? $"New update {latest} is available" : "You are on the latest version.",
				CheckFailed: false);
		}
		catch (Exception ex)
		{
			logger.Warn("Update manifest check failed: " + ex.Message);
			return null;
		}
	}

	private async Task<UpdateCheckResult?> TryCheckLatestReleaseAsync(CancellationToken cancellationToken)
	{
		try
		{
			using HttpResponseMessage response = await http.GetAsync("https://api.github.com/repos/Chucksterboy/BDOMultiTool/releases/latest", cancellationToken);
			if (!response.IsSuccessStatusCode)
				return null;

			using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
			JsonElement root = document.RootElement;
			string latest = NormalizeVersion(root.TryGetProperty("tag_name", out JsonElement tagValue) ? tagValue.GetString() ?? AppVersion.Current : AppVersion.Current);
			string htmlUrl = root.TryGetProperty("html_url", out JsonElement htmlValue) ? htmlValue.GetString() ?? AppVersion.ReleasesUrl : AppVersion.ReleasesUrl;
			string downloadUrl = htmlUrl;
			if (root.TryGetProperty("assets", out JsonElement assets) && assets.ValueKind == JsonValueKind.Array)
			{
				JsonElement? firstAsset = assets.EnumerateArray().FirstOrDefault();
				if (firstAsset.HasValue && firstAsset.Value.TryGetProperty("browser_download_url", out JsonElement assetValue))
					downloadUrl = assetValue.GetString() ?? htmlUrl;
			}

			bool updateAvailable = IsNewerVersion(latest, AppVersion.Current);
			return new UpdateCheckResult(
				updateAvailable,
				AppVersion.Current,
				latest,
				downloadUrl,
				AppVersion.RepositoryUrl,
				updateAvailable ? $"New update {latest} is available" : "You are on the latest version.",
				CheckFailed: false);
		}
		catch (Exception ex)
		{
			logger.Warn("GitHub release update check failed: " + ex.Message);
			return null;
		}
	}

	private static string NormalizeVersion(string version)
	{
		string trimmed = (version ?? string.Empty).Trim();
		return trimmed.StartsWith("v", StringComparison.OrdinalIgnoreCase) ? "v" + trimmed[1..] : "v" + trimmed;
	}

	private static bool IsNewerVersion(string latest, string current)
	{
		if (TryParseVersion(latest, out Version? latestVersion) && TryParseVersion(current, out Version? currentVersion))
			return latestVersion > currentVersion;

		return !string.Equals(latest, current, StringComparison.OrdinalIgnoreCase);
	}

	private static bool TryParseVersion(string value, out Version? version)
	{
		string clean = (value ?? string.Empty).Trim().TrimStart('v', 'V');
		return Version.TryParse(clean, out version);
	}

	private static string FirstNonBlank(params string?[] values)
	{
		return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? AppVersion.ReleasesUrl;
	}

	public void Dispose() => http.Dispose();

	private sealed record UpdateManifest(string Version, string? ReleaseUrl, string? DownloadUrl);
}

internal sealed record UpdateCheckResult(
	bool UpdateAvailable,
	string CurrentVersion,
	string LatestVersion,
	string Url,
	string RepositoryUrl,
	string Message,
	bool CheckFailed);

