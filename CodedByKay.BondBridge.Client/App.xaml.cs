using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Managers;
using CodedByKay.BondBridge.Client.Models;
using CodedByKay.BondBridge.Client.Pages;
using Microsoft.Extensions.Options;

namespace CodedByKay.BondBridge.Client
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = new AppShell();
            CheckUserState(serviceProvider);
        }

        private async void CheckUserState(IServiceProvider serviceProvider)
        {
            // Safely attempt to retrieve the ApplicationSettings and handle the case where it might not be registered or its Value is null
            var appSettingsOption = serviceProvider.GetService<IOptions<ApplicationSettings>>();
            if (appSettingsOption?.Value == null)
            {
                throw new Exception("AppSettings can not be null.");
            }

            // Extract the Value now that we've confirmed it's not null
            var appSettings = appSettingsOption.Value;

            // Attempt to retrieve the IUserSecureStorageService and handle the case where it might not be registered
            var userStateService = serviceProvider.GetService<IUserSecureStorageService>();
            if (userStateService == null)
            {
                // Handle the case where the user state service is not registered, e.g., log an error or throw an exception
                throw new Exception("UserStateService is not registered in the service provider.");
            }

            // Now that we've confirmed userStateService is not null, proceed with fetching the UserSecrets
            var userSecrets = await userStateService.GetAsync<UserSecrets>(appSettings.UserSecureStorageKey);

            if (userSecrets != null) // Implement this method to check user authentication status
            {
                UserSessionManager.Instance.UpdateSession(userSecrets.AccessToken);
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new SigninPage());
            }
        }
    }
}