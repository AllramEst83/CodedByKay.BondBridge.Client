using CommunityToolkit.Mvvm.Input;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class AboutPageViewModel : BaseViewModel
    {
        [RelayCommand]
        async Task GoToHyperlinkAdress(string adress)
        {
            await Launcher.OpenAsync(adress);
        }
    }
}
