# System Test Project

This project contains comprehensive system tests for the Optivem eShop application using xUnit and Playwright.

## Prerequisites

- .NET 8.0 SDK
- Playwright browsers (will be installed automatically)

## Project Structure

```
system-test/
├── E2eTests/
│   ├── ApiE2eTest.cs      # End-to-end API tests using HttpClient
│   └── UiE2eTest.cs       # End-to-end UI tests using Playwright
├── SmokeTests/
│   ├── ApiSmokeTest.cs    # Basic API health check
│   └── UiSmokeTest.cs     # Basic UI health check
├── TestConfiguration.cs    # Configuration reader for test settings
├── appsettings.json       # Test configuration file
└── Optivem.AtddAccelerator.EShop.SystemTest.csproj
```

## Configuration

Test configuration is managed through `appsettings.json`:

```json
{
  "BaseUrl": "http://localhost:8080",
  "WaitSeconds": 10
}
```

- **BaseUrl**: The base URL of the application under test
- **WaitSeconds**: Default timeout for operations

## Setting Up Playwright

Before running UI tests for the first time, install Playwright browsers:

```powershell
pwsh bin/Debug/net8.0/playwright.ps1 install
```

Or build the project and run:

```powershell
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install chromium
```

## Running Tests

### Run All Tests

```powershell
dotnet test
```

### Run Specific Test Categories

Run only smoke tests:
```powershell
dotnet test --filter "FullyQualifiedName~SmokeTests"
```

Run only E2E tests:
```powershell
dotnet test --filter "FullyQualifiedName~E2eTests"
```

Run only API tests:
```powershell
dotnet test --filter "FullyQualifiedName~ApiE2eTest"
```

Run only UI tests:
```powershell
dotnet test --filter "FullyQualifiedName~UiE2eTest"
```

### Run Specific Test

```powershell
dotnet test --filter "PlaceOrder_WithValidRequest_ShouldReturnCreated"
```

## Test Coverage

### Smoke Tests

**ApiSmokeTest**:
- Basic API health check via `/api/echo` endpoint

**UiSmokeTest**:
- Basic UI health check by loading the home page

### API E2E Tests (ApiE2eTest)

**Happy Path Tests**:
- `PlaceOrder_WithValidRequest_ShouldReturnCreated` - Place a valid order
- `GetOrder_WithExistingOrder_ShouldReturnOrder` - Retrieve an existing order
- `CancelOrder_WithExistingPlacedOrder_ShouldReturnNoContent` - Cancel a placed order

**Error Cases**:
- `GetOrder_WithNonExistentOrder_ShouldReturnNotFound` - Get non-existent order returns 404
- `CancelOrder_WithNonExistentOrder_ShouldReturnNotFound` - Cancel non-existent order returns 404
- `CancelOrder_WithAlreadyCancelledOrder_ShouldReturnBadRequest` - Cancel already cancelled order returns 400

**Validation Tests** (Parameterized):
- `PlaceOrder_WithInvalidRequest_ShouldReturnBadRequest` - Tests empty SKU, zero/negative quantity, empty country
- `PlaceOrder_WithInvalidQuantityType_ShouldReturnBadRequest` - Tests null quantity
- `PlaceOrder_WithMissingFields_ShouldReturnBadRequest` - Tests missing required fields

### UI E2E Tests (UiE2eTest)

**Happy Path Tests**:
- `PlaceOrder_WithValidInputs_ShouldDisplaySuccessMessage` - Place order through UI
- `GetOrder_WithExistingOrder_ShouldDisplayOrderDetails` - View order details through UI
- `CancelOrder_WithExistingPlacedOrder_ShouldDisplayCancelledStatus` - Cancel order through UI

**Error Cases**:
- `GetOrder_WithNonExistentOrder_ShouldDisplayErrorMessage` - Search for non-existent order

**Validation Tests** (Parameterized):
- `PlaceOrder_WithInvalidInputs_ShouldDisplayValidationError` - Tests empty SKU, zero/negative quantity, empty country
- `PlaceOrder_WithInvalidQuantityType_ShouldDisplayValidationError` - Tests non-numeric quantity inputs

**Navigation Tests**:
- `NavigateToShop_FromHomePage_ShouldLoadShopPage` - Navigate to shop page
- `NavigateToOrderHistory_FromHomePage_ShouldLoadOrderHistoryPage` - Navigate to order history page

## Test Framework Details

### xUnit Features Used

- **[Fact]**: Simple test cases
- **[Theory]** with **[InlineData]**: Parameterized tests for multiple input scenarios
- **[MemberData]**: Complex parameterized test data
- **IDisposable**: For synchronous cleanup (API tests)
- **IAsyncLifetime**: For async setup/teardown (UI tests with Playwright)

### Playwright Configuration

- Browser: Chromium (headless mode)
- Async/await pattern for all operations
- Page isolation per test
- Proper cleanup in DisposeAsync

### HttpClient Configuration

- Base address configured from appsettings.json
- JSON serialization for request/response
- Proper disposal in Dispose method

## CI/CD Integration

The tests are designed to run in CI/CD pipelines:

```yaml
# Example GitHub Actions
- name: Install Playwright
  run: pwsh system-test/bin/Debug/net8.0/playwright.ps1 install chromium

- name: Run Tests
  run: dotnet test --logger "trx;LogFileName=test-results.trx"
```

## Troubleshooting

### Playwright Installation Issues

If you encounter issues installing Playwright browsers:

```powershell
# Clean build
dotnet clean
dotnet build

# Install browsers manually
pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps chromium
```

### Connection Issues

If tests fail to connect to the application:
1. Verify the application is running on the configured BaseUrl
2. Check firewall settings
3. Verify the BaseUrl in appsettings.json matches your running application

### Test Isolation

Each test is isolated:
- API tests use HttpClient instances
- UI tests create new browser pages
- No shared state between tests

## Best Practices

1. **Async/Await**: All I/O operations use async/await
2. **Proper Cleanup**: All resources are disposed properly
3. **Test Isolation**: Tests don't depend on each other
4. **Descriptive Names**: Test names clearly describe what they test
5. **AAA Pattern**: Tests follow Arrange-Act-Assert pattern
6. **Parameterized Tests**: Use Theory for testing multiple scenarios
7. **Meaningful Assertions**: Clear assertions with helpful error messages

## Contributing

When adding new tests:
1. Follow the existing naming conventions
2. Use appropriate test categories (SmokeTests vs E2eTests)
3. Ensure tests are isolated and don't depend on execution order
4. Add parameterized tests for validation scenarios
5. Include both happy path and error cases
6. Update this README if adding new test categories
