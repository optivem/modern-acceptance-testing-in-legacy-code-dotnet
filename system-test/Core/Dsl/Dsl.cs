using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Erp;
using Optivem.EShop.SystemTest.Core.Dsl.Shop;
using Optivem.EShop.SystemTest.Core.Dsl.Tax;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Dsl;

public class Dsl : IDisposable
{
    private readonly Context _context;
    private readonly Dictionary<string, ShopDsl> _shopDsls;
    private ErpDsl? _erp;
    private TaxDsl? _tax;

    public Dsl()
    {
        _context = new Context();
        _shopDsls = new Dictionary<string, ShopDsl>();
    }

    public ShopDsl Shop(Channel channel)
    {
        if (!_shopDsls.TryGetValue(channel.Type, out var shop))
        {
            shop = new ShopDsl(channel, _context);
            _shopDsls[channel.Type] = shop;
        }
        return shop;
    }

    public ErpDsl Erp => _erp ??= new ErpDsl(_context);

    public TaxDsl Tax => _tax ??= new TaxDsl(_context);

    public void Dispose()
    {
        foreach (var shop in _shopDsls.Values)
        {
            shop?.Dispose();
        }
        _erp?.Dispose();
        _tax?.Dispose();
        ChannelContext.Clear();
    }
}
