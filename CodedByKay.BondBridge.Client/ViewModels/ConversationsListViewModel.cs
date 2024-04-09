using CodedByKay.BondBridge.Client.Exceptions;
using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Managers;
using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.Models;
using CodedByKay.BondBridge.Client.Models.Response;
using CodedByKay.BondBridge.Client.Pages;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class ConversationsListViewModel : BaseViewModel
    {

        [ObservableProperty]
        private string searchInput = string.Empty;
        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private ObservableCollection<Conversation> conversations = [];

        [ObservableProperty]
        private ObservableCollection<Conversation> filteredConversations = [];

        private readonly IBondBridgeDataService _bondBridgeDataService;
        private readonly IPreferencesService _preferencesService;
        private readonly ApplicationSettings _applicationSettings;
        public void UpdateFilteredConversations(string filter)
        {
            FilteredConversations.Clear();
            foreach (var conversation in Conversations.Where(c => string.IsNullOrEmpty(filter) || c.FullName.ToLower().Contains(filter.ToLower())))
            {
                FilteredConversations.Add(conversation);
            }
        }
        public ConversationsListViewModel(
            IBondBridgeDataService bondBridgeDataService,
            IPreferencesService preferencesService,
            IOptions<ApplicationSettings> applicationSettings)
        {
            _bondBridgeDataService = bondBridgeDataService;
            _preferencesService = preferencesService;
            _applicationSettings = applicationSettings.Value;

            RegisterEvents();

            SearchInput = string.Empty;
            Conversations = [];
            GetUserData();
        }

        //[] - Handle if server is offline and othe return message for ConversationsListViewModel
        //[] - Handle diffrent response messages for register page
        //[] - Create add conversation logic

        private void RegisterEvents()
        {
            WeakReferenceMessenger.Default.Register<NewGroupAddedMessage>(this, (recipient, message) =>
            {
                GetUserData();
            });
        }

        private void ClearConversations()
        {
            Conversations.Clear();
            FilteredConversations.Clear();
        }

        public void GetUserData(bool reloadFromServer = false)
        {
            if (!UserSessionManager.Instance.IsAuthenticated)
            {
                return;
            }

            Task.Run(async () =>
            {
                try
                {
                    var userId = UserSessionManager.Instance.UserId;
                    List<GroupInfoResponse> listOfroupInfoResponse = await _bondBridgeDataService.GetData<List<GroupInfoResponse>>($"/api/Group/groupsbyuserid/{userId}");

                    ClearConversations();

                    foreach (var item in listOfroupInfoResponse)
                    {
                        var lastMessage = "Starta en konversation.";
                        if (item.Messages.Count > 0)
                        {
                            lastMessage = item.Messages[0].MessageText;
                        }

                        var converstaion = new Conversation()
                        {
                            ConversationId = item.GroupID,
                            FullName = item.GroupName,
                            LastMessage = lastMessage,
                            ImagePath = item.Users[0].ImagePath
                        };

                        Conversations.Add(converstaion);
                        FilteredConversations.Add(converstaion);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    UpdateUI(ex.Message);
                }
                catch (BadRequestException ex)
                {
                    UpdateUI(ex.Message);
                }
                catch (Exception ex)
                {
                    UpdateUI("Ett fel inträffade under inloggningsförsöket.");
                }
            });
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
            await Shell.Current.GoToAsync(nameof(ShowConversationUsersPage));
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

            await _bondBridgeDataService.Delete("/api/group/deletegroup", conversation.ConversationId);

            if (result)
            {
                var conversationToremove = Conversations.FirstOrDefault(x => x.ConversationId == conversation.ConversationId);
                var filteredConversationToremove = FilteredConversations.FirstOrDefault(x => x.ConversationId == conversation.ConversationId);
                if (conversationToremove == null || filteredConversationToremove == null)
                {
                    await Toast.Make("Oops! Ett fel uppstod vid borttagning.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                }
                else
                {
                    Conversations.Remove(conversationToremove);
                    FilteredConversations.Remove(filteredConversationToremove);
                    await Toast.Make($"Konversation med {conversation.FullName} är bortagen", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                }
            }
            else
            {
                // Optionally handle the cancellation of the delete operation
                await Toast.Make("Borttagning avbruten.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
            }
        }

        [RelayCommand]
        public void RefreshConversations()
        {
            GetUserData();

            IsRefreshing = false;
        }

        private void UpdateUI(string exceptionMesage)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Toast.Make(exceptionMesage, CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
            });

        }
    }
}
