using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class RoleBaseRepository : IRoleBaseRepository
    {
        private readonly DevKidContext _context;
        public RoleBaseRepository(DevKidContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRole(Role role)
        {
            _context.Roles.Add(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
            if (role == null) throw new Exception("Role not found");
            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetPermissionRoleSlugs(int roleId)
        {
            return await _context.Roles.Where(x => x.Id == roleId)
                .Include(x => x.Permissions)
                .SelectMany(x => x.Permissions)
                .Select(x => x.Slug)
                .ToListAsync();
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId) ?? throw new Exception("Role not found");
        }

        public async Task<int> GetRoleId(string roleName)
        {
            return await Task.FromResult(_context.Roles.FirstOrDefault(x => x.Name == roleName)?.Id ?? throw new Exception("Role not found"));
        }

        public async Task<IEnumerable<User>> GetUsersByRole(int roleId)
        {
            return await _context.Users.Where(x => x.RoleId == roleId).ToListAsync();
        }

        public async Task<bool> UpdatePermissionRole(int roleId, List<string> permissionSlug)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == roleId) ?? throw new Exception("Role not found");
            var permissions = _context.Permissions.Where(x => permissionSlug.Contains(x.Slug)).ToList();
            // remove old permission
            role.Permissions.Clear();
            // add new permission
            role.Permissions = permissions;
            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserRole(int roleId, Guid userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId) ?? throw new Exception("User not found");
            user.RoleId = roleId;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
