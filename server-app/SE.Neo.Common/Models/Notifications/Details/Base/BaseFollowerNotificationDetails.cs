namespace SE.Neo.Common.Models.Notifications.Details.Base
{
    public class BaseFollowerNotificationDetails : BaseNotificationDetails
    {
        public int FollowerUserId { get; set; }
        public string FollowerName { get; set; }
    }
}
