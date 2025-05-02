using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserPatchRequest
    {
        [PatchWhiteList(new string[] { "/statusid" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }
    }
}