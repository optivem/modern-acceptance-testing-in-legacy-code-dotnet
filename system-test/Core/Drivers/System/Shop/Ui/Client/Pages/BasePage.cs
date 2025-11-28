using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui.Client.Pages;

public abstract class BasePage
{
    private const string NotificationSelector = "#notifications .notification";
    private const string SuccessNotificationSelector = "[role='alert'].success";
    private const string ErrorNotificationSelector = "[role='alert'].error";

    protected readonly TestPageClient PageClient;

    protected BasePage(TestPageClient pageClient)
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
}
