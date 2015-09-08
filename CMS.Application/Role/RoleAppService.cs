using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.UI;
using CMS.Application.Role.Dto;
using CMS.Domain;
using CMS.Domain.Role;
using CMS.Domain.RoleRight;

namespace CMS.Application.Role
{
    public class RoleAppService : CmsAppServiceBase, IRoleAppService
    {
        private readonly ICmsRepository<RoleEntity, Guid> _repository;
        private readonly ICmsRepository<RoleRightEntity, Guid> _roleRightRepository;

        public RoleAppService(ICmsRepository<RoleEntity, Guid> repository,
            ICmsRepository<RoleRightEntity, Guid> roleRightRepository)
        {
            _repository = repository;
            _roleRightRepository = roleRightRepository;
        }

        public async Task Create(RoleCreateDto input)
        {
            if (await _repository.FirstOrDefaultAsync(x => x.RoleCode == input.RoleCode) != null)
            {
                throw new UserFriendlyException(string.Format(L("DuplicateRoleCode"), input.RoleCode));
            }

            var role = input.MapTo<RoleEntity>();
            role.Id = Guid.NewGuid();

            await _repository.InsertAsync(role);
        }

        public async Task Update(RoleEditDto input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (rs == null)
            {
                throw new UserFriendlyException(string.Format("There is no role with id : {0}", input.Id));
            }

            input.MapTo(rs);

            var result = await _repository.FirstOrDefaultAsync(x => x.RoleCode == input.RoleCode);

            if (result != null && result.Id != input.Id)
            {
                throw new UserFriendlyException(string.Format(L("DuplicateRoleCode"), input.RoleCode));
            }

            await _repository.UpdateAsync(rs);
        }

        public async Task Delete(IdInput<Guid> input)
        {
            await _repository.DeleteAsync(input.Id);
        }

        public async Task<RoleEditDto> GetRole(string roleCode)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.RoleCode == roleCode);
            return rs.MapTo<RoleEditDto>();
        }

        public async Task<RoleEditDto> GetRole(IdInput<Guid> input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            return rs.MapTo<RoleEditDto>();
        }

        public async Task<PagedResultOutput<RoleEditDto>> GetRoles(GetRolesInput input)
        {
            var query = _repository.GetAll();
            var count = await query.CountAsync();
            var users = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            var dtos = users.MapTo<List<RoleEditDto>>();

            return new PagedResultOutput<RoleEditDto>(count, dtos);
        }

        public async Task CreateOrUpdate(IEnumerable<RoleRightDto> inputs)
        {
            foreach (var item in inputs)
            {
                var rs = item.MapTo<RoleRightEntity>();
                if (!item.Id.HasValue)
                {
                    if (await _roleRightRepository.FirstOrDefaultAsync(
                        x => x.ActionModuleId == item.ActionModuleId && x.RoleId == item.RoleId) == null)
                    {
                        rs.Id = Guid.NewGuid();
                        await _roleRightRepository.InsertAsync(rs);
                    }
                }
                else
                {
                    await _roleRightRepository.UpdateAsync(rs);
                }
            }
        }
    }
}