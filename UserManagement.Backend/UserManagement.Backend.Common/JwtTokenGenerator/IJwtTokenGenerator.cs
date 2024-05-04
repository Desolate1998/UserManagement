namespace UserManagement.Backend.Common.JwtTokenGenerator;

public interface IJwtTokenGenerator
{
    string GenerateToken(string email, string firstName, string lastName, long userId);
    string GenerateRefreshToken();
}
