using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Backend.Domain.Database;

namespace UserManagement.Backend.Domain.Repositories_Interfaces;

public interface IAuthenticationRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task LogLoginRequestAsync(UserLoginHistory userLogin);
}
