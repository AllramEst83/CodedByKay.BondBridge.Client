namespace CodedByKay.BondBridge.Client.Models
{
    public class ApplicationSettings
    {
        public string CustomHeader { get; set; } = string.Empty;
        public string UserSecureStorageKey { get; set; } = string.Empty;
        public string UserPreferancesKey { get; set; } = string.Empty;
        public string BondBridgeApiBaseUrl { get; set; } = string.Empty;
        public string AppUserRegistrationToken { get; set; } = string.Empty;
        public string NewGroupAdded { get; set; } = string.Empty;
    }
}
