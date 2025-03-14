﻿using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IRoleBaseRepository
    {
        public Task<IEnumerable<string>> GetPermissionRoleSlugs(int roleId);
        public Task<IEnumerable<User>> GetUsersByRole(int roleId);
        public Task<bool> UpdatePermissionRole(int roleId, List<string> permissionSlug);
        public Task<bool> CreateRole(Role role);
        public Task<bool> DeleteRole(int roleId);
        public Task<IEnumerable<Role>> GetAllRoles();
        public Task<Role> GetRoleById(int roleId);
        public Task<bool> UpdateRole(Role role);
        public Task<int> GetRoleId(string roleName);
        public Task<bool> UpdateUserRole(int roleId, Guid userId);
    }
}
