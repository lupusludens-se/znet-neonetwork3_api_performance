using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventJsonPatchRequest
    {
        [PatchWhiteList(new string[] { "/subject", "/description", "/highlights", "/ishighlighted", "/location", "/userregistration" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }
    }
}
