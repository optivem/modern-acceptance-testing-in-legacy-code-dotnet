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

    protected readonly PageClient PageClient;

    protected BasePage(PageClient pageClient)
    {
        PageClient = pageClient;
    }

    private async Task<bool> HasSuccessNotificationAsync()
    {
        var hasNotification = await PageClient.IsVisibleAsync(NotificationSelector);

        if (!hasNotification)
        {
            throw new InvalidOperationException(NoNotificationErrorMessage);
        }

        var isSuccess = await PageClient.IsVisibleAsync(NotificationSuccessSelector);

        if (isSuccess)
        {
            return true;
        }

        var isError = await PageClient.IsVisibleAsync(NotificationErrorSelector);

        if (isError)
        {
            return false;
        }

        throw new InvalidOperationException(UnrecognizedNotificationErrorMessage);
    }

    private async Task<string> ReadSuccessNotificationAsync()
    {
        return await PageClient.ReadTextContentAsync(NotificationSuccessSelector);
    }

    private async Task<string> ReadGeneralErrorMessageAsync()
    {
        return await PageClient.ReadTextContentAsync(NotificationErrorMessageSelector);
    }

    private async Task<List<string>> ReadFieldErrorsAsync()
    {
        if (!await PageClient.IsVisibleAsync(NotificationErrorFieldSelector))
        {
            return new List<string>();
        }
        return await PageClient.ReadAllTextContentsAsync(NotificationErrorFieldSelector);
    }

    public async Task<Result<string, SystemError>> GetResultAsync()
    {
        var isSuccess = await HasSuccessNotificationAsync();

        if (isSuccess)
        {
            var successMessage = await ReadSuccessNotificationAsync();
            return SystemResults.Success(successMessage);
        }

        var generalMessage = await ReadGeneralErrorMessageAsync();
        var fieldErrorTexts = await ReadFieldErrorsAsync();

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
