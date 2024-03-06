using CodedByKay.BondBridge.Client.Exceptions;
using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Managers;
using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;

namespace CodedByKay.BondBridge.Client.ViewModels
{

    public partial class SigninViewModel : BaseViewModel
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly IUserSecureStorageService _userSecureStorageService;
        private readonly IAuthenticationService _authenticationService;

        [ObservableProperty]
        string userEmail = string.Empty;

        [ObservableProperty]
        string userPassword = string.Empty;

        public SigninViewModel(
            IUserSecureStorageService userSecureStorageService,
            IOptions<ApplicationSettings> applicationSettings,
            IAuthenticationService authenticationService)
        {
            _userSecureStorageService = userSecureStorageService;
            _authenticationService = authenticationService;
            _applicationSettings = applicationSettings.Value;
        }

        [RelayCommand]
        public void GoToRegistrationPage()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                WeakReferenceMessenger.Default.Send(new NavigateToRegistrationMessage());

            });
        }

        [RelayCommand]
        public async Task Signin()
        {
            if (string.IsNullOrEmpty(UserEmail) || string.IsNullOrEmpty(UserPassword))
            {
                await Toast.Make("Oopss! Fyll i både email och lösenord.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                return;
            }

            try
            {
                var authSecrets = await _authenticationService.SignInAsync(UserEmail, UserPassword);

                UserSessionManager.Instance.UpdateSession(authSecrets.AccessToken);

                var userSecrets = new UserSecrets()
                {
                    AccessToken = authSecrets.AccessToken,
                    RefreshToken = authSecrets.Refreshtoken
                };

                await _userSecureStorageService.SetAsync(_applicationSettings.UserSecureStorageKey, userSecrets);

                UserEmail = string.Empty;
                UserPassword = string.Empty;

                await Toast.Make("Huzzuu! Du är Inloggad.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);

                if (Application.Current != null)
                {
                    Application.Current.MainPage = new AppShell();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                await UpdateUI(ex.Message);
            }
            catch (BadRequestException ex)
            {
                await UpdateUI(ex.Message);
            }
            catch (NotFoundException ex)
            {
                await UpdateUI(ex.Message);
            }
            catch (Exception ex)
            {
                await UpdateUI("Ett fel inträffade under inloggningsförsöket.");
            }
        }

        private async Task UpdateUI(string exceptionMesage)
        {
            WeakReferenceMessenger.Default.Send(new FlashSigInInputOnAuthErrorMessage());

            await Toast.Make(exceptionMesage, CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
        }
    }
}
