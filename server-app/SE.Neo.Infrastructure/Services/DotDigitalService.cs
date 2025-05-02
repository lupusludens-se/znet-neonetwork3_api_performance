using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Neo.Common.Models.User;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Models.DotDigital;
using SE.Neo.Infrastructure.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace SE.Neo.Infrastructure.Services
{
    public class DotDigitalService : IDotDigitalService
    {
        private readonly DotDigitalConfig _dotDigitalConnection;
        private readonly HttpClient _httpClient;
        private readonly ILogger<DotDigitalService> _logger;

        public DotDigitalService(IOptions<DotDigitalConfig> dotDigitalConnection, HttpClient httpClient, ILogger<DotDigitalService> logger)
        {
            _dotDigitalConnection = dotDigitalConnection.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task CreateContactAndAddUserToAddressBook(UserDTO userProfileDTO, string ConsentIp, string ConsentUserAgent)
        {
            DotDigitalCreateContactResponse responseModel = new();
            string UserName = _dotDigitalConnection.UserName;
            string Password = _dotDigitalConnection.Password;
            var byteArray = Encoding.ASCII.GetBytes($"{UserName}:{Password}");
            string authToken = Convert.ToBase64String(byteArray);
            DotDigitalCreateContact contactModel = new()
            {
                Email = userProfileDTO.Email,
                FirstName = userProfileDTO.FirstName,
                LastName = userProfileDTO.LastName,
                FullName = $"{userProfileDTO.FirstName} {userProfileDTO.LastName}",
                CompanyName = userProfileDTO.Company.Name,
                Country = userProfileDTO.Country?.Name,
                ConsentText = _dotDigitalConnection.ConsentText,
                ConsentUrl = _dotDigitalConnection.ConsentUrl,
                ConsentIp = ConsentIp,
                ConsentUserAgent = ConsentUserAgent
            };
            try
            {
                responseModel = await CreateContact(authToken, contactModel);
                _logger.LogInformation($"Created Dotdigital ContactId {responseModel?.Contact?.id}");
                DotDigitalAddressResponse addressResponse = await AddUserToAddressBook(authToken, contactModel.Email);
                _logger.LogInformation($"Added Contact to Address Book {addressResponse?.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while creating the contact/ adding to the address book for the email {contactModel.Email}. Error: {ex.Message}");
                if (!string.IsNullOrEmpty(responseModel?.Contact?.id))
                {
                    _logger.LogError($"Started deleting the contact in Dotdigital {contactModel.Email}");
                    await DeleteContact(authToken, responseModel.Contact.id, responseModel.Contact.Email);
                }
            }
        }

        private async Task<DotDigitalCreateContactResponse> CreateContact(string authToken, DotDigitalCreateContact contactModel)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_dotDigitalConnection.ConnectionBaseUrl}{_dotDigitalConnection.AddContactsWithConsentAndPreferencesUrl}"),
                Headers =
                        {
                            { "accept", "application/json" },
                            { "authorization", $"Basic {authToken}" },
                        },
                Content = new StringContent("{\"contact\":{\"email\":\"" + contactModel.Email + "\",\"optInType\":\"Single\",\"emailType\":\"Html\"," +
                "\"dataFields\":[" +
                "       {\"key\":\"FIRSTNAME\",\"value\":\"" + contactModel.FirstName + "\"}," +
                "       {\"key\":\"FULLNAME\",\"value\":\"" + contactModel.FullName + "\"}," +
                "       {\"key\":\"LASTNAME\",\"value\":\"" + contactModel.LastName + "\"}," +
                "       {\"key\":\"OPTIN\",\"value\":\"" + _dotDigitalConnection.OptInText + "\"}," +
                "       {\"key\":\"LASTSUBSCRIBED\",\"value\":\"" + DateTime.UtcNow + "\"}," +
                "       {\"key\":\"COMPANY_NAME\",\"value\":\"" + contactModel.CompanyName + "\"}," +
                "       {\"key\":\"COUNTRY\",\"value\":\"" + contactModel.Country + "\"}]}," +
                "\"consentFields\":[" +
                "       {\"fields\":[" +
                "           {\"key\":\"Text\",\"value\":\"" + contactModel.ConsentText + "\"}," +
                "           {\"key\":\"DateTimeConsented\",\"value\":\"" + DateTime.UtcNow + "\"}," +
                "           {\"key\":\"URL\",\"value\":\"" + contactModel.ConsentUrl + "\"}," +
                "           {\"key\":\"IPAddress\",\"value\":\"" + contactModel.ConsentIp + "\"}," +
                "           {\"key\":\"UserAgent\",\"value\":\"" + contactModel.ConsentUserAgent + "\"}" +
                "]}]}")
                {
                    Headers =
                            {
                                ContentType = new MediaTypeHeaderValue("application/json")
                            }
                }
            };
            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<DotDigitalCreateContactResponse>(body);
                return result;
            }
        }

        private async Task<DotDigitalAddressResponse> AddUserToAddressBook(string authToken, string email)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_dotDigitalConnection.ConnectionBaseUrl}{_dotDigitalConnection.AddUserToAddressBookUrl}"),
                Headers =
                        {
                            { "accept", "application/json" },
                            { "authorization", $"Basic {authToken}" },
                        },
                Content = new StringContent("{\"email\":\"" + email + "\",\"optInType\":\"Single\",\"emailType\":\"Html\"}")
                {
                    Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };
            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<DotDigitalAddressResponse>(body);
                return result;
            }
        }

        private async Task DeleteContact(string authToken, string contactId, string email)
        {
            try
            {
                var apiUrl = $"{_dotDigitalConnection.ConnectionBaseUrl}{_dotDigitalConnection.GetContactByIdUrl}";
                apiUrl = string.Format(apiUrl, contactId);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", $"Basic {authToken}" },
                    },
                };
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Completed deleting the contact {email}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting the contact {email}" + ex.Message);
            }
        }
    }
}