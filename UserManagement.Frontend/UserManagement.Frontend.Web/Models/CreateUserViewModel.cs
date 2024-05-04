using System.Text.Json;
using UserManagement.Frontend.Web.Models.APIModels;

namespace UserManagement.Frontend.Web.Models;

public class CreateUserViewModel
{
    public List<Permission> Permissions { get; set; }
    public List<(Group group, bool selected)> Groups { get; set; }
    public string GetGroupJsonDetails()
    {
        var groupDetails = Groups.Select(g => new
        {
            Group = g.group,
            Selected = g.selected
        });

        return JsonSerializer.Serialize(groupDetails);
    }
}
