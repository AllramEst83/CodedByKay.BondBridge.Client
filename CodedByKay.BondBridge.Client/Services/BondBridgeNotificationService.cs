using CodedByKay.BondBridge.Client.Interfaces;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification;

namespace CodedByKay.BondBridge.Client.Services
{
    public class BondBridgeNotificationService : IBondBridgeNotificationService
    {
        public void AddNotifications(int id,int bandgeNumber, string title, string subTitle, string lastmessage, DateTime timeOfNotification, string iconPath)
        {
            var request = new NotificationRequest
            {
                NotificationId = id,
                Title = title,
                Subtitle = subTitle,
                Description = lastmessage,
                BadgeNumber = bandgeNumber,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = timeOfNotification,
                },
                Android = new AndroidOptions()
                {
                    IconLargeName = new AndroidIcon()
                    {
                        ResourceName = iconPath
                    }
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }

        public void RegisterNotificationTapped()
        {
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        }

        private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {

            }
            else if (e.IsTapped)
            {

            }
        }
    }
}
