using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class AttachContentToInitiativeRequest
    {
        public int ContentId { get; set; }
        [Required, EnumExist(typeof(InitiativeModules), "must contain valid content type")]
        public InitiativeModules ContentType { get; set; }

        public List<int> InitiativeIds { get; set; }
    }
}
