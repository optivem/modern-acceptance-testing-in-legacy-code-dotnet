using Optivem.EShop.SystemTest.Core.Erp.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Dsl;
using Optivem.EShop.SystemTest.Core.Tax.Dsl;
using Optivem.Testing.Channels;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core;

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
