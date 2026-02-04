using Commons.Dsl;
using Optivem.EShop.SystemTest.Base.V5;
using Optivem.EShop.SystemTest.Core;

namespace Optivem.EShop.SystemTest.ContractTests.Base;

public abstract class BaseExternalSystemContractTest : BaseSystemDslTest
{
    protected SystemDsl App => _app;

    protected abstract ExternalSystemMode? FixedExternalSystemMode { get; }

    protected sealed override ExternalSystemMode? GetFixedExternalSystemMode() => FixedExternalSystemMode;
}
