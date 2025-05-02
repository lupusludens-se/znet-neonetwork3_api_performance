using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class UserPendingService : IUserPendingService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserPendingService> _logger;
        private readonly IMapper _mapper;

        public UserPendingService(ApplicationContext context, ILogger<UserPendingService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WrapperModel<UserPendingDTO>> GetUserPendingsAsync(ExpandOrderModel filter)
        {
            var userPendingsQueryable = ExpandSortUserPendings(_context.UserPendings.AsNoTracking(), filter.Expand, filter.OrderBy);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await userPendingsQueryable.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<UserPendingDTO> { Count = count, DataList = new List<UserPendingDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                userPendingsQueryable = userPendingsQueryable.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                userPendingsQueryable = userPendingsQueryable.Take(filter.Take.Value);
            }

            IEnumerable<UserPending> userPendings = await userPendingsQueryable.ToListAsync();
            IEnumerable<UserPendingDTO> userPendingDTOs = userPendings.Select(_mapper.Map<UserPendingDTO>);
            return new WrapperModel<UserPendingDTO> { Count = count, DataList = userPendingDTOs };
        }

        public async Task<UserPendingDTO?> GetUserPendingAsync(int id, string? expand = null)
        {
            var userPendingsQueryable = ExpandSortUserPendings(_context.UserPendings.AsNoTracking(), expand);
            UserPending? userPending = await userPendingsQueryable.SingleOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<UserPendingDTO>(userPending);
        }

        public async Task<UserPendingDTO> CreateUpdateUserPendingAsync(UserPendingDTO modelDTO)
        {
            UserPending userPending;
            if (modelDTO.Id == 0)
            {
                userPending = _mapper.Map<UserPending>(modelDTO);
                await _context.UserPendings.AddAsync(userPending);
            }
            else
            {
                userPending = await EnsureUserPendingExistsAsync(modelDTO.Id);
                if (userPending.CompanyId != modelDTO.CompanyId)
                {
                    Company? company = await _context.Companies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == modelDTO.CompanyId);
                    if (company != null)
                    {
                        modelDTO.CompanyName = company.Name;
                    }
                    if (company == null)
                    {
                        modelDTO.CompanyId = null;
                    }
                }
                _mapper.Map(modelDTO, userPending);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<UserPendingDTO>(userPending);
        }

        public async Task<bool> DeleteUserPendingAsync(int id)
        {
            UserPending? userPending = await EnsureUserPendingExistsAsync(id);
            _context.UserPendings.Remove(userPending);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool IsUserPendingExist(string email, int? exceptId = null)
        {
            return _context.UserPendings.Any(up => up.Email.ToLower() == email.ToLower() && (exceptId == null || up.Id != exceptId));
        }

        public async Task<UserDTO> ApproveUserPendingAsync(UserPendingDTO userPendingDTO)
        {
            UserPending? userPending = await EnsureUserPendingExistsAsync(userPendingDTO.Id);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = _mapper.Map<User>(userPending);
                    user.AzureId = userPendingDTO.AzureId;

                    _context.UserPendings.Remove(userPending);
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    await _context.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = userPending.RoleId });
                    await _context.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = (int)RoleType.All });

                    if (userPendingDTO.RoleId == (int)RoleType.SPAdmin)
                    {
                        var spAdminRolesUserIdsInHisOwnCompany = _context.Users.Include(y => y.Roles).
                        Where(c => c.CompanyId == userPendingDTO.CompanyId && (c.StatusId == Enums.UserStatus.Active || c.StatusId == Enums.UserStatus.Onboard)
                        && c.Roles.Any(x1 => x1.RoleId == (int)RoleType.SPAdmin)).Select(x => x.Id);

                        _context.RemoveRange(_context.UserRoles.Where(a => spAdminRolesUserIdsInHisOwnCompany.Contains(a.UserId) && a.RoleId == (int)RoleType.SPAdmin));
                        var _tempUsers = (spAdminRolesUserIdsInHisOwnCompany.Select(userId => new UserRole() { UserId = userId, RoleId = (int)RoleType.SolutionProvider })).ToList();
                        _context.UserRoles.AddRange(_tempUsers);

                        _context.RemoveRange(_context.UserPermissions.Where(a => spAdminRolesUserIdsInHisOwnCompany.Contains(a.UserId) && a.PermissionId != PermissionType.ProjectManagementOwn));
                    }

                    _context.UserPermissions.AddRange(_context.RolePermissions.Where(rp => rp.RoleId == userPendingDTO.RoleId).Select(item => new UserPermission() { UserId = user.Id, PermissionId = (PermissionType)item.PermissionId }));

                    var emailAlerts = await _context.EmailAlerts.ToListAsync();
                    _context.UserEmailAlerts.AddRange(emailAlerts.Select(ea => new UserEmailAlert
                    {
                        EmailAlertId = ea.Id,
                        Frequency = ea.Frequency,
                        UserId = user.Id,
                    }));

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return _mapper.Map<UserDTO>(user);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Auto Approval :transaction rollback for user pending {userPendingDTO.Email}");
                    _logger.LogInformation($"Auto Approval : exeprion message {ex.Message} : inner exception : {ex.InnerException?.Message}");
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
        }

        public async Task<bool> DenyUserPendingAsync(int id, bool isDenied)
        {
            UserPending? userPending = await EnsureUserPendingExistsAsync(id);
            if (userPending.IsDenied != isDenied)
            {
                userPending.IsDenied = isDenied;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<int> GetPendingUserCountAsync()
        {
            return await _context.UserPendings.CountAsync(u => u.IsDenied == false);
        }

        public async Task DeleteDeniedUserPendingsAsync()
        {

            _context.UserPendings.RemoveRange(_context.UserPendings.Where(u =>
                u.IsDenied && u.ModifiedOn.Value.AddDays(20) <= DateTime.UtcNow));

            await _context.SaveChangesAsync();
        }
    }
}
