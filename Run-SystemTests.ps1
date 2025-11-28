param(
    [Parameter(Position=0)]
    [ValidateSet("local", "pipeline")]
    [string]$Mode = "local",

    [switch]$TestOnly,

    [int]$LogLines = 50
)


Write-Host "Hello World"

# Download the script from GitHub
$TempScriptPath = Join-Path $env:TEMP "Run-SystemTests-Downloaded.ps1"
$GitHubScriptUrl = "https://raw.githubusercontent.com/optivem/modern-acceptance-testing-in-legacy-code/main/scripts/Run-SystemTests.ps1"

Write-Host "Downloading script from: $GitHubScriptUrl" -ForegroundColor Cyan
Invoke-WebRequest -Uri $GitHubScriptUrl -OutFile $TempScriptPath

Write-Host "Executing downloaded script..." -ForegroundColor Cyan
& $TempScriptPath -Mode $Mode -TestOnly:$TestOnly -LogLines $LogLines

# Clean up temp file
Write-Host "Cleaning up temporary file..." -ForegroundColor Cyan
Remove-Item $TempScriptPath -Force

Write-Host "Done!" -ForegroundColor Green
