using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UserManagement.Backend.Common.AuthorizationDetails;

public class AuthorizationDetails
{
    public required string Ip { get; set; }
    public long UserId { get; set; }

    public static AuthorizationDetails Create(IHttpContextAccessor context)
    {
        AuthorizationDetails authorizationDetails = new()
        {
            Ip = context.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Undefined"
        };

        var parsed = long.TryParse(context.HttpContext?.User.FindFirstValue("sid"), out long userId);
        if (parsed)
        {
            authorizationDetails.UserId = userId;
        }
        return authorizationDetails;
    }
}
