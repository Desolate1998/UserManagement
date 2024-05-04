namespace UserManagement.Frontend.Web.Models.APIModels;

public class User
{
    public long EntryId { get; set; }
    public  string FirstName { get; set; }
    public  string LastName { get; set; }
    public  string Email { get; set; }
    public virtual List<UserGroup>? UserGroups { get; set; }
    
}

