using ErrorOr;
using MediatR;
using UserManagement.Backend.Common.AuthorizationDetails;
using UserManagement.Backend.Domain.Database;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.CreateUser;

public record CreateUserCommand(CreateUserRequestDTO Data, AuthorizationDetails AuthorizationDetails) : IRequest<ErrorOr<User>>;

