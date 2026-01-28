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
    private ShopDsl? _shop;
    private ErpDsl? _erp;
    private TaxDsl? _tax;
    private ClockDsl? _clock;

    public SystemDsl(UseCaseContext context, SystemConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public SystemDsl(SystemConfiguration configuration)
        : this(new UseCaseContext(configuration.ExternalSystemMode), configuration) { }

    public ShopDsl Shop(Channel channel)
    {
        return GetOrCreate(ref _shop, () => new ShopDsl(
            _configuration.ShopUiBaseUrl,
            _configuration.ShopApiBaseUrl,
            channel,
            _context));
    }

    public ErpDsl Erp() => GetOrCreate(ref _erp, () => new ErpDsl(_configuration.ErpBaseUrl, _context));

    public TaxDsl Tax() => GetOrCreate(ref _tax, () => new TaxDsl(_configuration.TaxBaseUrl, _context));

    public ClockDsl Clock() => GetOrCreate(ref _clock, () => new ClockDsl(_configuration.ClockBaseUrl, _context));

    public void Dispose()
    {
        _shop?.Dispose();
        _erp?.Dispose();
        _tax?.Dispose();
        _clock?.Dispose();

        // TODO: VJ: Perhaps delete this?
        ChannelContext.Clear();
    }

    private static T GetOrCreate<T>(ref T? instance, Func<T> supplier) where T : class
    {
        return instance ??= supplier();
    }
}
