using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands.Base;

public abstract class BaseCommand<TDriver, TResponse, TVerification> : ICommand<CommandResult<TResponse, TVerification>>
{
    protected readonly TDriver Driver;
    protected readonly TestContext Context;

    protected BaseCommand(TDriver driver, TestContext context)
    {
        Driver = driver;
        Context = context;
    }

    public abstract CommandResult<TResponse, TVerification> Execute();
}
