#nullable disable

using FluentValidation;

namespace Yenko.Users;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().MaximumLength(50);
        RuleFor(user => user.Email).NotEmpty().EmailAddress();
        RuleFor(user => user.Address.Number).NotEmpty().When(user => user.Address != null);
        RuleFor(user => user.Address.City).NotEmpty().MaximumLength(50);
        RuleFor(user => user.Address.State).NotEmpty().MaximumLength(50);
        RuleFor(user => user.Address.Zipcode).NotEmpty().When(user => user.Address != null);
        RuleFor(user => user.Address.Geolocation.Latitude).NotEmpty().When(user => user.Address != null);
        RuleFor(user => user.Address.Geolocation.Longitude).NotEmpty().When(user => user.Address != null);
    }
}