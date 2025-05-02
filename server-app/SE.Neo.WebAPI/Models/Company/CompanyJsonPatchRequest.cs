using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Company
{
    public class CompanyJsonPatchRequest
    {
        [PatchWhiteList(new string[] { "/statusid" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }
    }
}
