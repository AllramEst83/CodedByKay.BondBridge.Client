using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace CodedByKay.BondBridge.Client.Pages;

public partial class RegistrationPage : ContentPage
{
    private readonly RegistrationViewModel? viewModel;
    public RegistrationPage()
    {
        InitializeComponent();

        if (Application.Current?.Handler?.MauiContext?.Services is not null)
        {
            viewModel = Application.Current.Handler.MauiContext.Services.GetService<RegistrationViewModel>();
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
        WeakReferenceMessenger.Default.Register<NavigateToSigninMessage>(this, async (recipient, message) =>
        {
            await NavigateToSiginPage();
        });

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WeakReferenceMessenger.Default.Unregister<NavigateToSigninMessage>(this);
    }

    private async Task NavigateToSiginPage()
    {
        await Navigation.PopAsync();
    }
}