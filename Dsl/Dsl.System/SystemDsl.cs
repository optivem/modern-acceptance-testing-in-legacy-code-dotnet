using Optivem.EShop.SystemTest.Core.Clock.Dsl;
using Optivem.EShop.SystemTest.Core.Common;
using Optivem.EShop.SystemTest.Core.Erp.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Dsl;
using Optivem.EShop.SystemTest.Core.Tax.Dsl;
using Optivem.Testing.Channels;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core;

public class SystemDsl : IDisposable
{
    private readonly UseCaseContext _context;
    private readonly SystemConfiguration _configuration;
    private readonly Dictionary<string, ShopDsl> _shopDsls;
    private ErpDsl? _erp;
    private TaxDsl? _tax;
    private ClockDsl? _clock;

    public SystemDsl(UseCaseContext context, SystemConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _shopDsls = new Dictionary<string, ShopDsl>();
    }

    public SystemDsl(SystemConfiguration configuration)
        : this(new UseCaseContext(), configuration) { }

    public ShopDsl Shop(Channel channel)
    {
        if (!_shopDsls.TryGetValue(channel.Type, out var shop))
        {
            shop = new ShopDsl(channel, _context, _configuration);
            _shopDsls[channel.Type] = shop;
        }
        return shop;
    }

    public ErpDsl Erp => _erp ??= new ErpDsl(_configuration.ErpBaseUrl, _context);

    public TaxDsl Tax => _tax ??= new TaxDsl(_configuration.TaxBaseUrl, _context);

    public ClockDsl Clock => _clock ??= new ClockDsl(_context, _configuration);

    public void Dispose()
    {
        foreach (var shop in _shopDsls.Values)
        {
            shop?.Dispose();
        }
        _erp?.Dispose();
        _tax?.Dispose();
        _clock?.Dispose();

        ChannelContext.Clear();
    }
}
