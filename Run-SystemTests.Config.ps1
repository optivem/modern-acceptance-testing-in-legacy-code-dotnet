# System Test Configuration
# This file contains configuration values for Run-SystemTests.ps1

$Config = @{

    Tests = @(
        @{  Id = "smoke";
            Name = "Smoke Tests";
            Command = "dotnet test --filter 'FullyQualifiedName~SmokeTests' --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "system-test";
            TestReportPath = "TestResults\testResults.html"
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install"; },
        @{ 
            Id = "e2e";
            Name = "E2E Tests";
            Command = "dotnet test --filter 'FullyQualifiedName~E2eTests' --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "system-test";
            TestReportPath = "TestResults\testResults.html";
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install";  }
    )
}

# Export the configuration
return $Config

