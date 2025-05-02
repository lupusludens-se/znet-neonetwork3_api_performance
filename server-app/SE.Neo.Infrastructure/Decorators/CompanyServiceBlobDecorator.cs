using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class CompanyServiceBlobDecorator : AbstractCompanyServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public CompanyServiceBlobDecorator(
            ICompanyService companyService,
            IBlobServicesFacade blobServicesFacade)
            : base(companyService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<WrapperModel<CompanyDTO>> GetCompaniesAsync(BaseSearchFilterModel filter, int? userId = null)
        {
            WrapperModel<CompanyDTO> companiesResult = await base.GetCompaniesAsync(filter, userId);

            List<CompanyDTO> companyDTOs = companiesResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(companyDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
            companiesResult.DataList = companyDTOs;

            return companiesResult;
        }

        public override async Task<CompanyDTO?> GetCompanyAsync(int id, int? userId = null, string? expand = null)
        {
            CompanyDTO? companyDTO = await base.GetCompanyAsync(id, userId, expand);

            await _blobServicesFacade.PopulateWithBlobAsync(companyDTO, dto => dto?.Image, (dto, b) => { if (dto != null) dto.Image = b; });
            if(companyDTO != null && companyDTO.Followers?.Count() > 0)
            {
                List<CompanyFollowerDTO>? companyFollowerDTOs = companyDTO?.Followers.ToList();
                await _blobServicesFacade.PopulateWithBlobAsync(companyFollowerDTOs, dto => dto?.Follower?.Image, (dto, b) => { if (dto != null) dto.Follower.Image = b; });
                companyDTO.Followers = companyFollowerDTOs;
            }

            return companyDTO;
        }

        public override async Task<CompanyDTO> CreateUpdateCompanyAsync(int id, CompanyDTO modelDTO)
        {
            bool isUpdate = id > 0;
            string? oldCompanyImageLogo = isUpdate ? (await base.GetCompanyAsync(id))?.ImageLogo : null;

            CompanyDTO companyDTO = await base.CreateUpdateCompanyAsync(id, modelDTO);

            if (isUpdate)
                if (!string.IsNullOrEmpty(oldCompanyImageLogo) && oldCompanyImageLogo != modelDTO.ImageLogo)
                    await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO() { Name = oldCompanyImageLogo, ContainerName = BlobType.Companies.ToString() });

            return companyDTO;
        }

        public override async Task<WrapperModel<CompanyDTO>> GetFollowedCompaniesAsync(int userId, PaginationModel filter)
        {
            WrapperModel<CompanyDTO> companiesResult = await base.GetFollowedCompaniesAsync(userId, filter);

            List<CompanyDTO> companyDTOs = companiesResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(companyDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
            companiesResult.DataList = companyDTOs;

            return companiesResult;
        }

        public override async Task<bool> DeleteCompanyFileByTypeAsync(int currentUserId, int currentUserCompanyId, int fileId, bool IsPrivate, bool isAdmin)
        {
            string filename = await base.GetBlobFileName(fileId);
            bool isRemoved = await base.DeleteCompanyFileByTypeAsync(currentUserId, currentUserCompanyId, fileId, IsPrivate, isAdmin);

            if (isRemoved && !string.IsNullOrEmpty(filename))
            {
                await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO()
                {
                    Name = filename,
                    ContainerName = BlobType.Companies.ToString()
                });
                return true;
            }
            return false;
        }
    }
}