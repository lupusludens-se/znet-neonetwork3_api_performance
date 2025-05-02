using AutoMapper;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities.CMS;
using System.Net;

namespace SE.Neo.Core.Mapping
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDTO>()
                .ForMember(dest => dest.ContentTags, opt => opt.MapFrom(src => src.ArticleContentTags.Where(x => x.ContentTag != null).Select(x => x.ContentTag)))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.ArticleRegions.Where(x => x.Region != null).Select(x => x.Region)))
                .ForMember(dest => dest.Solutions, opt => opt.MapFrom(src => src.ArticleSolutions.Where(x => x.Solution != null).Select(x => x.Solution)))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.ArticleTechnologies.Where(x => x.Technology != null).Select(x => x.Technology)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ArticleCategories.Where(x => x.Category != null).Select(x => x.Category)));

            CreateMap<Article, ArticleForInitiativeDTO>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ArticleCategories.Where(x => x.Category != null).Select(x => x.Category)));

            CreateMap<ArticleCMSDTO, ArticleDTO>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => WebUtility.HtmlDecode(src.Title.Name)))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => src.TypeId.FirstOrDefault((int)Core.Enums.ArticleType.Articles)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(id => new CategoryDTO { Id = id })))
                .ForMember(dest => dest.Solutions, opt => opt.MapFrom(src => src.Solutions.Select(id => new SolutionDTO { Id = id })))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.Technologies.Select(id => new TechnologyDTO { Id = id })))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions.Select(id => new RegionDTO { Id = id })))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(id => new RoleDTO { CMSRoleId = id })))
                .ForMember(dest => dest.ContentTags, opt => opt.MapFrom(src => src.ContentTags.Select(id => new ContentTagDTO { Id = id })))
                .ForMember(dest => dest.IsPublicArticle, opts => opts.MapFrom(src => src.CustomFields.IsPublicField));

            CreateMap<Article, ArticleNewAndNoteworthyDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<ArticleDTO, ArticleNewAndNoteworthyDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<ContentTag, ContentTagDTO>();
        }
    }
}