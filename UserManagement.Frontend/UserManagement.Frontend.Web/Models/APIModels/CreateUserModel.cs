using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Text.RegularExpressions;

namespace UserManagement.Frontend.Web.Models.APIModels;

public class CreateUserModel:User
{
    public required List<string> Groups { get; set; }
    public Dictionary<string,string> Validate()
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Dictionary<string, string> validationResults = [];
        if (Groups.Count < 1)
        {
            validationResults.Add("Group", "Please select atleast one group");
        }
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
