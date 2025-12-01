# System Test Configuration
# This file contains configuration values for Run-SystemTests.ps1

$Config = @{
    # Test Configuration
    TestCommand = "dotnet test --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html'"
    SmokeTestCommand = "dotnet test --filter 'FullyQualifiedName~SmokeTests' --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html'"
    E2ETestCommand = "dotnet test --filter 'FullyQualifiedName~E2eTests' --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html'"

    TestReportPath = "TestResults\testResults.html"
}

# Export the configuration
return $Config

