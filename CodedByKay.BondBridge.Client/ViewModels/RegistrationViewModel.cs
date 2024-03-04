using CodedByKay.BondBridge.Client.MessageEvents;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel()
        {

        }

        [RelayCommand]
        public void RegisterUser()
        {
            //registerUser

            MainThread.BeginInvokeOnMainThread(() =>
            {
                WeakReferenceMessenger.Default.Send(new NavigateToSigninMessage());
            });
        }
    }
}
