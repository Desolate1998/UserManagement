using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Backend.Domain.Database;

public class UserGroup
{
    public long UserId { get; set; }
    public required string GroupCode { get; set; }
    public virtual User? User { get; set; }
    public virtual Group? Group { get; set; }

    public static UserGroup Create(long userId, string groupCode)
    {
        return new() 
        { 
            GroupCode = groupCode, 
            UserId = userId 
        };
    }
}
