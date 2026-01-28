using Optivem.Commons.Http;
using Optivem.Commons.Playwright;
using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Commons;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public abstract class BasePage
{
    // Use role='alert' for semantic HTML and accessibility - matches Java reference
    private const string NotificationSelector = "[role='alert']";
    private const string SuccessNotificationSelector = "[role='alert'].notification.success";
    private const string ErrorNotificationSelector = "[role='alert'].notification.error";
    private const string ErrorMessageSelector = "[role='alert'].notification.error .error-message";
    private const string FieldErrorSelector = "[role='alert'].notification.error .field-error";
    private const string NoNotificationErrorMessage = "No notification appeared";
    private const string UnrecognizedNotificationErrorMessage = "Notification type is not recognized";

    protected readonly PageClient PageClient;

    protected BasePage(PageClient pageClient)
    {
        PageClient = pageClient;
    }

    private bool HasSuccessNotification()
    {
        var hasNotification = PageClient.IsVisible(NotificationSelector);

        if (!hasNotification)
        {
            throw new InvalidOperationException(NoNotificationErrorMessage);
        }

        var isSuccess = PageClient.IsVisible(SuccessNotificationSelector);

        if (isSuccess)
        {
            return true;
        }

        var isError = PageClient.IsVisible(ErrorNotificationSelector);

        if (isError)
        {
            return false;
        }

        throw new InvalidOperationException(UnrecognizedNotificationErrorMessage);
    }

    private string ReadSuccessNotification()
    {
        return PageClient.ReadTextContent(SuccessNotificationSelector);
    }

    private string ReadGeneralErrorMessage()
    {
        return PageClient.ReadTextContent(ErrorMessageSelector);
    }

    private List<string> ReadFieldErrors()
    {
        if (!PageClient.IsVisible(FieldErrorSelector))
        {
            return new List<string>();
        }
        return PageClient.ReadAllTextContents(FieldErrorSelector);
    }

    public Result<string, SystemError> GetResult()
    {
        var isSuccess = HasSuccessNotification();

        if (isSuccess)
        {
            var successMessage = ReadSuccessNotification();
            return SystemResults.Success(successMessage);
        }

        var generalMessage = ReadGeneralErrorMessage();
        var fieldErrorTexts = ReadFieldErrors();

        if (!fieldErrorTexts.Any())
        {
            return SystemResults.Failure<string>(generalMessage);
        }

        var fieldErrors = fieldErrorTexts.Select(text =>
        {
            var parts = text.Split(':', 2);
            if (parts.Length == 2)
            {
                return new SystemError.FieldError(parts[0].Trim(), parts[1].Trim());
            }
            return new SystemError.FieldError("unknown", text);
        }).ToList();

        var error = SystemError.Of(generalMessage, fieldErrors);

        return SystemResults.Failure<string>(error);
    }
}
