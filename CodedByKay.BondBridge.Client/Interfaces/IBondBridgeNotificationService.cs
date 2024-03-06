namespace CodedByKay.BondBridge.Client.Interfaces
{
    public interface IBondBridgeNotificationService
    {
        void AddNotifications(int id, int bandgeNumber, string title, string subTitle, string lastmessage, DateTime timeOfNotification, string iconPath);
        void RegisterNotificationTapped();
    }
}
