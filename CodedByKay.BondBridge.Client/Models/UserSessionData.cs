namespace CodedByKay.BondBridge.Client.Models
{
    public class UserSessionData
    {
        public List<string> Roles { get; set; } = [];
        public string? UserId { get; set; }
        public string? Username { get; set; }
    }
}
