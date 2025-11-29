# Modern Acceptance Testing in Legacy Code (.NET)

[![acceptance-stage](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/acceptance-stage.yml/badge.svg)](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/acceptance-stage.yml)
[![qa-stage](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/qa-stage.yml/badge.svg)](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/qa-stage.yml)
[![qa-signoff](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/qa-signoff.yml/badge.svg)](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/qa-signoff.yml)
[![prod-stage](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/prod-stage.yml/badge.svg)](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/prod-stage.yml)

## Prerequisites

- .NET 8 SDK
- Docker Desktop
- PowerShell 7+

Ensure you have .NET 8 SDK installed

```shell
dotnet --version
```

Check that you have PowerShell 7

```shell
$PSVersionTable.PSVersion
```

Ensure you have Playwright browser installed:

```shell
cd system-test
pwsh bin/Debug/net8.0/playwright.ps1 install
```

## Run Everything

```powershell
.\run.ps1 all
```

This will:
1. Build the Monolith (compile code and create Docker image)
2. Start Docker containers (Monolith, PostgreSQL, & Simulated External Systems)
3. Wait for services to be healthy
4. Run all System Tests (xUnit + Playwright)

You can open these URLs in your browser:
- Monolith Application: [http://localhost:8081](http://localhost:8081)
- ERP API (JSON Server): [http://localhost:3100](http://localhost:3100)
- Tax API (JSON Server): [http://localhost:3101](http://localhost:3101)
- PostgreSQL: localhost:5433 (username: `eshop_user`, password: `eshop_password`)

## Separate Commands

### Build
Compiles the code and creates the Docker image (local mode only):
```powershell
.\run.ps1 build
```

### Start Services
Starts the Docker containers:
```powershell
# Local mode (builds from source)
.\run.ps1 start

# Pipeline mode (uses pre-built image)
.\run.ps1 start pipeline
```

You can open these URLs in your browser:
- Monolith Application: [http://localhost:8081](http://localhost:8081)
- ERP API (JSON Server): [http://localhost:3100](http://localhost:3100)
- Tax API (JSON Server): [http://localhost:3101](http://localhost:3101)
- PostgreSQL: localhost:5433 (username: `eshop_user`, password: `eshop_password`)

### Run Tests
```powershell
.\run.ps1 test
```

### View Logs
```powershell
.\run.ps1 logs
```

### Stop Services
```powershell
.\run.ps1 stop
```

## License

[![MIT License](https://img.shields.io/badge/license-MIT-lightgrey.svg)](https://opensource.org/licenses/MIT)

This project is released under the [MIT License](https://opensource.org/licenses/MIT).

## Contributors

- [Valentina Jemuović](https://github.com/valentinajemuovic)
- [Jelena Cupać](https://github.com/jcupac)
