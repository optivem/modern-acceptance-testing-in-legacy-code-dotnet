using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V5;

public abstract class BaseSystemDslTest : BaseConfigurableTest, IAsyncLifetime
{
    protected SystemDsl App { get; private set; } = null!;

    public virtual async Task InitializeAsync()
    {
        var configuration = LoadConfiguration();
        App = new SystemDsl(configuration);
        await Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        if (App != null)
            await App.DisposeAsync();
    }
}
