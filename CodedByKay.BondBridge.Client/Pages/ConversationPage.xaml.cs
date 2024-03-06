using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace CodedByKay.BondBridge.Client.Pages;

public partial class ConversationPage : ContentPage
{
    private readonly ConverstaionViewModel? viewModel;
    public ConversationPage()
    {
        InitializeComponent();

        if (Application.Current?.Handler?.MauiContext?.Services is not null)
        {
            viewModel = Application.Current.Handler.MauiContext.Services.GetService<ConverstaionViewModel>();
            if (viewModel is null)
            {
                throw new InvalidOperationException("ContactViewModel service not found.");
            }
        }
        else
        {
            throw new InvalidOperationException("Unable to access services.");
        }

        this.BindingContext = viewModel;
    }


    private void ScrollMessageListToLastMessage()
    {
        // Make sure to check if Messages.Count is greater than 0 to avoid errors
        if (MessagesList.ItemsSource.Cast<object>().Any() && viewModel != null)
        {
            MessagesList.ScrollTo(MessagesList.ItemsSource.Cast<object>().Last(), position: ScrollToPosition.End, animate: true);
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        WeakReferenceMessenger.Default.Register<ScrollMessageListoLastMessage>(this, (recipient, message) =>
        {
            ScrollMessageListToLastMessage();
        });

        await Task.Delay(500);
        ScrollMessageListToLastMessage();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WeakReferenceMessenger.Default.Unregister<ScrollMessageListoLastMessage>(this);
    }
}