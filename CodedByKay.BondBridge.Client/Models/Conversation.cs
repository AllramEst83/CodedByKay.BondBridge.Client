namespace CodedByKay.BondBridge.Client.Models
{
    public class Conversation
    {
        public Guid ConversationId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string LastMessage { get; set; } = string.Empty;
    }
}
