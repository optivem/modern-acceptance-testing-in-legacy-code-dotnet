# Test Infrastructure Evolution (V1-V7)

This directory contains seven versions of test base classes demonstrating the evolution from raw HTTP/Playwright code to advanced Gherkin-style DSL patterns. Each version builds upon the previous one, showing progressive abstraction and improved maintainability.

## Version Progression

### V1: Raw HTTP and Playwright
**File:** `V1/BaseRawTest.cs`

- **Pattern:** Direct HTTP client and Playwright API usage
- **Setup:** Manual instantiation of HttpClient and IBrowser
- **Benefits:** Complete control, no abstractions
- **Drawbacks:** Verbose, repetitive, low-level details exposed in tests
- **Use Case:** Learning fundamentals, debugging low-level issues

```csharp
protected HttpClient? ErpHttpClient;
protected IBrowser? ShopBrowser;
```

### V2: Client Wrappers
**File:** `V2/BaseClientTest.cs`

- **Pattern:** Typed client classes wrapping HTTP/Playwright logic
- **Setup:** ErpRealClient, TaxRealClient
- **Benefits:** Encapsulates HTTP details, reusable clients
- **Drawbacks:** Still tightly coupled to client implementations
- **Use Case:** Organizing HTTP code, basic abstraction

```csharp
protected ErpRealClient? ErpClient;
protected TaxRealClient? TaxClient;
```

### V3: Driver Pattern
**File:** `V3/BaseDriverTest.cs`

- **Pattern:** Driver interfaces abstracting business operations
- **Setup:** ErpRealDriver, TaxRealDriver, IShopDriver
- **Benefits:** Business-focused methods, testable via interfaces
- **Drawbacks:** No channel selection, requires manual driver choice
- **Use Case:** Business-level test automation, interface-based testing

```csharp
protected IShopDriver? ShopDriver;
protected void SetUpShopApiDriver();
```

### V4: Channel-Aware Drivers
**File:** `V4/BaseChannelDriverTest.cs`

- **Pattern:** Driver selection based on channel context (UI vs API)
- **Setup:** Automatic driver instantiation via CreateShopDriverAsync()
- **Benefits:** Tests run on both UI and API channels without code changes
- **Drawbacks:** Still requires driver management in tests
- **Use Case:** Multi-channel testing (same tests on UI and API)

```csharp
private async Task<IShopDriver?> CreateShopDriverAsync();
```

### V5: SystemDsl Facade
**File:** `V5/BaseSystemDslTest.cs`

- **Pattern:** Unified system facade exposing all drivers
- **Setup:** Single `SystemDsl` property providing access to Shop, Erp, Tax
- **Benefits:** Single entry point, simplified test code
- **Drawbacks:** No Gherkin-style Given/When/Then structure
- **Use Case:** System-level integration tests with clean API

```csharp
protected SystemDsl App { get; private set; }
```

### V6: ScenarioDsl (Basic)
**File:** `V6/BaseScenarioDslTest.cs`

- **Pattern:** Gherkin-style Given/When/Then DSL
- **Setup:** ScenarioDsl with Given(), When(), Then() methods
- **Benefits:** Readable BDD-style tests, expressive test structure
- **Drawbacks:** Simplified version without full lifecycle management
- **Use Case:** BDD-style tests with business-readable syntax

```csharp
protected ScenarioDsl Scenario { get; private set; }
// Test: Scenario.Given.ShopOrder(order).When.Submit().Then.Success();
```

### V7: ScenarioDsl (Complete)
**File:** `V7/BaseScenarioDslTest.cs` *(Recommended)*

- **Pattern:** Full Gherkin DSL with complete lifecycle management
- **Setup:** ScenarioDsl with browser lifecycle, channel context, and cleanup
- **Benefits:** Production-ready, handles all edge cases, most maintainable
- **Drawbacks:** Most complex implementation (but hidden from tests)
- **Use Case:** **Recommended for all new acceptance tests**

```csharp
protected ScenarioDsl Scenario { get; private set; }
// Complete lifecycle: browser initialization, proper disposal, error handling
```

## Recommendation

**For new tests, use V7 (BaseScenarioDslTest)** - it provides the most maintainable and readable test structure while handling all infrastructure concerns automatically.

## Usage Examples

### V1: Raw
```csharp
public class MyTest : BaseRawTest
{
    [Fact]
    public async Task Test()
    {
        SetUpExternalHttpClients();
        var response = await ErpHttpClient.GetAsync("/api/products");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
```

### V3: Driver
```csharp
public class MyTest : BaseDriverTest
{
    [Fact]
    public async Task Test()
    {
        SetUpShopApiDriver();
        await ShopDriver.CreateOrderAsync(order);
        var result = await ShopDriver.GetOrderAsync(orderId);
        Assert.NotNull(result);
    }
}
```

### V7: ScenarioDsl (Recommended)
```csharp
public class MyTest : BaseScenarioDslTest
{
    [Fact]
    public async Task Test()
    {
        await Scenario
            .Given.ShopOrder(order)
            .When.Submit()
            .Then.Success();
    }
}
```

## Architecture Principles

Each version demonstrates key principles:
- **V1-V2:** Show the progression from raw APIs to organized clients
- **V3-V4:** Demonstrate the Driver pattern and channel abstraction
- **V5-V7:** Illustrate facade patterns and DSL design

## Environment Configuration

All versions inherit from `BaseConfigurableTest`, supporting:
- Environment-specific configs: `appsettings.{env}.{mode}.json`
- Environments: Local, Acceptance, QA, Production
- Modes: STUB, REAL
- Dynamic loading via `OPTIVEM_ENVIRONMENT` and `OPTIVEM_EXTERNAL_SYSTEM_MODE` env variables

## Migration Path

Legacy tests should migrate following this path:
1. V1 → V2: Extract clients from raw HTTP code
2. V2 → V3: Create driver interfaces from clients
3. V3 → V4: Add channel context for multi-channel support
4. V4 → V5: Consolidate into SystemDsl facade
5. V5 → V7: Adopt Gherkin-style ScenarioDsl for readability

**Target: All tests should eventually use V7 for maximum maintainability.**
