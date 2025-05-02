

using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models
{
    public class CompanyFileResponse : BaseFile
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
