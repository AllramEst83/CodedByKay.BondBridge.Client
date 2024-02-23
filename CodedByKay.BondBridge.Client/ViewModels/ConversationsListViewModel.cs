using CodedByKay.BondBridge.Client.Models;
using CodedByKay.BondBridge.Client.Pages;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class ConversationsListViewModel : BaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<Conversation> conversations;
        public ConversationsListViewModel()
        {
            Conversations = [];

            Conversations.Add(new Conversation { ConversationId = Guid.NewGuid(), FullName = "Alice", ImagePath = "contact.png", LastMessage = "Hi there!" });
            Conversations.Add(new Conversation { ConversationId = Guid.NewGuid(), FullName = "Bob", ImagePath = "contact.png", LastMessage = "How's it going?" });
        }

        [RelayCommand]
        private async Task GoToConverstaion(Guid conversationId)
        {
            await Toast.Make($"conversationId: {conversationId}", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);

            await Shell.Current.GoToAsync(nameof(ConversationPage));
        }


        [RelayCommand]
        private async Task StartConversation()
        {
            Conversations.Add(new Conversation { ConversationId = Guid.NewGuid(), FullName = "Bob", ImagePath = "contact.png", LastMessage = "How's it going?" });
            await Toast.Make("Vilken användare vill du starta en konverstaion med?", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
        }

        [RelayCommand]
        private async Task DeleteConversation(Conversation conversation)
        {
            if (conversation == null)
            {
                await Toast.Make("Kunde inte tabort konversationen.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                return;
            }

            bool result = false;
            if (Application.Current?.MainPage is not null)
            {
                result = await Application.Current.MainPage.DisplayAlert(
               "Bekräfta borttagning",
               $"Är du säker på att du vill ta bort konvesation med  {conversation.FullName}?",
               "Ja",
               "Nej");
            }
            else
            {
                await Toast.Make("Oops! Ett fel uppstod vid borttagning.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
            }

            if (result)
            {
                var conversationToremove = Conversations.FirstOrDefault(x => x.ConversationId == conversation.ConversationId);
                if (conversationToremove == null)
                {
                    await Toast.Make("Oops! Ett fel uppstod vid borttagning.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                }
                else
                {
                    Conversations.Remove(conversationToremove);
                    await Toast.Make($"Konversation med {conversation.FullName} är bortagen", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                }
            }
            else
            {
                // Optionally handle the cancellation of the delete operation
                await Toast.Make("Borttagning avbruten.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
            }
        }
    }
}
