using Commons.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public class CouponManagementPage : BasePage
{
    private const string CouponCodeInputSelector = "[aria-label=\"Coupon Code\"]";
    private const string DiscountRateInputSelector = "[aria-label=\"Discount Rate\"]";
    private const string ValidFromInputSelector = "[aria-label=\"Valid From\"]";
    private const string ValidToInputSelector = "[aria-label=\"Valid To\"]";
    private const string UsageLimitInputSelector = "[aria-label=\"Usage Limit\"]";
    private const string PublishCouponButtonSelector = "[aria-label=\"Create Coupon\"]";
    
    // Selectors for browsing coupons
    private const string CouponsTableSelector = "[aria-label=\"Coupons Table\"]";
    
    // Time format constants
    private const string TimeMidnight = "T00:00";
    private const string TimeEndOfDay = "T23:59";
    
    // Table selectors
    private const string TableRowSelector = "table.table tbody tr";
    private const string TableCellCodeSelector = "table.table tbody tr td:nth-child(1)";
    private const string TableCellDiscountSelector = "table.table tbody tr td:nth-child(2)";
    private const string TableCellValidFromSelector = "table.table tbody tr td:nth-child(3)";
    private const string TableCellValidToSelector = "table.table tbody tr td:nth-child(4)";
    private const string TableCellUsageLimitSelector = "table.table tbody tr td:nth-child(5)";
    private const string TableCellUsedCountSelector = "table.table tbody tr td:nth-child(6)";
    
    // Display text constants
    private const string PercentSymbol = "%";
    private const string TextImmediate = "Immediate";
    private const string TextNever = "Never";
    private const string TextUnlimited = "Unlimited";

    public CouponManagementPage(PageClient pageClient) : base(pageClient)
    {
    }

    public async Task InputCouponCodeAsync(string? couponCode)
    {
        await PageClient.FillAsync(CouponCodeInputSelector, couponCode);
    }

    public async Task InputDiscountRateAsync(string? discountRate)
    {
        await PageClient.FillAsync(DiscountRateInputSelector, discountRate);
    }

    public async Task InputValidFromAsync(string? validFrom)
    {
        var datetimeValue = GetValidFromDateTimeString(validFrom);
        await PageClient.FillAsync(ValidFromInputSelector, datetimeValue);
    }

    public async Task InputValidToAsync(string? validTo)
    {
        var datetimeValue = GetValidToDateTimeString(validTo);
        await PageClient.FillAsync(ValidToInputSelector, datetimeValue);
    }

    public async Task InputUsageLimitAsync(string? usageLimit)
    {
        await PageClient.FillAsync(UsageLimitInputSelector, usageLimit);
    }

    public async Task ClickPublishCouponAsync()
    {
        await PageClient.ClickAsync(PublishCouponButtonSelector);
    }

    public async Task<bool> HasCouponsTableAsync()
    {
        return await PageClient.IsVisibleAsync(CouponsTableSelector);
    }

    public async Task<List<CouponDto>> ReadCouponsAsync()
    {
        if (!await HasCouponsTableAsync())
        {
            return new List<CouponDto>();
        }
        
        var coupons = new List<CouponDto>();

        // Use readAllTextContents to avoid strict mode violations
        // These selectors intentionally match multiple elements (one per table row)
        var codes = await PageClient.ReadAllTextContentsAsync(TableCellCodeSelector);
        
        // If no codes found, table is empty
        if (!codes.Any())
        {
            return new List<CouponDto>();
        }
        
        var discountRates = await PageClient.ReadAllTextContentsAsync(TableCellDiscountSelector);
        var validFroms = await PageClient.ReadAllTextContentsAsync(TableCellValidFromSelector);
        var validTos = await PageClient.ReadAllTextContentsAsync(TableCellValidToSelector);
        var usageLimits = await PageClient.ReadAllTextContentsAsync(TableCellUsageLimitSelector);
        var usedCounts = await PageClient.ReadAllTextContentsAsync(TableCellUsedCountSelector);

        var rowCount = codes.Count;
        
        // Double-check we have data before trying to access it
        // Also verify all columns have the same row count (handles empty tables or malformed data)
        if (rowCount == 0 || discountRates.Count != rowCount || validFroms.Count != rowCount 
                || validTos.Count != rowCount || usageLimits.Count != rowCount || usedCounts.Count != rowCount) {
            return new List<CouponDto>();
        }

        // Build coupon objects from the collected data
        for (int i = 0; i < rowCount; i++) {
            var code = codes[i].Trim();
            var discountRateText = discountRates[i].Trim().Replace(PercentSymbol, "");
            var validFromText = validFroms[i].Trim();
            var validToText = validTos[i].Trim();
            var usageLimitText = usageLimits[i].Trim();
            var usedCountText = usedCounts[i].Trim();

            var coupon = new CouponDto
            {
                Code = code,
                DiscountRate = ParseDiscountRate(discountRateText),
                ValidFrom = ParseInstant(validFromText),
                ValidTo = ParseInstant(validToText),
                UsageLimit = ParseUsageLimit(usageLimitText),
                UsedCount = int.Parse(usedCountText)
            };

            coupons.Add(coupon);
        }

        return coupons;
    }

    private static string GetValidFromDateTimeString(string? validFrom)
    {
        if (string.IsNullOrEmpty(validFrom))
        {
            return "";
        }

        // Extract date from ISO 8601 format (2024-08-31T00:00:00Z -> 2024-08-31)
        // Then convert to datetime-local format (2024-08-31T00:00) for HTML input
        string dateOnly = validFrom.Substring(0, 10); // YYYY-MM-DD
        return dateOnly + TimeMidnight;
    }

    private static string GetValidToDateTimeString(string? validTo)
    {
        if (string.IsNullOrEmpty(validTo))
        {
            return "";
        }

        // Extract date from ISO 8601 format (2024-08-31T23:59:59Z -> 2024-08-31)
        // Then convert to datetime-local format (2024-08-31T23:59) for HTML input
        string dateOnly = validTo.Substring(0, 10); // YYYY-MM-DD
        return dateOnly + TimeEndOfDay;
    }

    private static decimal ParseDiscountRate(string? text)
    {
        if(text == null || string.IsNullOrEmpty(text))
        {
            return 0.0m;
        }

        try
        {
            return decimal.Parse(text) / 100.0m; // Convert percentage to decimal
        }
        catch (FormatException)
        {
            return 0.0m;
        }
    }

    private static DateTime? ParseInstant(string text)
    {
        if (text == null || text.Equals(TextImmediate, StringComparison.OrdinalIgnoreCase) || 
            text.Equals(TextNever, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(text))
        {
            return null;
        }
        try
        {
            // Try to parse as ISO format first
            return DateTime.Parse(text);
        }
        catch (Exception)
        {
            // If it fails, return null (UI might show formatted date)
            return null;
        }
    }

    private static int? ParseUsageLimit(string? text)
    {
        if (text == null || text.Equals(TextUnlimited, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(text))
        {
            return null;
        }
        try
        {
            return int.Parse(text);
        }
        catch (FormatException)
        {
            return null;
        }
    }
}