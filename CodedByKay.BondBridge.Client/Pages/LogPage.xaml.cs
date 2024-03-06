using CodedByKay.BondBridge.Client.ViewModels;

namespace CodedByKay.BondBridge.Client.Pages;

public partial class LogPage : ContentPage
{
    private readonly LogViewModel? viewModel;
    public LogPage()
    {
        InitializeComponent();

        if (Application.Current?.Handler?.MauiContext?.Services is not null)
        {
            viewModel = Application.Current.Handler.MauiContext.Services.GetService<LogViewModel>();
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
}