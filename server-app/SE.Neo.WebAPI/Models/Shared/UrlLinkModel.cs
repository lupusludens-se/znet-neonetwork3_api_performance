using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Shared
{
    public class UrlLinkModel
    {

        [UrlCustom, StringLength(2048)]
        public string UrlLink { get; set; }

        [NotNullOrWhiteSpace, StringLength(250)]
        public string UrlName { get; set; }
    }
}
