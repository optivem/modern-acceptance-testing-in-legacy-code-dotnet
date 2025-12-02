# System Test Configuration
# This file contains configuration values for Run-SystemTests.ps1

$Config = @{
    # Test Configuration
    TestInstallCommands = @(
        "pwsh bin/Debug/net8.0/playwright.ps1 install"
    )
    SmokeTestCommand = "dotnet test --filter 'FullyQualifiedName~SmokeTests' --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'"
    E2ETestCommand = "dotnet test --filter 'FullyQualifiedName~E2eTests' --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'" 

    TestReportPath = "TestResults\testResults.html"
}

# Export the configuration
return $Config

