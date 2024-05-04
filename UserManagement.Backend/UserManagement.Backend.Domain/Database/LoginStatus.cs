namespace UserManagement.Backend.Domain.Database;

public class LoginStatusLookup
{
    public required string StatusCode { get; set; }
    public required string Description { get; set; }

    public virtual List<UserLoginHistory>? UserLoginHistory { get; set; }

    public static LoginStatusLookup Create(string statusCode, string description)
    {
        return new()
        {
            Description = description,
            StatusCode = statusCode
        };
    }
}
