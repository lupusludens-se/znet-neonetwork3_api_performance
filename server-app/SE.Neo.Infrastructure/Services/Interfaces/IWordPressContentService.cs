namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IWordPressContentService
    {
        Task<string> GetContentFromCMS(string paremeter);

        Task<string> GetTaxonomyUpdateDate();

    }
}