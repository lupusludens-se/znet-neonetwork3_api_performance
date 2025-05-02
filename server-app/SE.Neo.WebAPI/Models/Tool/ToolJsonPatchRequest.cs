using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Tool
{
    public class ToolJsonPatchRequest
    {
        [PatchWhiteList(new string[] { "/isactive" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }

    }
}
