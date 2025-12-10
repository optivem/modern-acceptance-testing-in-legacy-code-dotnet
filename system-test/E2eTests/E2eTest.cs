using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Shop;
using Optivem.EShop.SystemTest.Core.Dsl.Erp;
using Optivem.EShop.SystemTest.Core.Dsl.Tax;
using System;
using System.Collections.Generic;
using Channel = Optivem.Testing.Channels.Channel;

namespace Optivem.EShop.SystemTest.E2eTests
{
    public class E2eTest : IDisposable
    {
        private Context _context;
        private ShopDsl? _shop;
        private ErpDsl? _erp;
        private TaxDsl? _tax;

        public E2eTest()
        {
            _context = new Context();
            _erp = new ErpDsl(_context);
            _tax = new TaxDsl(_context);
        }

        public void Dispose()
        {
            _shop?.Dispose();
            _erp?.Dispose();
            _tax?.Dispose();
            ChannelContext.Clear();
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldPlaceOrderAndCalculateOriginalPrice(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            const string SKU = "SKU";
            const string QUANTITY = "5";
            const string COUNTRY = "US";
            const string ORDER_NUMBER = "ORDER_NUMBER";

            _erp.CreateProduct()
                .Sku(SKU)
                .UnitPrice("20.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(SKU)
                .Quantity(QUANTITY)
                .Country(COUNTRY)
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER);

            _shop.ViewOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Quantity(5)
                .Country(COUNTRY)
                .Status(OrderStatus.PLACED)
                .SubtotalPriceGreaterThanZero()
                .TaxRateGreaterThanOrEqualToZero()
                .TaxAmountGreaterThanOrEqualToZero()
                .TotalPriceGreaterThanZero();
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldCancelOrder(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            const string SKU = "SKU";
            const string QUANTITY = "2";
            const string COUNTRY = "US";
            const string ORDER_NUMBER = "ORDER_NUMBER";

            _erp.CreateProduct()
                .Sku(SKU)
                .UnitPrice("50.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(SKU)
                .Quantity(QUANTITY)
                .Country(COUNTRY)
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER);

            _shop.CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            _shop.ViewOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Quantity(2)
                .Country(COUNTRY)
                .Status(OrderStatus.CANCELLED);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNonExistentSku(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku("NON-EXISTENT-SKU-12345")
                .Quantity("5")
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Product does not exist for SKU: NON-EXISTENT-SKU-12345");
        }

        public static IEnumerable<object[]> ShouldNotBeAbleToViewNonExistentOrderData()
        {
            return GenerateChannelTestDataEnumerable(
                [ChannelType.UI, ChannelType.API],
                [
                    ("NON-EXISTENT-ORDER-12345", "Order NON-EXISTENT-ORDER-12345 does not exist."),
                    ("INVALID-ORD-99999", "Order INVALID-ORD-99999 does not exist."),
                    ("MISSING-ORDER-00000", "Order MISSING-ORDER-00000 does not exist.")
                ]
            );
        }

        [Theory]
        [MemberData(nameof(ShouldNotBeAbleToViewNonExistentOrderData))]
        public void ShouldNotBeAbleToViewNonExistentOrder(Channel channel, string orderNumber, string expectedMessage)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.ViewOrder()
                .OrderNumber(orderNumber)
                .Execute()
                .ShouldFail()
                .ErrorMessage(expectedMessage);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNegativeQuantity(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            var sku = "DEF-" + Guid.NewGuid();
            _erp!.CreateProduct()
                .Sku(sku)
                .UnitPrice("30.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(sku)
                .Quantity("-3")
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithZeroQuantity(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            var sku = "GHI-" + Guid.NewGuid();
            _erp!.CreateProduct()
                .Sku(sku)
                .UnitPrice("40.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(sku)
                .Quantity("0")
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("")]
        [ChannelInlineData("   ")]
        public void ShouldRejectOrderWithEmptySku(Channel channel, string sku)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku(sku)
                .Quantity("5")
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("")]
        [ChannelInlineData("   ")]
        public void ShouldRejectOrderWithEmptyQuantity(Channel channel, string emptyQuantity)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku("some-sku")
                .Quantity(emptyQuantity)
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("3.5")]
        [ChannelInlineData("lala")]
        public void ShouldRejectOrderWithNonIntegerQuantity(Channel channel, string nonIntegerQuantity)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku("some-sku")
                .Quantity(nonIntegerQuantity)
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must be an integer");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("")]
        [ChannelInlineData("   ")]
        public void ShouldRejectOrderWithEmptyCountry(Channel channel, string emptyCountry)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku("some-sku")
                .Quantity("5")
                .Country(emptyCountry)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithUnsupportedCountry(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            var sku = "JKL-" + Guid.NewGuid();
            _erp!.CreateProduct()
                .Sku(sku)
                .UnitPrice("25.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(sku)
                .Quantity("3")
                .Country("XX")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Country does not exist: XX");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullQuantity(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku("some-sku")
                .Quantity(null!)
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullSku(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku(null!)
                .Quantity("5")
                .Country("US")
                .Execute()
                .ShouldFail()
                .ErrorMessage("SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullCountry(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.PlaceOrder()
                .Sku("some-sku")
                .Quantity("5")
                .Country(null!)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        [ChannelInlineData("NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist.")]
        [ChannelInlineData("INVALID-ORDER-12345", "Order INVALID-ORDER-12345 does not exist.")]
        [ChannelInlineData("FAKE-ORDER-00000", "Order FAKE-ORDER-00000 does not exist.")]
        public void ShouldNotCancelNonExistentOrder(Channel channel, string orderNumber, string expectedMessage)
        {
            _shop = new ShopDsl(channel, _context);

            _shop.CancelOrder()
                .OrderNumber(orderNumber)
                .Execute()
                .ShouldFail()
                .ErrorMessage(expectedMessage);
        }

        // IEnumerable version - same functionality but returns IEnumerable<object[]>
        private static IEnumerable<object[]> GenerateChannelTestDataEnumerable(
            string[] channels,
            (string value, string message)[] testCases)
        {
            foreach (var channelType in channels)
            {
                foreach (var (value, message) in testCases)
                {
                    yield return new object[] { new Channel(channelType), value, message };
                }
            }
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldNotCancelAlreadyCancelledOrder(Channel channel)
        {
            _shop = new ShopDsl(channel, _context);

            const string SKU = "SKU";
            const string ORDER_NUMBER = "ORDER_NUMBER";

            _erp!.CreateProduct()
                .Sku(SKU)
                .UnitPrice("35.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(SKU)
                .Quantity("3")
                .Country("US")
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            // Cancel the order first time - should succeed
            _shop.CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            // Try to cancel the same order again - should fail
            _shop.CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Order has already been cancelled");
        }
    }
}
