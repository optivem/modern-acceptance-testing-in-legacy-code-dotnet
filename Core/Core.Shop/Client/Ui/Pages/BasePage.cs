using Optivem.Commons.Http;
using Optivem.Commons.Playwright;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public abstract class BasePage
{
    private const string NotificationSelector = "#notifications .notification";
    private const string SuccessNotificationSelector = "[role='alert'].success";
    private const string ErrorNotificationSelector = "[role='alert'].error";
    private const string ErrorMessageSelector = "[role='alert'].error .error-message";
    private const string FieldErrorSelector = "[role='alert'].error .field-error";

    protected readonly PageClient PageClient;

    protected BasePage(PageClient pageClient)
    {
        PageClient = pageClient;
    }

    public bool HasSuccessNotification()
    {
        PageClient.WaitForVisible(NotificationSelector);

        if (PageClient.Exists(SuccessNotificationSelector))
        {
            return true;
        }

        if (PageClient.Exists(ErrorNotificationSelector))
        {
            return false;
        }

        throw new InvalidOperationException("Notification is neither success nor error");
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
}
