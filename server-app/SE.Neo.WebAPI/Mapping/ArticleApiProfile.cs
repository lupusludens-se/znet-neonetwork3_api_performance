using AutoMapper;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.WebAPI.Models.Article;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Initiative;
using System.Net;

namespace SE.Neo.WebAPI.Mapping
{
    public class ArticleApiProfile : Profile
    {
        public ArticleApiProfile()
        {
            CreateMap<ArticleCMSDTO, ArticleResponse>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => WebUtility.HtmlDecode(src.Title.Name)))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => src.TypeId.FirstOrDefault((int)Core.Enums.ArticleType.Articles)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(id => new CategoryDTO { Id = id })))
                .ForMember(dest => dest.Solutions, opt => opt.MapFrom(src => src.Solutions.Select(id => new SolutionDTO { Id = id })))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.Technologies.Select(id => new TechnologyDTO { Id = id })))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions.Select(id => new RegionDTO { Id = id })));

            CreateMap<ArticleDTO, ArticleResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(cat => new CategoryResponse { Id = cat.Id, Name = cat.Name })))
                .ForMember(dest => dest.Solutions, opt => opt.MapFrom(src => src.Solutions.Select(sol => new SolutionResponse { Id = sol.Id, Name = sol.Name })))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.Technologies.Select(tech => new TechnologyResponse { Id = tech.Id, Name = tech.Name })))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions.Select(reg => new RegionResponse { Id = reg.Id, Name = reg.Name })))
                .ForMember(dest => dest.ContentTags, opt => opt.MapFrom(src => src.ContentTags.Select(ct => new ContentTagResponse { Id = ct.Id, Name = ct.Name })));

            CreateMap<ArticleNewAndNoteworthyDTO, NewAndNoteworthyArticlesResponse>();


            CreateMap<ArticleDTO, InitiativeArticleResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(cat => new CategoryResponse { Id = cat.Id, Name = cat.Name })));

            CreateMap<ArticleForInitiativeDTO, InitiativeArticleResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(cat => new CategoryResponse { Id = cat.Id, Name = cat.Name })));
        }
    }
}