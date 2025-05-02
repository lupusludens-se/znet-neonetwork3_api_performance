using SE.Neo.Common.Models.Media;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IBlobService
    {
        Task<string> CreateBlobAsync(BlobBaseDTO modelDTO);
        Task DeleteBlobAsync(BlobBaseDTO modelDTO);
        bool IsBlobExist(string name);
    }
}
