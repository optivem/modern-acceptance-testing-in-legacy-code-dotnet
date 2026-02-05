using Commons.Http;
using Commons.Playwright;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Commons;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public abstract class BasePage
{
    private const string NotificationSelector = "[role='alert']";
    private const string NotificationSuccessSelector = "[role='alert'].notification.success";
    private const string NotificationErrorSelector = "[role='alert'].notification.error";
    private const string NotificationErrorMessageSelector = "[role='alert'].notification.error .error-message";
    private const string NotificationErrorFieldSelector = "[role='alert'].notification.error .field-error";
    private const string NoNotificationErrorMessage = "No notification appeared";
    private const string UnrecognizedNotificationErrorMessage = "Notification type is not recognized";
    private const int NotificationStabilizationDelayMilliseconds = 150;

    protected readonly PageClient PageClient;

    protected BasePage(PageClient pageClient)
    {
        PageClient = pageClient;
    }

    private record NotificationResult(bool IsSuccess, string? SuccessMessage, string? ErrorMessage, List<string>? FieldErrors);

    private async Task<NotificationResult> DetectAndReadNotificationAsync()
    {
        // First wait for ANY notification to appear
        var isNotification = await PageClient.IsVisibleAsync(NotificationSelector);

        if (!isNotification)
        {
            throw new InvalidOperationException(NoNotificationErrorMessage);
        }

        // Small wait to ensure we're checking the latest notification state,
        // not a stale one from a previous action
        await Task.Delay(NotificationStabilizationDelayMilliseconds);

        // Now check immediately what type it is (don't wait again)
        var isSuccess = await IsImmediatelyVisibleAsync(NotificationSuccessSelector);

        if (isSuccess)
        {
            // Read immediately while notification is still visible
            var successMessage = await PageClient.ReadTextContentImmediatelyAsync(NotificationSuccessSelector);
            return new NotificationResult(true, successMessage, null, null);
        }

        var isError = await IsImmediatelyVisibleAsync(NotificationErrorSelector);

        if (isError)
        {
            // Read all error content immediately while notification is still visible
            var errorMessage = await PageClient.ReadTextContentImmediatelyAsync(NotificationErrorMessageSelector);
            var fieldErrors = new List<string>();
            if (await IsImmediatelyVisibleAsync(NotificationErrorFieldSelector))
            {
                fieldErrors = await PageClient.ReadAllTextContentsAsync(NotificationErrorFieldSelector);
            }
            return new NotificationResult(false, null, errorMessage, fieldErrors);
        }

        throw new InvalidOperationException(UnrecognizedNotificationErrorMessage);
    }

    private async Task<bool> IsImmediatelyVisibleAsync(string selector)
    {
        var locator = PageClient.GetLocator(selector);
        var count = await locator.CountAsync();
        return count > 0;
    }

    public async Task<Result<string, SystemError>> GetResultAsync()
    {
        var notification = await DetectAndReadNotificationAsync();

        if (notification.IsSuccess)
        {
            return SystemResults.Success(notification.SuccessMessage!);
        }

        var generalMessage = notification.ErrorMessage!;
        var fieldErrorTexts = notification.FieldErrors!;

        if (!fieldErrorTexts.Any())
        {
            return SystemResults.Failure<string>(generalMessage);
        }

        var fieldErrors = fieldErrorTexts.Select(text =>
        {
            var parts = text.Split(':', 2);

            if(parts.Length != 2)
            {
                throw new InvalidOperationException($"Invalid field error format: {text}");
            }

            return new SystemError.FieldError(parts[0].Trim(), parts[1].Trim());
        }).ToList();

        var error = SystemError.Of(generalMessage, fieldErrors);

        return SystemResults.Failure<string>(error);
    }
}
