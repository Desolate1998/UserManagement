using FluentValidation;
using UserManagement.Backend.Domain.LookupCodes;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.EditUser;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(x => x.Data.FirstName).NotEmpty()
                                      .WithMessage("First Name is required")
                                      .OverridePropertyName("FirstName");

        RuleFor(x => x.Data.LastName).NotEmpty()
                                     .WithMessage("Last Name is required")
                                     .OverridePropertyName("LastName");

        RuleFor(x => x.Data.Email.Value).NotEmpty()
                                        .WithMessage("Email is required")
                                        .OverridePropertyName("Email");

        RuleFor(x => x.Data.Email.Value).EmailAddress()
                                        .WithMessage("Invalid Email Address")
                                        .OverridePropertyName("Email");

        RuleFor(x => x.Data.UserGroups).Must(userGroups => userGroups.Any(x => x.Changed == UserGroupChangeCodes.None 
                                                            || x .Changed == UserGroupChangeCodes.Added))
                                                   .WithMessage("User must have at least one active group")
                                                   .OverridePropertyName("Group");
    }
}