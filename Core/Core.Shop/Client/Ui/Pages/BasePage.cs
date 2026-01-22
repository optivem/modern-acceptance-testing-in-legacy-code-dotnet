using Optivem.Commons.Http;
using Optivem.Commons.Playwright;
using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Common.Error;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public abstract class BasePage
{
    // Use role='alert' for semantic HTML and accessibility - matches Java reference
    private const string NotificationSelector = "[role='alert']";
    private const string SuccessNotificationSelector = "[role='alert'].notification.success";
    private const string ErrorNotificationSelector = "[role='alert'].notification.error";
    private const string ErrorMessageSelector = "[role='alert'].notification.error .error-message";
    private const string FieldErrorSelector = "[role='alert'].notification.error .field-error";

    protected readonly PageClient PageClient;

    protected BasePage(PageClient pageClient)
    {
        PageClient = pageClient;
    }

    public bool HasSuccessNotification()
    {
        // Debug: Check for any notification first
        Console.WriteLine($"[DEBUG] Checking for notifications with selector: {NotificationSelector}");
        var hasNotification = PageClient.IsVisible(NotificationSelector);
        Console.WriteLine($"[DEBUG] Has any notification: {hasNotification}");

        if (!hasNotification)
        {
            // Get page content for debugging
            var pageContent = PageClient.GetPageContent();
            Console.WriteLine($"[DEBUG] Page content sample: {pageContent?.Substring(0, Math.Min(500, pageContent?.Length ?? 0))}");
            throw new InvalidOperationException("No notification appeared");
        }

        // Debug: Check for success notification
        Console.WriteLine($"[DEBUG] Checking for success notification with selector: {SuccessNotificationSelector}");
        var isSuccess = PageClient.IsVisible(SuccessNotificationSelector);
        Console.WriteLine($"[DEBUG] Has success notification: {isSuccess}");

        if (isSuccess)
        {
            return true;
        }

        // Debug: Check for error notification
        Console.WriteLine($"[DEBUG] Checking for error notification with selector: {ErrorNotificationSelector}");
        var isError = PageClient.IsVisible(ErrorNotificationSelector);
        Console.WriteLine($"[DEBUG] Has error notification: {isError}");

        if (isError)
        {
            return false;
        }

        throw new InvalidOperationException("Notification type is not recognized");
    }

    public string ReadSuccessNotification()
    {
        return PageClient.ReadTextContent(SuccessNotificationSelector);
    }

    public List<string> ReadErrorNotification()
    {
        var text = PageClient.ReadTextContent(ErrorNotificationSelector);
        return text.Split('\n').ToList();
    }

    public string ReadGeneralErrorMessage()
    {
        return PageClient.ReadTextContent(ErrorMessageSelector);
    }

    public List<string> ReadFieldErrors()
    {
        return PageClient.ReadAllTextContents(FieldErrorSelector);
    }

    public Result<string, Error> GetResult()
    {
        var isSuccess = HasSuccessNotification();

        if (isSuccess)
        {
            var successMessage = ReadSuccessNotification();
            return Results.Success(successMessage);
        }

        var generalMessage = ReadGeneralErrorMessage();
        var fieldErrorTexts = ReadFieldErrors();

        if (!fieldErrorTexts.Any())
        {
            return Results.Failure<string>(Error.Of(generalMessage));
        }

        var fieldErrors = fieldErrorTexts.Select(text =>
        {
            var parts = text.Split(':', 2);
            if (parts.Length == 2)
            {
                return new Error.FieldError(parts[0].Trim(), parts[1].Trim());
            }
            return new Error.FieldError("unknown", text);
        }).ToArray();

        var error = Error.Of(generalMessage, fieldErrors);

        return Results.Failure<string>(error);
    }
}
