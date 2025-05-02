using SE.Neo.Common.Enums;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.EmailAlertSender.Models
{
    public class UserEmailAlertItem
    {
        public string UserEmailAddress { get; set; }
        public string UserFirstName { get; set; }
        public EmailAlertCategory EmailAlertCategory { get; set; }
        public EmailAlertFrequency EmailAlertFrequency { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<int> UserRegionIds { get; set; }
        public IEnumerable<int> UserCategoryIds { get; set; }
        public IEnumerable<int> UserRoleIds { get; set; }
        public TimeZone UserTimeZone { get; set; }

        public string UserLastName { get; set; }
    }
}
