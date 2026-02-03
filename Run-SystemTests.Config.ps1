# System Test Configuration
# This file contains configuration values for Run-SystemTests.ps1

$Config = @{

    BuildCommands = @(
        @{  Name = "Clean Build";
            Command = "dotnet clean; dotnet build"
        }
    )

    Tests = @(

        # Smoke Tests
        @{  Id = "smoke-stub";
            Name = "Smoke Tests - Stubbed External Systems";
            Command = "dotnet test -e ENVIRONMENT=local -e EXTERNAL_SYSTEM_MODE=stub --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/SmokeTests";
            TestReportPath = "SystemTests\SmokeTests\TestResults\testResults.html"
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install"; },
        @{  Id = "smoke-real";
            Name = "Smoke Tests - Real External Systems";
            Command = "dotnet test -e ENVIRONMENT=local -e EXTERNAL_SYSTEM_MODE=real --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/SmokeTests";
            TestReportPath = "SystemTests\SmokeTests\TestResults\testResults.html"
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install"; },

        # Acceptance Tests
        @{  Id = "acceptance-api";
            Name = "Acceptance Tests - Channel: API";
            Command = "dotnet test -e ENVIRONMENT=local -e EXTERNAL_SYSTEM_MODE=stub -e CHANNEL=API --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/AcceptanceTests";
            TestReportPath = "SystemTests\AcceptanceTests\TestResults\testResults.html" },
        @{  Id = "acceptance-ui";
            Name = "Acceptance Tests - Channel: UI";
            Command = "dotnet test -e ENVIRONMENT=local -e EXTERNAL_SYSTEM_MODE=stub -e CHANNEL=UI --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/AcceptanceTests";
            TestReportPath = "SystemTests\AcceptanceTests\TestResults\testResults.html";
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install"; },

        # E2E Tests
        @{  Id = "e2e-api";
            Name = "E2E Tests - Channel: API";
            Command = "dotnet test -e ENVIRONMENT=local -e EXTERNAL_SYSTEM_MODE=real -e CHANNEL=API --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/E2eTests";
            TestReportPath = "SystemTests\E2eTests\TestResults\testResults.html";
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install"; },
        @{  Id = "e2e-ui";
            Name = "E2E Tests - Channel: UI";
            Command = "dotnet test -e ENVIRONMENT=local -e EXTERNAL_SYSTEM_MODE=real -e CHANNEL=UI --logger 'trx;LogFileName=testResults.trx' --logger 'html;LogFileName=testResults.html' --logger 'console;verbosity=detailed'";
            Path = "SystemTests/E2eTests";
            TestReportPath = "SystemTests\E2eTests\TestResults\testResults.html";
            TestInstallCommands = "pwsh bin/Debug/net8.0/playwright.ps1 install"; }


    )
}

# Export the configuration
return $Config

