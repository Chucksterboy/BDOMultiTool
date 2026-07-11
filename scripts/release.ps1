param(
	[Parameter(Mandatory = $true)]
	[string]$Version,

	[string]$Repository = "Chucksterboy/BDOMultiTool",

	[string]$Notes = "",

	[switch]$Draft
)

$ErrorActionPreference = "Stop"

function Resolve-ToolPath {
	param(
		[string]$Command,
		[string[]]$Fallbacks
	)

	$found = Get-Command $Command -ErrorAction SilentlyContinue
	if ($found) {
		return $found.Source
	}

	foreach ($fallback in $Fallbacks) {
		$match = Get-ChildItem -Path $fallback -ErrorAction SilentlyContinue | Select-Object -Last 1
		if ($match) {
			return $match.FullName
		}
	}

	throw "Could not find required tool: $Command"
}

function Resolve-DotnetSdkPath {
	param([string]$RepoRoot)

	$candidates = @(
		(Join-Path $RepoRoot ".dotnet-sdk\dotnet.exe"),
		"$env:ProgramFiles\dotnet\dotnet.exe"
	)
	$pathCommand = Get-Command "dotnet" -ErrorAction SilentlyContinue
	if ($pathCommand) {
		$candidates = @($pathCommand.Source) + $candidates
	}

	foreach ($candidate in ($candidates | Select-Object -Unique)) {
		if (!(Test-Path -LiteralPath $candidate)) {
			continue
		}

		$sdks = & $candidate --list-sdks 2>$null
		if ($LASTEXITCODE -eq 0 -and ![string]::IsNullOrWhiteSpace(($sdks | Out-String))) {
			return $candidate
		}
	}

	throw "Could not find a .NET SDK. Install the .NET 8 SDK or place it in .dotnet-sdk."
}

function Normalize-Version {
	param([string]$Value)
	$clean = $Value.Trim()
	if ($clean.StartsWith("v", [System.StringComparison]::OrdinalIgnoreCase)) {
		return "v" + $clean.Substring(1)
	}

	return "v" + $clean
}

function Get-Assembly-Version {
	param([string]$Value)
	$clean = $Value.Trim().TrimStart("v", "V")
	$parts = @($clean.Split(".") | Where-Object { $_ -ne "" })
	while ($parts.Count -lt 4) {
		$parts += "0"
	}

	return ($parts[0..3] -join ".")
}

function Replace-Text {
	param(
		[string]$Path,
		[string]$Pattern,
		[string]$Replacement
	)

	$text = Get-Content -LiteralPath $Path -Raw
	if (![regex]::IsMatch($text, $Pattern)) {
		throw "No replacement was made in $Path"
	}

	$newText = [regex]::Replace($text, $Pattern, $Replacement)
	Set-Content -LiteralPath $Path -Value $newText -Encoding UTF8
}

$versionTag = Normalize-Version $Version
$assemblyVersion = Get-Assembly-Version $versionTag
$repoRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot "..")).Path
$sourceRoot = Join-Path $repoRoot "Source Code"
$projectFile = Join-Path $sourceRoot "BDO Multi-Tool.csproj"
$installerProject = Join-Path $sourceRoot "InstallerSource\BDOMultiToolInstaller\BDOMultiToolInstaller.csproj"
$installerPayload = Join-Path $sourceRoot "InstallerSource\BDOMultiToolInstaller\Payload.zip"
$appVersionFile = Join-Path $sourceRoot "BDOMultiTool\AppVersion.cs"
$assemblyInfoFile = Join-Path $sourceRoot "Properties\AssemblyInfo.cs"
$updateManifestFile = Join-Path $repoRoot "update.json"
$artifactRoot = Join-Path $repoRoot "artifacts"
$appOut = Join-Path $artifactRoot "App Files"
$installerOut = Join-Path $artifactRoot "Installer"
$installerExe = Join-Path $installerOut "BDO Multi-Tool Installer.exe"
$installerReleaseAsset = Join-Path $installerOut "BDO-Multi-Tool-Installer.exe"
$installerAssetName = Split-Path $installerReleaseAsset -Leaf

$dotnet = Resolve-DotnetSdkPath $repoRoot
$git = Resolve-ToolPath "git" @("$env:LOCALAPPDATA\GitHubDesktop\app-*\resources\app\git\cmd\git.exe")
$gh = Resolve-ToolPath "gh" @("$env:LOCALAPPDATA\Microsoft\WinGet\Packages\GitHub.cli_Microsoft.Winget.Source_8wekyb3d8bbwe\bin\gh.exe")

Set-Location $repoRoot

Write-Host "Preparing BDO Multi-Tool $versionTag"

Replace-Text $appVersionFile 'public const string Current = "v[^"]+";' ('public const string Current = "' + $versionTag + '";')
Replace-Text $assemblyInfoFile 'AssemblyFileVersion\("[^"]+"\)' ('AssemblyFileVersion("' + $assemblyVersion + '")')
Replace-Text $assemblyInfoFile 'AssemblyInformationalVersion\("[^"]+"\)' ('AssemblyInformationalVersion("' + $versionTag + '")')
Replace-Text $assemblyInfoFile 'AssemblyVersion\("[^"]+"\)' ('AssemblyVersion("' + $assemblyVersion + '")')

$manifest = [ordered]@{
	version = $versionTag
	releaseUrl = "https://github.com/$Repository/releases/latest"
	downloadUrl = "https://github.com/$Repository/releases/latest/download/$installerAssetName"
	notes = if ([string]::IsNullOrWhiteSpace($Notes)) { "BDO Multi-Tool $versionTag release." } else { $Notes }
}
$manifest | ConvertTo-Json | Set-Content -LiteralPath $updateManifestFile -Encoding UTF8
Copy-Item -LiteralPath $updateManifestFile -Destination (Join-Path $sourceRoot "update.json") -Force

if (Test-Path -LiteralPath $artifactRoot) {
	Remove-Item -LiteralPath $artifactRoot -Recurse -Force
}
New-Item -ItemType Directory -Path $appOut -Force | Out-Null
New-Item -ItemType Directory -Path $installerOut -Force | Out-Null

& $dotnet publish $projectFile -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=false -p:PublishReadyToRun=false -o $appOut
if ($LASTEXITCODE -ne 0) {
	throw "Application publish failed."
}

Get-ChildItem -LiteralPath $appOut -Recurse -File | Where-Object { $_.Extension -in @(".pdb", ".xml") } | Remove-Item -Force
$runtimes = Join-Path $appOut "runtimes"
if (Test-Path -LiteralPath $runtimes) {
	Get-ChildItem -LiteralPath $runtimes -Directory | Where-Object { $_.Name -ne "win-x64" } | Remove-Item -Recurse -Force
}

if (Test-Path -LiteralPath $installerPayload) {
	Remove-Item -LiteralPath $installerPayload -Force
}
Compress-Archive -Path (Join-Path $appOut "*") -DestinationPath $installerPayload -CompressionLevel Optimal -Force

& $dotnet publish $installerProject -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishReadyToRun=false -o $installerOut
if ($LASTEXITCODE -ne 0) {
	throw "Installer publish failed."
}

Get-ChildItem -LiteralPath $installerOut -Recurse -File | Where-Object { $_.Extension -in @(".pdb", ".xml") } | Remove-Item -Force
if (!(Test-Path -LiteralPath $installerExe)) {
	throw "Installer was not created: $installerExe"
}
Copy-Item -LiteralPath $installerExe -Destination $installerReleaseAsset -Force

Remove-Item -LiteralPath $installerPayload -Force -ErrorAction SilentlyContinue

& $gh auth status | Out-Host
if ($LASTEXITCODE -ne 0) {
	throw "GitHub CLI is not logged in."
}

& $git add --all
& $git commit -m "Release $versionTag"
if ($LASTEXITCODE -ne 0) {
	throw "Git commit failed."
}

& $git tag -a $versionTag -m "BDO Multi-Tool $versionTag"
if ($LASTEXITCODE -ne 0) {
	throw "Git tag failed. The tag may already exist."
}

& $git push origin main
if ($LASTEXITCODE -ne 0) {
	throw "Git push failed."
}

& $git push origin $versionTag
if ($LASTEXITCODE -ne 0) {
	throw "Git tag push failed."
}

$releaseArgs = @("release", "create", $versionTag, $installerReleaseAsset, "--repo", $Repository, "--title", "BDO Multi-Tool $versionTag", "--notes")
$releaseNotes = if ([string]::IsNullOrWhiteSpace($Notes)) {
	"BDO Multi-Tool $versionTag release."
} else {
	$Notes
}
$releaseArgs += $releaseNotes
if ($Draft) {
	$releaseArgs += "--draft"
} else {
	$releaseArgs += "--latest"
}

& $gh @releaseArgs
if ($LASTEXITCODE -ne 0) {
	throw "GitHub release creation failed."
}

Write-Host "Release complete: $versionTag"
Write-Host "Installer: $installerReleaseAsset"
