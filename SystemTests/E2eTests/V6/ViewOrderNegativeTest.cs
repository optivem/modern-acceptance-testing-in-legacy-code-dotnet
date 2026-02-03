using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.E2eTests.V6.Base;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests.V6;

public class ViewOrderNegativeTest : BaseE2eTest
{
    public static IEnumerable<object[]> NonExistentOrderValues()
    {
        yield return new object[] { "NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist." };
        yield return new object[] { "NON-EXISTENT-ORDER-88888", "Order NON-EXISTENT-ORDER-88888 does not exist." };
        yield return new object[] { "NON-EXISTENT-ORDER-77777", "Order NON-EXISTENT-ORDER-77777 does not exist." };
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelMemberData(nameof(NonExistentOrderValues))]
    public async Task ShouldNotBeAbleToViewNonExistentOrder(Channel channel, string orderNumber, string expectedErrorMessage)
    {
        var then = Scenario(channel)
            .When().ViewOrder().WithOrderNumber(orderNumber)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage(expectedErrorMessage);
    }
}
