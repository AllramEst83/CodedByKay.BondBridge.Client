using CodedByKay.BondBridge.Client.Models.Response;
using CodedByKay.BondBridge.Client.ViewModels;

namespace CodedByKay.BondBridge.Client.Pages;

public partial class ShowConversationUsersPage : ContentPage
{
    private readonly ShowConversationUsersViewModel? viewModel;
    public ShowConversationUsersPage()
    {
        InitializeComponent();

        if (Application.Current?.Handler?.MauiContext?.Services is not null)
        {
            viewModel = Application.Current.Handler.MauiContext.Services.GetService<ShowConversationUsersViewModel>();
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

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (ConversationUsersList.SelectedItems != null)
        {
            var selectedItems = new List<object>(ConversationUsersList.SelectedItems);
            foreach (var item in selectedItems)
            {
                ConversationUsersList.SelectedItems.Remove(item);
            }
        }
    }


    void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Add new selections
        foreach (var item in e.CurrentSelection)
        {
            var user = item as ConversationUsersResponse; // Adjust type if necessary
            if (!viewModel.SelectedUsers.Contains(user))
            {
                viewModel.SelectedUsers.Add(user);
            }
        }

        // Remove unselected items
        foreach (var item in e.PreviousSelection)
        {
            var user = item as ConversationUsersResponse; // Adjust type if necessary
            if (!e.CurrentSelection.Contains(item))
            {
                viewModel.SelectedUsers.Remove(user);
            }
        }
    }
}