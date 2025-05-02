using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class DotDigitalConfig
    {
        [Required]
        public string ConnectionBaseUrl { get; set; }

        [Required]
        public string GetContactByIdUrl { get; set; }

        [Required]
        public string AddContactsWithConsentAndPreferencesUrl { get; set; }

        [Required]
        public string AddUserToAddressBookUrl { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConsentUrl { get; set; }

        [Required]
        public string ConsentText { get; set; }

        [Required]
        public string OptInText { get; set; }
    }
}