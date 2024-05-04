using UserManagement.Backend.Domain.Common;
using UserManagement.Backend.Domain.Database;

namespace UserManagement.Backend.Domain.Repositories_Interfaces;

public interface IUserManagementRepository
{
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
    public Task<User> AddUserAsync(User user);
    public Task UpdateUserAsync(User user);
    public Task DeleteUserAsync(long userId);
    public Task<bool> CheckIfAdminUserAsync(long userId);
    public Task<List<User>> GetAllUsersAsync();
    public Task<bool> CheckIfUserExistsAsync(long userId);
    public Task<bool> CheckIfUserExistsAsync(string email);
    public Task<User?> GetUserByIdAsync(long userId);
    public Task UpdateUserGroupsAsync(List<string> groupsToRemove, List<UserGroup> groupsToAdd, long userId);
    public Task<List<Permission>> GetPermissionAsync();
    public Task<List<Group>> GetGroupsAsync();
    public Task AssignUserGroupsAsync(List<UserGroup> groups);
    public Task<ApplicationStats> GetStatsAsync();

}
