using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Managers;
using CodedByKay.BondBridge.Client.MessageEvents;
using CodedByKay.BondBridge.Client.Models;
using CodedByKay.BondBridge.Client.Models.Request;
using CodedByKay.BondBridge.Client.Models.Response;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class ShowConversationUsersViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<ConversationUsersResponse> conversationUsers = [];

        [ObservableProperty]
        ObservableCollection<ConversationUsersResponse> selectedUsers = [];

        private readonly IBondBridgeDataService _bondBridgeDataService;
        private readonly IPreferencesService _preferencesService;
        private readonly ApplicationSettings _applicationSettings;
        public ShowConversationUsersViewModel(
            IBondBridgeDataService bondBridgeDataService,
                        IPreferencesService preferencesService,
            IOptions<ApplicationSettings> applicationSettings)
        {
            _bondBridgeDataService = bondBridgeDataService;
            _preferencesService = preferencesService;
            _applicationSettings = applicationSettings.Value;
            LoadUsers();
        }

        [RelayCommand]
        public async Task StartaConversation()
        {
            if (SelectedUsers.Count == 0)
            {
                await Toast.Make("Du måste välja minst 1 användare för att starta en konversation.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
            }

            var currentUser = new ConversationUserRequest()
            {
                UserId = Guid.Parse(UserSessionManager.Instance.UserId)
            };

            var groupName = string.Join(", ", SelectedUsers.Select(x => x.UserName));

            var createGroupRequest = new CreateGroupRequest()
            {
                GroupName = groupName,
                ConversationUserIds = [currentUser],
            };

            foreach (var user in SelectedUsers)
            {
                createGroupRequest.ConversationUserIds.Add(new ConversationUserRequest()
                {
                    UserId = user.UserID
                });
            }

            var resposne = await _bondBridgeDataService.CreateGroup(createGroupRequest, "/api/group/creategroup");

            WeakReferenceMessenger.Default.Send<NewGroupAddedMessage>();

            await Shell.Current.GoToAsync("../");
        }

        public void LoadUsers()
        {
            Task.Run(async () =>
            {
                var userId = UserSessionManager.Instance.UserId;
                var userData = await _bondBridgeDataService.GetData<List<ConversationUsersResponse>>($"/api/usermanager/getusers/{userId}");

                foreach (var conversationUser in userData)
                {
                    ConversationUsers.Add(conversationUser);
                }
            });
        }
    }
}
