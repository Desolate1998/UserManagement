namespace UserManagement.Backend.Application.Core.UserManagement.Commands.CreateUser;

public class CreateUserRequestDTO
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required List<string> Groups { get; set; }
}


