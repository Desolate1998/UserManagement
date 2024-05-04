using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Backend.Common.ApplicationProviders;

namespace UserManagement.Backend.Domain.Database;

public class UserLoginHistory
{
    public long EntryId { get; set; }
    public long? UserId { get; set; }
    public DateTime Date { get; set; }
    public required string LoginStatus { get; set; }
    public required string Ip { get; set; }
    public virtual User? User { get; set; }
    public virtual LoginStatusLookup? StatusLookup { get; set; }

    public static UserLoginHistory Create(long? userId, string status, string ip)
    {
        return new() { 
            Ip = ip,
            Date = DateTimeProvider.ApplicationDate,
            LoginStatus = status,
            UserId = userId
        };
    }
}
