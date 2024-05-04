using FluentValidation;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Data.Email).NotEmpty()
                                  .EmailAddress()
                                  .WithMessage("Invalid Email Address")
                                  .OverridePropertyName("Email");

        RuleFor(x => x.Data.FirstName).NotEmpty()
                                      .WithMessage("First Name is required")
                                      .OverridePropertyName("FirstName");

        RuleFor(x => x.Data.LastName).NotEmpty()
                                     .WithMessage("Last Name is required")
                                     .OverridePropertyName("LastName");

        RuleFor(x => x.Data.Groups).NotEmpty()
                                       .WithMessage("User must belong to at least one group")
                                       .OverridePropertyName("UserGroups");
    }
}


