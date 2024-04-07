using CodedByKay.BondBridge.Client.Exceptions;
using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        [ObservableProperty]
        string userPassword = string.Empty;

        [ObservableProperty]
        string userEmail = string.Empty;

        [ObservableProperty]
        bool? isEmailValid;

        [ObservableProperty]
        bool? isPasswordValid;

        public bool CanRegister => IsEmailValid.GetValueOrDefault(false) && IsPasswordValid.GetValueOrDefault(false);


        partial void OnIsEmailValidChanged(bool? value)
        {
            OnPropertyChanged(nameof(CanRegister));
        }

        partial void OnIsPasswordValidChanged(bool? value)
        {
            OnPropertyChanged(nameof(CanRegister));
        }

        public RegistrationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [RelayCommand]
        public async Task RegisterUser()
        {
            if (!CanRegister)
            {
                await Toast.Make("Ooppss! Du måste fylla i email och lösenord.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                return;
            }

            try
            {
                await _authenticationService.AddUser(UserEmail, UserPassword);

                await Toast.Make("Zing!!! Vi lyckade registrera dig. Logga in nu.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    WeakReferenceMessenger.Default.Send(new NavigateToSigninMessage());
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                await UpdateUI(ex.Message);
            }
            catch (BadRequestException ex)
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
            WeakReferenceMessenger.Default.Send(new FlashSigInInputOnRegistrationErrorMessage());

            await Toast.Make(exceptionMesage, CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
        }
    }
}
