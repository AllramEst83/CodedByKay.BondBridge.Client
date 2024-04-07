namespace CodedByKay.BondBridge.Client.Models.Request
{
    public class CreateGroupRequest
    {
        public string GroupName { get; set; } = string.Empty;
        public List<ConversationUserRequest> ConversationUserIds { get; set; }
    }
}
