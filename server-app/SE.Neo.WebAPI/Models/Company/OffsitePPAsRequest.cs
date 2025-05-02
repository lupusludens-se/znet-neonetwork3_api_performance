using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Company
{
    public class OffsitePPAsRequest
    {
        [EnumExist(typeof(OffsitePPAs), "must contain valid offsiteppas id")]
        public OffsitePPAs Id { get; set; }
    }
}