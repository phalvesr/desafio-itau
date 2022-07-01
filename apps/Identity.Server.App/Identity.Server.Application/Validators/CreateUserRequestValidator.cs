using System.Text.RegularExpressions;
using FluentValidation;
using Identity.Server.Application.Models.Requests;

namespace Identity.Server.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username)
        .MinimumLength(3)
        .WithMessage("Username must be at least 3 characters long")
        .NotEmpty().WithMessage("An username must be provided when creating a nem user");

        RuleFor(x => x.Password)
        .MinimumLength(8)
        .WithMessage("The password should be at least 8 characters long")
        .Must(CheckForAtLeastOneSpecialCharacter)
        .WithMessage("Password must contain at least one of the following characters: !, @, #, $, *, ?");
    }

    private bool CheckForAtLeastOneSpecialCharacter(string password)
    {
        return Regex.IsMatch(password, @"[!|@|#|$|*|?]",
            RegexOptions.IgnoreCase | RegexOptions.Singleline,
        TimeSpan.FromMilliseconds(100));
    }
}
