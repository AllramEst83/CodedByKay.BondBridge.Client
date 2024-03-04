using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace CodedByKay.BondBridge.Client.Pages;

public partial class SigninPage : ContentPage
{
    private readonly SigninViewModel? viewModel;
    public SigninPage()
    {
        InitializeComponent();

        if (Application.Current?.Handler?.MauiContext?.Services is not null)
        {
            viewModel = Application.Current.Handler.MauiContext.Services.GetService<SigninViewModel>();
            if (viewModel is null)
            {
                throw new InvalidOperationException("ContactViewModel service not found.");
            }
        }
        else
        {
            throw new InvalidOperationException("Unable to access services.");
        }

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        WeakReferenceMessenger.Default.Register<NavigateToRegistrationMessage>(this, async (recipient, message) =>
        {
            await NavigateToRegistrationPage();
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WeakReferenceMessenger.Default.Unregister<NavigateToRegistrationMessage>(this);
    }

    private async Task NavigateToRegistrationPage()
    {
        await Navigation.PushAsync(new RegistrationPage());
    }
}