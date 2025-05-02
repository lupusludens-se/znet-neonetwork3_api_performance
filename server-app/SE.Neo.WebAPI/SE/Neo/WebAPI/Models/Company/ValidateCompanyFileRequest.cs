namespace SE.Neo.WebAPI.Models.Company
{
    public class ValidateCompanyFileRequest
    {
        public int CompanyId { get;  set; }
        public string FileName { get;  set; }
        public bool IsPrivate { get;  set; }
    }
}