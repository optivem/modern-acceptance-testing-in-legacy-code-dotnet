using Dsl.Gherkin.Then;
using Optivem.Commons.Dsl;
using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;

namespace Dsl.Gherkin.Then
{
    public class ThenFailureBuilder<TSuccessResponse, TSuccessVerification> 
        : BaseThenBuilder<TSuccessResponse, TSuccessVerification>
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly ErrorFailureVerification _failureVerification;

        public ThenFailureBuilder(ThenClause<TSuccessResponse, TSuccessVerification> thenClause, ShopUseCaseResult<TSuccessResponse, TSuccessVerification> result) : base(thenClause)
        {
            if (result == null)
            {
                throw new InvalidOperationException("Cannot verify failure: no operation was executed");
            }
            
            _failureVerification = result.ShouldFail();
        }

        public ThenFailureBuilder<TSuccessResponse, TSuccessVerification> ErrorMessage(string expectedMessage)
        {
            _failureVerification.ErrorMessage(expectedMessage);
            return this;
        }

        public ThenFailureBuilder<TSuccessResponse, TSuccessVerification> FieldErrorMessage(string expectedField, string expectedMessage)
        {
            _failureVerification.FieldErrorMessage(expectedField, expectedMessage);
            return this;
        }
    }
}