using CodedByKay.BondBridge.Client.ViewModels;

namespace CodedByKay.BondBridge.Client.Pages;

public partial class LogDetailsPage : ContentPage
{
    private readonly LogDetailsViewModel? viewModel;
    public LogDetailsPage()
    {
        InitializeComponent();

        if (Application.Current?.Handler?.MauiContext?.Services is not null)
        {
            viewModel = Application.Current.Handler.MauiContext.Services.GetService<LogDetailsViewModel>();
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