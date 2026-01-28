using Optivem.Commons.Dsl;
using Optivem.Testing;

namespace Dsl.Gherkin.Then;

public abstract class BaseThenBuilder<TSuccessResponse, TSuccessVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse>
{
    private readonly ThenClause<TSuccessResponse, TSuccessVerification> _thenClause;

    protected BaseThenBuilder(ThenClause<TSuccessResponse, TSuccessVerification> thenClause)
    {
        _thenClause = thenClause;
    }

    public ThenClause<TSuccessResponse, TSuccessVerification> And()
    {
        return _thenClause;
    }

    protected Channel Channel => _thenClause.Channel;
}