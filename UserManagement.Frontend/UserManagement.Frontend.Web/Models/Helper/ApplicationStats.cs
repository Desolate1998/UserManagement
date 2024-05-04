using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Frontend.Web.Models.Helper;

public class ApplicationStats
{
    public long UsersCount { get; set; }
    public Dictionary<string, long> GroupStats { get; set; } = [];
}
