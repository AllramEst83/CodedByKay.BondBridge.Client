namespace CodedByKay.BondBridge.Client.Models.Response
{
    public class ConversationUsersResponse
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;
    }
}
