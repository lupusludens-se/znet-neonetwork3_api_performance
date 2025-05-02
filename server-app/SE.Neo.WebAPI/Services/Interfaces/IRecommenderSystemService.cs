namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IRecommenderSystemService
    {
        Task<IEnumerable<int>> GetRecommendedProjectIds(int userId);
    }
}
