using Optivem.EShop.SystemTest.Core.Dsl.Commons;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands.Base;

public abstract class BaseCommand<TDriver, TResponse, TVerification> : ICommand<CommandResult<TResponse, TVerification>>
{
    protected readonly TDriver Driver;
    protected readonly Context Context;

    protected BaseCommand(TDriver driver, Context context)
    {
        Driver = driver;
        Context = context;
    }

    public abstract CommandResult<TResponse, TVerification> Execute();
}
