namespace SE.Neo.Common.Models.Notifications.Details.Base
{
    public class BaseNotificationDetails
    {
        public BaseNotificationDetails()
        {
            this.Type = this.GetType().FullName!;
        }

        [Obsolete] // TODO: Remove this attribute after testing adding notifications via Swagger
        public string Type { get; set; }

    }
}
