using FluentValidation;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Domain.Enums;

namespace Identity.Server.Application.Validators;

public class ChangeUserRoleRequestValidator : AbstractValidator<ChangeUserRoleRequest>
{
    public ChangeUserRoleRequestValidator()
    {
        RuleFor(x => x.Role)
        .IsEnumName(typeof(UserRolesEnum), false)
        .WithMessage("Wrong enum name");
    }
}
