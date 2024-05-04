using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.Repositories_Interfaces;
using UserManagement.Backend.Infrastructure.DataContext;

namespace UserManagement.Backend.Infrastructure.Repositories;

internal class AuthenticationRepository(ManagementContext context) : IAuthenticationRepository
{
    async Task<User?> IAuthenticationRepository.GetUserByEmailAsync(string email)
    {
        return await context.Users.Where(x => x.Email == email)
                                  .AsNoTracking()
                                  .SingleOrDefaultAsync();
    }

    async Task IAuthenticationRepository.LogLoginRequestAsync(UserLoginHistory userLogin)
    {
        context.UserLoginHistory.Add(userLogin);
        await context.SaveChangesAsync();   
    }
}
