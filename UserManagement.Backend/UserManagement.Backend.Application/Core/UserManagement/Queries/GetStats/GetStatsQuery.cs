using ErrorOr;
using MediatR;
using UserManagement.Backend.Common.AuthorizationDetails;
using UserManagement.Backend.Domain.Common;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Application.Core.UserManagement.Queries.GetStats;

public record GetStatsQuery(AuthorizationDetails AuthorizationDetails) : IRequest<ErrorOr<ApplicationStats>>;

public class GetStatsQueryHandler(IUserManagementRepository repository) : IRequestHandler<GetStatsQuery, ErrorOr<ApplicationStats>>
{
    async Task<ErrorOr<ApplicationStats>> IRequestHandler<GetStatsQuery, ErrorOr<ApplicationStats>>.Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        if (!await repository.CheckIfAdminUserAsync(request.AuthorizationDetails.UserId))
        {
            return Error.Unauthorized("Unauthorized");
        }

        return await repository.GetStatsAsync();
    }
}
