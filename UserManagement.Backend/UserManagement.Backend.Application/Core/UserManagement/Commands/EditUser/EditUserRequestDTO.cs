using UserManagement.Backend.Common.ValueChangeTracker;
using UserManagement.Backend.Domain.LookupCodes;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.EditUser;

public class EditUserRequestDTO
{
    public long EntryId { get; set; }
    public required string  FirstName { get; set; }
    public required string  LastName { get; set; }
    public required ValueChangeTracker<string, bool> Email { get; set; }
    public required List<ValueChangeTracker<string, UserGroupChangeCodes>> UserGroups { get; set; }

}
