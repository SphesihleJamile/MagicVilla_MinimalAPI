using FluentValidation;
using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Models.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestVM>
    {
        public LoginRequestValidator()
        {
            RuleFor(model => model.UserName)
                .NotEmpty().WithMessage("UserName is required");
            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
