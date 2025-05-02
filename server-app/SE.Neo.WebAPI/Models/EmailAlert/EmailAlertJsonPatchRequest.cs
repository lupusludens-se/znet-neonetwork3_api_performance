using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.EmailAlert
{
    public class EmailAlertJsonPatchRequest
    {
        [PatchWhiteList(new string[] { "/frequency" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }
    }
}
