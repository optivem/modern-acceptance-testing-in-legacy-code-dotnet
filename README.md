# Modern Acceptance Testing in Legacy Code (.NET)

[![commit-stage-monolith](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/commit-stage-monolith.yml/badge.svg)](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-dotnet/actions/workflows/commit-stage-monolith.yml)
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

## Project Structure

```
modern-acceptance-testing-in-legacy-code-dotnet/
├── .github/                           # GitHub Actions workflows
│   ├── workflows/                     # CI/CD pipeline definitions
│   │   ├── commit-stage-monolith.yml  # Build & publish on every commit
│   │   ├── acceptance-stage.yml       # Automated testing every 30 min
│   │   ├── qa-stage.yml               # QA deployment workflow
│   │   ├── qa-signoff.yml             # Manual QA approval
│   │   └── prod-stage.yml             # Production deployment
│   └── actions/
│       └── deploy-docker-images/      # Reusable deployment action
│
├── monolith/                          # ASP.NET Core Web API
│   ├── Controllers/                   # API Controllers
│   ├── Core/
│   │   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Entities/                  # EF Core Entities
│   │   ├── Exceptions/                # Custom Exceptions
│   │   ├── Repositories/              # Data Access Layer
│   │   └── Services/                  # Business Logic Layer
│   ├── Api/
│   │   └── Exceptions/                # Exception Handlers
│   ├── wwwroot/                       # Static Files (HTML, CSS, JS)
│   ├── Program.cs                     # Application Entry Point
│   ├── appsettings.json               # Configuration
│   └── Dockerfile                     # Docker Build Definition
│
├── system-test/                       # xUnit + Playwright Tests
│   ├── SmokeTests/                    # Basic Health Checks
│   ├── E2eTests/                      # End-to-End Tests
│   │   ├── ApiE2eTest.cs              # API Tests (HttpClient)
│   │   └── UiE2eTest.cs               # UI Tests (Playwright)
│   ├── TestConfiguration.cs           # Test Config Reader
│   └── appsettings.json               # Test Settings
│
├── docker-compose.yml                 # Default (includes local)
├── docker-compose.local.yml           # Local development setup
├── docker-compose.pipeline.yml        # CI/CD pipeline setup
├── json-server-db.erp-api.json        # Mock ERP API data
├── json-server-db.tax-api.json        # Mock Tax API data
└── run.ps1                            # PowerShell automation script
```

## Technology Stack

### Monolith Application
- **Framework**: ASP.NET Core 8 Web API
- **Database**: PostgreSQL 16 with Entity Framework Core
- **External Services**: 
  - ERP API (JSON Server) - Product pricing
  - Tax API (JSON Server) - Tax rates by country

### System Tests
- **Test Framework**: xUnit
- **UI Testing**: Playwright (Chromium)
- **API Testing**: HttpClient
- **Test Types**:
  - Smoke Tests - Basic health checks
  - E2E Tests - Complete user workflows

### Infrastructure
- **Containerization**: Docker & Docker Compose
- **CI/CD**: GitHub Actions
- **Database Migrations**: EF Core Migrations (auto-applied on startup)

## Business Features

### Order Management
- **Place Order**: Create new orders with automatic price calculations
  - Discount Logic: 15% discount after 17:00, 0% before
  - Tax Calculation: Fetches tax rates from external Tax API by country
  - Price Calculation: Unit Price × Quantity - Discount + Tax
  - Order Number Format: `ORD-{GUID}`

- **Get Order**: Retrieve order details by order number

- **Cancel Order**: Cancel placed orders
  - Blocking Logic: Cannot cancel on Dec 31st between 22:00-23:00

### External Integrations
- **ERP Gateway**: Fetches product unit prices by SKU
- **Tax Gateway**: Fetches tax rates by country code

### Validation
- SKU must not be empty
- Quantity must be a positive integer
- Country must not be empty (2-letter code)

## API Endpoints

### Health Check
```http
GET /api/echo
```

### Order Operations
```http
# Place Order
POST /api/orders
Content-Type: application/json
{
  "sku": "HP-15",
  "quantity": 5,
  "country": "US"
}

# Get Order
GET /api/orders/{orderNumber}

# Cancel Order
POST /api/orders/{orderNumber}/cancel
```

## Test Coverage

### Smoke Tests (2 tests)
- API health check via `/api/echo`
- UI home page verification

### API E2E Tests (13 tests)
- Place order successfully
- Get order details
- Cancel order
- Validate SKU (empty/null/whitespace)
- Validate quantity (empty/null/negative/non-integer)
- Validate country (empty/null/whitespace)
- Reject non-existent SKU

### UI E2E Tests (11 tests)
- Place order and verify price calculation
- Retrieve order history
- Cancel order
- Validate SKU (empty/whitespace)
- Validate quantity (empty/negative/decimal/non-integer)
- Validate country (empty/whitespace)

**Total: 26 automated tests**

## Development

### Build Monolith
```powershell
cd monolith
dotnet build
```

### Run Monolith Locally
```powershell
cd monolith
dotnet run
```

### Run Tests
```powershell
cd system-test
dotnet test
```

### Database Migrations
Migrations are automatically applied on application startup. To create a new migration:

```powershell
cd monolith
dotnet ef migrations add MigrationName
```

## Migration from Java

This project is a .NET port of the [Java version](https://github.com/optivem/modern-acceptance-testing-in-legacy-code-java). Key changes:

| Java (Spring Boot) | .NET (ASP.NET Core) |
|-------------------|---------------------|
| Gradle | .NET CLI / MSBuild |
| JUnit 5 | xUnit |
| Lombok `@Data` | C# Properties |
| JPA/Hibernate | Entity Framework Core |
| Spring Validation | Data Annotations |
| `application.yml` | `appsettings.json` |
| `Optional<T>` | Nullable types `T?` |
| Streams API | LINQ |

All business logic, test scenarios, and workflows have been preserved.

## License

[![MIT License](https://img.shields.io/badge/license-MIT-lightgrey.svg)](https://opensource.org/licenses/MIT)

This project is released under the [MIT License](https://opensource.org/licenses/MIT).

## Contributors

- [Valentina Jemuović](https://github.com/valentinajemuovic)
- [Jelena Cupać](https://github.com/jcupac)
