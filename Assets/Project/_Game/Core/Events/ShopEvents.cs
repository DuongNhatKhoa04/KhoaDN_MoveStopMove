namespace MoveStopMove.Core.Events
{
    public struct ShopNotification
    {
        public readonly string Notification;
        public readonly int NotificationCode;

        public ShopNotification(string noti, int code)
        {
            Notification = noti;
            NotificationCode = code;
        }
    }
}