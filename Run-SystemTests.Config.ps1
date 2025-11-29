# System Test Configuration
# This file contains configuration values for Run-SystemTests.ps1

$Config = @{
    # Test Configuration
    TestCommand = "dotnet test"
    # TestCommand = "dotnet test --filter `"FullyQualifiedName~SmokeTests`""
    TestReportPath = "system-test\TestResults\testResults.html"
}

# Export the configuration
return $Config

