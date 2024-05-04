using FluentValidation;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Data.UserId).NotNull().OverridePropertyName("Email");
    }
}

