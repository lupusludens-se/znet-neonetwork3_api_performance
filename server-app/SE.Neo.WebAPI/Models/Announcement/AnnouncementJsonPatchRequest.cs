using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Announcement
{
    public class AnnouncementJsonPatchRequest
    {
        [PatchWhiteList(new string[] { "/isactive" })]
        public JsonPatchDocument JsonPatchDocument { get; set; }

    }
}
