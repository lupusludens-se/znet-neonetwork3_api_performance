using SE.Neo.Core.Entities;
using SE.Neo.Core.Models.Project;

namespace SE.Neo.Core.Factories.Interfaces
{
    public interface IProjectDetailsFactory
    {
        Task<BaseProjectDetails> RemoveManyRelationsAsync(BaseProjectDetails baseProjectDetails);
        Task<BaseProjectDetails> GetProjectDetailsAsync(Project project);
    }
}
