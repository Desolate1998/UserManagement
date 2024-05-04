using ErrorOr;
using MediatR;
using UserManagement.Backend.Common.AuthorizationDetails;
using UserManagement.Backend.Domain.Database;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.EditUser;

public record EditUserCommand(EditUserRequestDTO Data, AuthorizationDetails AuthorizationDetails):IRequest<ErrorOr<bool>>;
