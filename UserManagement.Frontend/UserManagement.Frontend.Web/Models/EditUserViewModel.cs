using System.Text.Json;
using System.Text.RegularExpressions;
using UserManagement.Frontend.Web.Models.APIModels;
using UserManagement.Frontend.Web.Models.Lookups;

namespace UserManagement.Frontend.Web.Models;

public class EditUserViewModel : User
{
    public required string ExtraInformation { get; set; }
    public List<Permission> Permissions { get; set; } = [];
    public List<(APIModels.Group group, bool selected)> Groups { get; set; } = [];
    public void SetGroupsAndPermissions(List<APIModels.Group> userGroups, List<Permission> permissions)
    {
        Permissions = permissions;
        var currentGroups = JsonSerializer.Deserialize<List<UserGroup>>(ExtraInformation) ?? [];
        foreach (var group in userGroups)
        {
            Groups.Add((group, currentGroups!.Any(x => x.GroupCode == group.Code)));
        }
    }

    public List<string> GetUserGroupCodes()
    {
        return Groups.Where(x=>x.selected).Select(x=>x.group.Code).ToList();
    }

    public string GetGroupJsonDetails(bool onlySelected = false)
    {
        var groupDetails = Groups.Select(g => new
        {
            Group = g.group,
            Selected = g.selected
        }).ToList();

        if(onlySelected) groupDetails = groupDetails.Where(x=>x.Selected).ToList();

        return JsonSerializer.Serialize(groupDetails);
    }

    public Dictionary<string, string> Validate()
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Dictionary<string, string> validationResults = [];

        if (FirstName.Length < 1)
        {
            validationResults.Add("FirstName", "Invalid First Name");
        }
        if (LastName.Length < 1)
        {
            validationResults.Add("LastName", "Invalid Last Name");
        }
        if (!Regex.IsMatch(Email, pattern))
        {
            validationResults.Add("Email", "Invalid Email");
        }

        return validationResults;
    }
}
