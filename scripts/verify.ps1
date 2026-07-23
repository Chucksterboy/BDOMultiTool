param([switch]$SkipBuild)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$dotnetCandidates = @(
	(Join-Path $repoRoot ".dotnet-sdk\dotnet.exe"),
	"$env:ProgramFiles\dotnet\dotnet.exe"
)
$dotnet = $dotnetCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (!$dotnet) {
	$dotnetCommand = Get-Command dotnet -ErrorAction SilentlyContinue
	if ($dotnetCommand) { $dotnet = $dotnetCommand.Source }
}
if (!$dotnet) { throw "A .NET 8 SDK is required." }

$project = Join-Path $repoRoot "Source Code\BDO Multi-Tool.csproj"
$sourceRoot = Join-Path $repoRoot "Source Code"
$htmlPath = Join-Path $sourceRoot "BDOMultiTool.Resources.BDO_Multi_Tool.html"
$cssPath = Join-Path $sourceRoot "BDOMultiTool.Resources.BDO_Multi_Tool.css"
$scriptPath = Join-Path $sourceRoot "BDOMultiTool.Resources.BDO_Multi_Tool.js"
$grindDataPath = Join-Path $sourceRoot "Assets\GrindTracker\grind-spots.js"

if (!$SkipBuild) {
	& $dotnet build $project -c Release -p:EnableNETAnalyzers=true -p:AnalysisLevel=latest -p:WarningLevel=9999 --nologo
	if ($LASTEXITCODE -ne 0) { throw "Application build failed." }
}

foreach ($path in @($htmlPath, $cssPath, $scriptPath, $grindDataPath)) {
	if (!(Test-Path -LiteralPath $path)) { throw "Required UI asset is missing: $path" }
}

$html = Get-Content -LiteralPath $htmlPath -Raw
$css = Get-Content -LiteralPath $cssPath -Raw
$script = Get-Content -LiteralPath $scriptPath -Raw
if ($html -notmatch 'BDOMultiTool\.Resources\.BDO_Multi_Tool\.css' -or $html -notmatch 'BDOMultiTool\.Resources\.BDO_Multi_Tool\.js') {
	throw "The HTML shell does not reference the external UI assets."
}

$htmlIds = [regex]::Matches($html, '\bid="([^"]+)"') | ForEach-Object { $_.Groups[1].Value }
$duplicateIds = $htmlIds | Group-Object | Where-Object Count -gt 1
if ($duplicateIds) {
	throw "Duplicate HTML ids: $($duplicateIds.Name -join ', ')"
}
$literalElementIds = [regex]::Matches($script, 'getElementById\(\s*["'']([^"'']+)["'']\s*\)') |
	ForEach-Object { $_.Groups[1].Value } |
	Sort-Object -Unique
$missingElementIds = $literalElementIds | Where-Object { $_ -notin $htmlIds }
if ($missingElementIds) {
	throw "JavaScript references missing HTML ids: $($missingElementIds -join ', ')"
}

$textExtensions = @(".cs", ".csproj", ".css", ".html", ".js", ".json", ".md", ".ps1")
$textFiles = Get-ChildItem -LiteralPath $sourceRoot, (Join-Path $repoRoot "scripts") -Recurse -File |
	Where-Object {
		$_.Extension -in $textExtensions -and
		$_.FullName -notmatch '\\(?:bin|obj)\\'
	}
$mojibakeFiles = foreach ($file in $textFiles) {
	$content = [System.IO.File]::ReadAllText($file.FullName)
	if ($content -match '[\u00C2\u00C3\uFFFD]|\u00E2\u20AC|\u00EF\u00BF\u00BD') {
		$file.FullName
	}
}
if ($mojibakeFiles) {
	throw "Mojibake or replacement characters found in: $($mojibakeFiles -join ', ')"
}
if ($html -match 'Discord:\s*Chucksterboy|Cyber\s*-\s*notifications on' -or $script -match 'Discord:\s*Chucksterboy|Cyber\s*-\s*notifications on') {
	throw "Retired status-bar content was reintroduced."
}

$grindData = Get-Content -LiteralPath $grindDataPath -Raw
$grindAssignment = [regex]::Match(
	$grindData,
	'(?s)window\.BDO_GRIND_SPOTS\s*=\s*(\[.*\])\s*;\s*$'
)
if (!$grindAssignment.Success) { throw "The grind-spot data assignment is malformed." }
$grindSpots = $grindAssignment.Groups[1].Value | ConvertFrom-Json
if ($grindSpots.Count -lt 90) {
	throw "The grind-spot catalog unexpectedly contains only $($grindSpots.Count) spots."
}
$duplicateSpotIds = $grindSpots | Group-Object { [string]$_.id } | Where-Object Count -gt 1
if ($duplicateSpotIds) {
	throw "Duplicate grind-spot ids: $($duplicateSpotIds.Name -join ', ')"
}

$missingGrindAssets = [System.Collections.Generic.HashSet[string]]::new(
	[System.StringComparer]::OrdinalIgnoreCase
)
foreach ($spot in $grindSpots) {
	if ([string]::IsNullOrWhiteSpace([string]$spot.trashId) -or
		[string]::IsNullOrWhiteSpace([string]$spot.primaryTrash)) {
		throw "Grind spot '$($spot.name)' is missing primary trash metadata."
	}
	if (!$spot.drops -or $spot.drops.Count -eq 0) {
		throw "Grind spot '$($spot.name)' has no loot table."
	}
	$trashDrop = $spot.drops | Where-Object {
		[string]$_.id -eq [string]$spot.trashId -and $_.isTrash -eq $true
	} | Select-Object -First 1
	if (!$trashDrop) {
		throw "Grind spot '$($spot.name)' does not contain its primary trash drop."
	}
	$duplicateDropIds = $spot.drops | Group-Object { [string]$_.id } | Where-Object Count -gt 1
	if ($duplicateDropIds) {
		throw "Grind spot '$($spot.name)' contains duplicate drop ids: $($duplicateDropIds.Name -join ', ')"
	}
	foreach ($assetPath in @($spot.icon) + @($spot.drops | ForEach-Object { $_.icon })) {
		if ([string]::IsNullOrWhiteSpace([string]$assetPath)) {
			throw "Grind spot '$($spot.name)' contains an empty icon path."
		}
		$absoluteAssetPath = Join-Path $sourceRoot ([string]$assetPath -replace '/', '\')
		if (!(Test-Path -LiteralPath $absoluteAssetPath -PathType Leaf)) {
			[void]$missingGrindAssets.Add([string]$assetPath)
		}
	}
}
if ($missingGrindAssets.Count -gt 0) {
	throw "Missing Grind Tracker icons: $([string]::Join(', ', $missingGrindAssets))"
}

$referencedGrindAssets = [regex]::Matches(
	$script,
	'Assets/GrindTracker/(?:classes|buffs)/[^"'']+\.png'
) |
	ForEach-Object { $_.Value } |
	Where-Object { $_ -notmatch '\$\{' } |
	Sort-Object -Unique
$referencedGrindAssets += 1..40 | ForEach-Object {
	"Assets/GrindTracker/buffs/buff-$($_.ToString('00')).png"
}
$missingReferencedAssets = foreach ($assetPath in $referencedGrindAssets) {
	$absoluteAssetPath = Join-Path $sourceRoot ($assetPath -replace '/', '\')
	if (!(Test-Path -LiteralPath $absoluteAssetPath -PathType Leaf)) { $assetPath }
}
if ($missingReferencedAssets) {
	throw "Missing class or modifier icons: $($missingReferencedAssets -join ', ')"
}

$homeTimerIconCount = [regex]::Matches($html, 'class="homeTimerIcon"[^>]*>\s*<svg\b').Count
$resetTimerIconCount = [regex]::Matches($script, '(?m)^\s{2}(?:daily|imperial|bsa|agris|barter|trading):''<svg\b').Count
if ($homeTimerIconCount -ne 5 -or $resetTimerIconCount -ne 6 -or $script -match 'icon:\s*"\?"') {
	throw "Dashboard timer badges are missing, malformed, or using placeholder glyphs."
}
if ($html.Length -gt 100000) { throw "The HTML shell exceeded the 100 KB performance budget." }
if ($script.Length -gt 500000) { throw "The main UI script exceeded the 500 KB performance budget." }
if ($css -notmatch 'body\[data-motion="reduced"\]' -or $script -notmatch 'visibilitychange') {
	throw "Reduced-motion or visibility lifecycle handling is missing."
}

$dayNightMatch = [regex]::Match($script, 'dayNight:\{\s*cycleMinutes:(\d+),\s*nightMinutes:(\d+),\s*nightStartUtcIso:"([^"]+)"')
if (!$dayNightMatch.Success) { throw "The BDO day/night cycle must use a fixed UTC anchor." }
$cycleMs = [int64]$dayNightMatch.Groups[1].Value * 60000
$nightMs = [int64]$dayNightMatch.Groups[2].Value * 60000
$nightStart = [DateTimeOffset]::Parse($dayNightMatch.Groups[3].Value).ToUnixTimeMilliseconds()
$observedAt = [DateTimeOffset]::Parse('2026-07-20T11:52:53Z').ToUnixTimeMilliseconds()
$elapsed = (($observedAt - $nightStart) % $cycleMs + $cycleMs) % $cycleMs
$remaining = $nightMs - $elapsed
if ($elapsed -lt 0 -or $elapsed -ge $nightMs -or $remaining -lt (26 * 60000) -or $remaining -gt (28 * 60000)) {
	throw "The BDO day/night cycle no longer matches the live EU phase captured on 2026-07-20."
}

$functionNames = [regex]::Matches($script, '(?m)^(?:async\s+)?function\s+([A-Za-z_$][\w$]*)\s*\(') |
	ForEach-Object { $_.Groups[1].Value }
$duplicates = $functionNames | Group-Object | Where-Object Count -gt 1
if ($duplicates) {
	throw "Duplicate JavaScript function declarations: $($duplicates.Name -join ', ')"
}
$unusedFunctions = foreach ($functionName in $functionNames) {
	$references = [regex]::Matches($script, "\b$([regex]::Escape($functionName))\b").Count
	if ($references -eq 1) { $functionName }
}
if ($unusedFunctions) {
	throw "Unreferenced JavaScript functions: $($unusedFunctions -join ', ')"
}

$calculatorSource = Get-Content -LiteralPath (Join-Path $sourceRoot "BDOMultiTool\CalculatorForm.cs") -Raw
if ($calculatorSource -match 'CancellationToken\.None') {
	throw "CalculatorForm contains an uncancellable host operation."
}
$bridgeCommands = @(
	[regex]::Matches($script, 'bridgeCall\(\s*["'']([^"'']+)["'']') |
		ForEach-Object { $_.Groups[1].Value }
) + @("initializeEvents", "refreshEvents")
$hostCommands = [regex]::Matches($calculatorSource, 'case\s+"([^"]+)"\s*:') |
	ForEach-Object { $_.Groups[1].Value }
$missingHostCommands = Compare-Object `
	($bridgeCommands | Sort-Object -Unique) `
	($hostCommands | Sort-Object -Unique) |
	Where-Object SideIndicator -eq "<=" |
	ForEach-Object InputObject
$unusedHostCommands = Compare-Object `
	($bridgeCommands | Sort-Object -Unique) `
	($hostCommands | Sort-Object -Unique) |
	Where-Object SideIndicator -eq "=>" |
	ForEach-Object InputObject
if ($missingHostCommands) {
	throw "JavaScript bridge commands without host handlers: $($missingHostCommands -join ', ')"
}
if ($unusedHostCommands) {
	throw "Host bridge handlers without JavaScript callers: $($unusedHostCommands -join ', ')"
}
if ($script -notmatch 'bdoMultiTool\.grindTrackerSessionsRecovery' -or
	$script -match 'localStorage\.setItem\("grindTrackerSessionsRecovery"') {
	throw "Grind Tracker emergency recovery is not using the namespaced storage key."
}

$node = Get-Command node -ErrorAction SilentlyContinue
if ($node) {
	& $node.Source --check $scriptPath
	if ($LASTEXITCODE -ne 0) { throw "JavaScript syntax validation failed." }
}

$appDll = Join-Path $sourceRoot "bin\Release\net8.0-windows10.0.19041.0\BDO Multi-Tool.dll"
& $dotnet $appDll --offline-smoke-test
if ($LASTEXITCODE -ne 0) { throw "Offline application smoke test failed with exit code $LASTEXITCODE." }

Write-Host "Verification passed: build, offline smoke test, DOM wiring, data integrity, UI assets, performance budgets, cancellation, and duplicate-function checks."
