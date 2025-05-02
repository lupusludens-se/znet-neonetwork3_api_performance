using AutoMapper;
using SE.Neo.Common.Models.Saved;
using SE.Neo.WebAPI.Models.Saved;

namespace SE.Neo.WebAPI.Mapping
{
    public class SavedApiProfile : Profile
    {
        public SavedApiProfile()
        {
            CreateMap<ProjectSavedRequest, ProjectSavedDTO>();

            CreateMap<ArticleSavedRequest, ArticleSavedDTO>();

            CreateMap<ForumSavedRequest, ForumSavedDTO>();

            CreateMap<SavedContentItemDTO, SavedContentItemResponse>();
        }
    }
}