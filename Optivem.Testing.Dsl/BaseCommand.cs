using Optivem.Testing.Dsl;

namespace Optivem.Testing.Dsl;

public abstract class BaseCommand<TDriver, TResponse, TVerification> : ICommand<CommandResult<TResponse, TVerification>>
{
    protected readonly TDriver _driver;
    protected readonly Context _context;

    protected BaseCommand(TDriver driver, Context context)
    {
        _driver = driver;
        _context = context;
    }

    public abstract CommandResult<TResponse, TVerification> Execute();
}
