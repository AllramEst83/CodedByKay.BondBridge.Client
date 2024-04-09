using CodedByKay.BondBridge.Client.Models.Response;

namespace CodedByKay.BondBridge.Client.Models
{
    internal class ConversationGroup
    {
        public Guid ConversationGroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public List<ConversationUsersResponse> ConversationUsers { get; set; }
    }
}
