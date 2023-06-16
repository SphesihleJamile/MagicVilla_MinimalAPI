using FluentValidation;
using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Models.Validators
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequestVM>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Name is required");
            RuleFor(model => model.UserName)
                .NotEmpty().WithMessage("UserName is required");
            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
