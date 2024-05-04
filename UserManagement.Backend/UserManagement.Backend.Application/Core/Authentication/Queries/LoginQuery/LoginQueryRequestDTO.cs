namespace UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;

public class LoginQueryRequestDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
