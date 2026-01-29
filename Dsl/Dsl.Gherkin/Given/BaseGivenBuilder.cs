using Dsl.Gherkin.When;
using Optivem.EShop.SystemTest.Core;
using Optivem.Testing;

namespace Dsl.Gherkin.Given;

public abstract class BaseGivenBuilder
{
    private readonly GivenClause _givenClause;

    protected BaseGivenBuilder(GivenClause givenClause) 
    {
        _givenClause = givenClause;
    }

    public GivenClause And()
    {
        return _givenClause;
    }

    public WhenClause When()
    {
        return _givenClause.When();
    }

    internal abstract Task Execute(SystemDsl app);

    protected Channel Channel => _givenClause.Channel;
}