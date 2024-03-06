using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Managers;
using CodedByKay.BondBridge.Client.Pages;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class AccountDetailsViewModel : BaseViewModel
    {
        private readonly IUserSecureStorageService _userSecureStorageService;
        [ObservableProperty]
        private bool isAuthenticated;
        [ObservableProperty]
        string userMessage = string.Empty;

        public AccountDetailsViewModel(IUserSecureStorageService userSecureStorageService)
        {
            // Initialize isAuthenticated based on your session state
            IsAuthenticated = UserSessionManager.Instance.IsAuthenticated;

            // Listen to changes in authentication state if needed
            UserSessionManager.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(UserSessionManager.IsAuthenticated))
                {
                    IsAuthenticated = UserSessionManager.Instance.IsAuthenticated;
                    UserMessage = ReturnAuthState();
                }
            };

            _userSecureStorageService = userSecureStorageService;

            UserMessage = ReturnAuthState();
        }

        [RelayCommand]
        private async Task Logout()
        {
            UserSessionManager.Instance.ClearSession();
            _userSecureStorageService.ClearAsync();

            await Toast.Make("Huuzzaa! Du är nu utloggad.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None); ;
            Application.Current.MainPage = new NavigationPage(new SigninPage());
        }

        private string ReturnAuthState()
        {
            return IsAuthenticated ? "Du är inloggad" : "Du är utloggad";
        }
    }
}
