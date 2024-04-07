namespace CodedByKay.BondBridge.Client.Models.Response
{
    public class GroupInfoResponse
    {
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public List<UserInfo> Users { get; set; }
        public List<MessageInfo> Messages { get; set; }
    }

    public class UserInfo
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }

    public class MessageInfo
    {
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
    }
}
