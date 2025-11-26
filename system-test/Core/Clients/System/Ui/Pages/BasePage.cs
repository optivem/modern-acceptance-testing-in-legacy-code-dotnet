using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

public abstract class BasePage
{
    protected readonly TestPageClient PageClient;

    private const string SuccessNotificationSelector = ".alert-success";
    private const string ErrorNotificationSelector = ".alert-danger";

    protected BasePage(TestPageClient pageClient)
    {
        PageClient = pageClient;
    }

    public string ReadSuccessNotification()
    {
        return PageClient.ReadTextContent(SuccessNotificationSelector);
    }

    public string ReadErrorNotification()
    {
        return PageClient.ReadTextContent(ErrorNotificationSelector);
    }

    public bool HasSuccessNotification()
    {
        return PageClient.Exists(SuccessNotificationSelector);
    }

    public bool HasErrorNotification()
    {
        return PageClient.Exists(ErrorNotificationSelector);
    }
}
