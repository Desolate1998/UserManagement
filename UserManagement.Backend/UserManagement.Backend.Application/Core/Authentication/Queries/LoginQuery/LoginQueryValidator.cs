using FluentValidation;

namespace UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Data.Email).NotEmpty()
                                .EmailAddress()
                                .WithMessage("Invalid Email Address")
                                .OverridePropertyName("Email");

        RuleFor(x => x.Data.Password).NotEmpty()
                                     .WithMessage("Please Provide a Valid Password")
                                     .OverridePropertyName("Password");
    }
}