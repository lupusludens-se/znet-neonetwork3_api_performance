namespace SE.Neo.WebAPI.Models.User
{
    public class UserExportResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Roles { get; set; }
        public string Status { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Regions { get; set; }
        public string? ApprovedBy { get; set; }
    }
}