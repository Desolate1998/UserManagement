using ErrorOr;
using MediatR;
using UserManagement.Backend.Common.AuthorizationDetails;
using UserManagement.Backend.Domain.Database;

namespace UserManagement.Backend.Application.Core.UserManagement.Queries.GetUsers;

public record GetUsersQuery(AuthorizationDetails AuthorizationDetails) : IRequest<ErrorOr<List<User>>>;
