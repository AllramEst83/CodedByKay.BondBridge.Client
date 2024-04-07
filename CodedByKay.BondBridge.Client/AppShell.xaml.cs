using CodedByKay.BondBridge.Client.Managers;
using CodedByKay.BondBridge.Client.Pages;

namespace CodedByKay.BondBridge.Client
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Register Routes
            Routing.RegisterRoute(nameof(ConversationsListPage), typeof(ConversationsListPage));
            Routing.RegisterRoute(nameof(ConversationPage), typeof(ConversationPage));
            Routing.RegisterRoute(nameof(ThemeSelectorPage), typeof(ThemeSelectorPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(AccountDetailsPage), typeof(AccountDetailsPage));
            Routing.RegisterRoute(nameof(LogPage), typeof(LogPage));
            Routing.RegisterRoute(nameof(LogDetailsPage), typeof(LogDetailsPage));
            Routing.RegisterRoute(nameof(ShowConversationUsersPage), typeof(ShowConversationUsersPage));

            SetAdminVisibilityForLogs();
        }

        private void SetAdminVisibilityForLogs()
        {
            logFlyoutItem.IsVisible = UserSessionManager.Instance.IsAdmin;
        }
    }
}
