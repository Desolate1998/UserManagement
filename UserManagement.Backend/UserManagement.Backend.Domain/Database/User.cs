namespace UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Common;
using UserManagement.Backend.Common.PasswordHelper;

public class User
{
    public long EntryId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public virtual List<UserGroup>? UserGroups { get; set; }
    public virtual List<UserLoginHistory>? UserLoginHistory { get; set; }
    public static User Create(string firstName, string lastName, string email, string password)
    {
        // Let's generate some salt by playing Dota or LoL
        var salt = PasswordHelper.GenerateSalt();
        return new()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = PasswordHelper.HashPassword(password, salt),
            PasswordSalt = Convert.ToBase64String(salt)
        };

    }
}

