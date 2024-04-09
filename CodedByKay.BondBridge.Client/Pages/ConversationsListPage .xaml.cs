using CodedByKay.BondBridge.Client.ViewModels;

namespace CodedByKay.BondBridge.Client.Pages
{
    public partial class ConversationsListPage : ContentPage
    {
        private readonly ConversationsListViewModel? viewModel;
        public ConversationsListPage()
        {
            InitializeComponent();

            if (Application.Current?.Handler?.MauiContext?.Services is not null)
            {
                viewModel = Application.Current.Handler.MauiContext.Services.GetService<ConversationsListViewModel>();
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
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = (ConversationsListViewModel)this.BindingContext;
            viewModel.UpdateFilteredConversations(e.NewTextValue);
        }
    }
}
