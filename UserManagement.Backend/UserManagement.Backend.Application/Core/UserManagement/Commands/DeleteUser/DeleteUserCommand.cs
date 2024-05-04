using ErrorOr;
using MediatR;
using UserManagement.Backend.Common.AuthorizationDetails;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.DeleteUser;

public record DeleteUserCommand(DeleteUserCommandRequestDTO  Data, AuthorizationDetails AuthorizationDetails) :IRequest<ErrorOr<bool>>;