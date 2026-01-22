# System Test Configuration
# This file contains configuration values for Run-SystemTests.ps1

$Config = @{

    BuildCommands = @(
        @{  Name = "Clean Build";
            Command = "dotnet clean; dotnet build"
        }
    )

    Tests = @(
        @{  Id = "smoke";
            Name = "Smoke Tests";
            Command = "dotnet test --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/SmokeTests";
            TestReportPath = "SystemTests\SmokeTests\TestResults\testResults.html"
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install"; },
        @{ 
            Id = "e2e";
            Name = "E2E Tests";
            Command = "dotnet test --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/E2eTests";
            TestReportPath = "SystemTests\E2eTests\TestResults\testResults.html";
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install";  }
    )
}

# Export the configuration
return $Config

