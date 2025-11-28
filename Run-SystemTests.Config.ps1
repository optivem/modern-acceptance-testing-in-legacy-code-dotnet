# System Test Configuration
# This file contains configuration values for Run-SystemTests.ps1

# System Configuration
$Config = @{
    # Docker Configuration
    ContainerName = "modern-acceptance-testing-in-legacy-code-dotnet"

    # System Components (our application services)
    SystemComponents = @(
        @{ Name = "Frontend";
            Url = "http://localhost:3001";
            ContainerName = "frontend";
            BuildPath = "..\modern-acceptance-testing-in-legacy-code-frontend\frontend";
            BuildCommand = "npm run build" }
        @{ Name = "Backend API";
            Url = "http://localhost:8081/health";
            ContainerName = "backend";
            BuildPath = "..\modern-acceptance-testing-in-legacy-code-backend\backend";
            BuildCommand = "& .\gradlew.bat clean build" }
    )

    # External Systems (third-party/mock APIs)
    ExternalSystems = @(
        @{ Name = "ERP API";
            Url = "http://localhost:9001/erp/health";
            ContainerName = "external" }
        @{ Name = "Tax API";
            Url = "http://localhost:9001/tax/health";
            ContainerName = "external" }
    )

    # Test Configuration
    TestCommand = "dotnet test"
    # TestCommand = "dotnet test --filter `"FullyQualifiedName~SmokeTests`""
    TestReportPath = "system-test\TestResults\testResults.html"
}

# Export the configuration
return $Config

