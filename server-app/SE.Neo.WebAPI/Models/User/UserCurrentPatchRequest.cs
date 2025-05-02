using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserCurrentPatchRequest
    {
        [PatchWhiteList(new string[] { "/countryid", "/requestdeletedate" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }
    }
}