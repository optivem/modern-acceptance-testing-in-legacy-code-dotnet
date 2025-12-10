using Optivem.EShop.SystemTest.Core.Dsl.Erp;
using Optivem.EShop.SystemTest.Core.Dsl.Shop;
using Optivem.EShop.SystemTest.Core.Dsl.Tax;
using Optivem.Testing.Channels;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Dsl;

public class DslFactory
{
    private readonly Context _context;

    private DslFactory(Context context)
    {
        _context = context;
    }

    public DslFactory() : this(new Context())
    {
    }

    public ShopDsl CreateShopDsl(Channel channel)
    {
        return new ShopDsl(channel, _context);
    }

    public ErpDsl CreateErpDsl()
    {
        return new ErpDsl(_context);
    }

    public TaxDsl CreateTaxDsl()
    {
        return new TaxDsl(_context);
    }
}
