using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Saved;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;

namespace SE.Neo.Core.Mapping
{
    public class SavedProfile : Profile
    {
        public SavedProfile()
        {
            CreateMap<ProjectSavedDTO, ProjectSaved>();

            CreateMap<ArticleSavedDTO, ArticleSaved>();

            CreateMap<ForumSavedDTO, DiscussionSaved>()
                .ForMember(dest => dest.DiscussionId, opt => opt.MapFrom(src => src.ForumId));

            CreateMap<Project, SavedContentItemDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.Technologies.Where(x => x.Technology != null).Select(x => x.Technology)))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions.Where(x => x.Region != null).Select(x => x.Region)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => new List<Category>() { src.Category }))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(x => SavedContentType.Project));

            CreateMap<Article, SavedContentItemDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ArticleCategories.Where(x => x.Category != null).Select(x => x.Category)))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.ArticleRegions.Where(x => x.Region != null).Select(x => x.Region)))
                .ForMember(dest => dest.Solutions, opt => opt.MapFrom(src => src.ArticleSolutions.Where(x => x.Solution != null).Select(x => x.Solution)))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.ArticleTechnologies.Where(x => x.Technology != null).Select(x => x.Technology)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(x => SavedContentType.Article));

            CreateMap<Discussion, SavedContentItemDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Messages.Where(x => x.ParentMessage == null).OrderBy(x => x.CreatedOn).First().Text))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.DiscussionCategories.Where(x => x.Category != null).Select(x => x.Category)))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.DiscussionRegions.Where(x => x.Region != null).Select(x => x.Region)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(x => SavedContentType.Forum));
        }
    }
}
