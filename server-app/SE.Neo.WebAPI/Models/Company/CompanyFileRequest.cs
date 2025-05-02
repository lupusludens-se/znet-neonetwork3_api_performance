
using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Company
{
    public class CompanyFileRequest : BaseFile
    {
        public bool IsPrivate { get; set; }
    }
}
