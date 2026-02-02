using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Tax.Client;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V2;

/// <summary>
/// V2: Client-based test infrastructure.
/// Introduces typed clients that wrap HTTP and Playwright logic.
/// NOTE: Simplified version for educational purposes - shows the pattern evolution.
/// </summary>
public abstract class BaseClientTest : BaseConfigurableTest, IAsyncLifetime
{
    protected ErpRealClient? ErpClient { get; private set; }
    protected TaxRealClient? TaxClient { get; private set; }
    protected SystemConfiguration? Configuration { get; private set; }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    protected void SetUpExternalClients()
    {
        Configuration = LoadConfiguration();
        ErpClient = new ErpRealClient(Configuration.ErpBaseUrl);
        TaxClient = new TaxRealClient(Configuration.TaxBaseUrl);
    }

    public virtual async Task DisposeAsync()
    {
        ErpClient?.Dispose();
        TaxClient?.Dispose();
        await Task.CompletedTask;
    }
}
