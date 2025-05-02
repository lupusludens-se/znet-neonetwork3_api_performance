using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectPatchRequest
    {
        [PatchWhiteList(new string[] { "/statusid", "/ispinned" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }

    }
}
