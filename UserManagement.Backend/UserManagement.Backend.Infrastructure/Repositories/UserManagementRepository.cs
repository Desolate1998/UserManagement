using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using UserManagement.Backend.Domain.Common;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.Repositories_Interfaces;
using UserManagement.Backend.Infrastructure.DataContext;

namespace UserManagement.Backend.Infrastructure.Repositories
{
    internal class UserManagementRepository(ManagementContext context) : IUserManagementRepository
    {
        async Task<User> IUserManagementRepository.AddUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        async Task IUserManagementRepository.AssignUserGroupsAsync(List<UserGroup> groups)
        {
            context.UserGroups.AddRange(groups);
            await context.SaveChangesAsync();
        }

        async Task<bool> IUserManagementRepository.CheckIfAdminUserAsync(long userId)
        {
            return await context.UserGroups.Where(x => x.UserId == userId && x.GroupCode == "GRP0000001")
                                           .AnyAsync();
        }

        async Task<bool> IUserManagementRepository.CheckIfUserExistsAsync(long userId)
        {
            return await context.Users.Where(x => x.EntryId == userId).AnyAsync();
        }

        async Task<bool> IUserManagementRepository.CheckIfUserExistsAsync(string email)
        {
            return await context.Users.Where(x => x.Email == email).AnyAsync();
        }

        async Task IUserManagementRepository.DeleteUserAsync(long userId)
        {
            await context.UserLoginHistory.Where(x => x.UserId == userId).ExecuteDeleteAsync();
            await context.UserGroups.Where(x => x.UserId == userId).ExecuteDeleteAsync();
            await context.Users.Where(x => x.EntryId == userId).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }

        async Task<List<User>> IUserManagementRepository.GetAllUsersAsync()
        {
            return await context.Users.Include(x => x.UserGroups).AsNoTracking().Select(x => new User()
            {
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EntryId = x.EntryId,
                UserGroups = x.UserGroups
            }).ToListAsync();
        }

        async Task<List<Group>> IUserManagementRepository.GetGroupsAsync()
        {
            return await context.Groups.Include(g => g.GroupPermissions)
                                       .AsNoTracking()
                                       .Select(g => new Group
                                       {
                                           Code = g.Code,
                                           Name = g.Name,
                                           Description = g.Description,
                                           GroupPermissions = g.GroupPermissions!.Select(gp => new GroupPermission()
                                           {
                                               PermissionCode = gp.PermissionCode,
                                               GroupCode = gp.GroupCode,
                                           }).ToList()
                                       }).ToListAsync();
        }

        async Task<List<Permission>> IUserManagementRepository.GetPermissionAsync()
        {
            return await context.Permissions.AsNoTracking().ToListAsync();
        }

        async Task<ApplicationStats> IUserManagementRepository.GetStatsAsync()
        {
            var GroupStats = (await (from userGroup in context.UserGroups
                                    join groups in context.Groups on userGroup.GroupCode equals groups.Code
                                    group userGroup by groups.Name into grp
                                    select new KeyValuePair<string, long>(grp.Key, grp.Count()))
                                .ToListAsync()).ToDictionary();

            var stats = new ApplicationStats
            {
                UsersCount = await context.Users.LongCountAsync(),
                GroupStats = GroupStats

            };

            return stats;
        }

        async Task<User?> IUserManagementRepository.GetUserByIdAsync(long userId)
        {
            return await context.Users.Where(x => x.EntryId == userId)
                                      .Include(x => x.UserGroups)
                                      .FirstOrDefaultAsync();
        }

        async Task IUserManagementRepository.UpdateUserAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        async Task IUserManagementRepository.UpdateUserGroupsAsync(List<string> groupsToRemove, List<UserGroup> groupsToAdd, long userId)
        {
            if (groupsToRemove.Count != 0)
            {
                await context.UserGroups.Where(x => x.UserId == userId && groupsToRemove.Contains(x.GroupCode)).ExecuteDeleteAsync();
            }

            if (groupsToAdd.Count != 0)
            {
                await context.UserGroups.AddRangeAsync(groupsToAdd);
            }

            await context.SaveChangesAsync();
        }
    }
}
