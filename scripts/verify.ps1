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

$functionNames = [regex]::Matches($script, '(?m)^(?:async\s+)?function\s+([A-Za-z_$][\w$]*)\s*\(') |
	ForEach-Object { $_.Groups[1].Value }
$duplicates = $functionNames | Group-Object | Where-Object Count -gt 1
if ($duplicates) {
	throw "Duplicate JavaScript function declarations: $($duplicates.Name -join ', ')"
}

$calculatorSource = Get-Content -LiteralPath (Join-Path $sourceRoot "BDOMultiTool\CalculatorForm.cs") -Raw
if ($calculatorSource -match 'CancellationToken\.None') {
	throw "CalculatorForm contains an uncancellable host operation."
}

$appDll = Join-Path $sourceRoot "bin\Release\net8.0-windows10.0.19041.0\BDO Multi-Tool.dll"
& $dotnet $appDll --offline-smoke-test
if ($LASTEXITCODE -ne 0) { throw "Offline application smoke test failed with exit code $LASTEXITCODE." }

Write-Host "Verification passed: build, offline smoke test, UI assets, performance budgets, cancellation, and duplicate-function checks."
