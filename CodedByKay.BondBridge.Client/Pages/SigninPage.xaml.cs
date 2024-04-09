using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.ViewModels;
using CommunityToolkit.Maui.Extensions;
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

        WeakReferenceMessenger.Default.Register<FlashSigInInputOnSigninAuthErrorMessage>(this, async (recipient, message) =>
        {
            await FlashInputFieldsAsync();
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WeakReferenceMessenger.Default.Unregister<NavigateToRegistrationMessage>(this);
        WeakReferenceMessenger.Default.Unregister<FlashSigInInputOnSigninAuthErrorMessage>(this);
    }

    private async Task NavigateToRegistrationPage()
    {
        await Navigation.PushAsync(new RegistrationPage());
    }

    private async Task FlashInputFieldsAsync()
    {
        uint animationDuration = 1000;

        await AnimateToErrorBackground(animationDuration);
        await AnimateToDefaultBackground(animationDuration);
        await AnimateToErrorBackground(animationDuration);
        await AnimateToDefaultBackground(animationDuration);
    }

    private async Task AnimateToErrorBackground(uint animationDuration)
    {
        var errorColor = Colors.PaleVioletRed;

        await Task.WhenAll(
                        EmailFrame.BackgroundColorTo(errorColor, animationDuration, easing: Easing.Linear),
                        PasswordFrame.BackgroundColorTo(errorColor, animationDuration, easing: Easing.Linear)
                    );
    }

    private async Task AnimateToDefaultBackground(uint animationDuration)
    {

        var originalColor = Colors.LightGray;
        await Task.WhenAll(
                        EmailFrame.BackgroundColorTo(originalColor, animationDuration, easing: Easing.Linear),
                        PasswordFrame.BackgroundColorTo(originalColor, animationDuration, easing: Easing.Linear)
                    );
    }

}