using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Company;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class CompanyServiceAzureDecorator : AbstractCompanyServiceDecorator
    {
        private readonly IGraphAPIService _graphAPIService;

        public CompanyServiceAzureDecorator(
            ICompanyService companyService,
            IGraphAPIService graphAPIService)
            : base(companyService)
        {
            _graphAPIService = graphAPIService;
        }
        public override async Task<CompanyDTO> CreateUpdateCompanyAsync(int id, CompanyDTO modelDTO)
        {
            CompanyDTO companyDTO = await base.CreateUpdateCompanyAsync(id, modelDTO);

            await EnsureAzureAccess(companyDTO);

            return companyDTO;
        }

        public override async Task<CompanyDTO> PatchCompanyAsync(int id, JsonPatchDocument patchDoc)
        {
            CompanyDTO companyDTO = await base.PatchCompanyAsync(id, patchDoc);

            await EnsureAzureAccess(companyDTO);

            return companyDTO;
        }

        private async Task EnsureAzureAccess(CompanyDTO companyDTO)
        {
            if (companyDTO.StatusId == (int)CompanyStatus.Inactive)
            {
                IEnumerable<string> userAzureIds = companyDTO.Users
                    .Where(u => u.StatusId == (int)UserStatus.Inactive)
                    .Select(u => u.AzureId);

                List<Task> updateAccessTasks = new List<Task>();
                foreach (string userAzureId in userAzureIds)
                {
                    updateAccessTasks.Add(_graphAPIService.UpdateUserAccessAsync(userAzureId, false));
                }

                Task.WaitAll(updateAccessTasks.ToArray());
            }
        }

    }
}