using UserManagement.Frontend.Web.Models.Helper;
using UserManagement.Frontend.Web.Models.Lookups;

namespace UserManagement.Frontend.Web.Models;

public class EditUserRequest
{
    public long EntryId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required ValueChangeTracker<string, bool> Email { get; set; }
    public required List<ValueChangeTracker<string, ushort>> UserGroups { get; set; }

    public Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(FirstName))
            errors.Add("FirstName", "First name is required.");

        if (string.IsNullOrEmpty(LastName))
            errors.Add("LastName", "Last name is required.");

        if (Email == null || string.IsNullOrEmpty(Email.Value))
            errors.Add("Email", "Email is required.");

        if (UserGroups == null || UserGroups.Count == 0)
            errors.Add("Group", "At least one user group is required.");

        return errors;
    }
}
