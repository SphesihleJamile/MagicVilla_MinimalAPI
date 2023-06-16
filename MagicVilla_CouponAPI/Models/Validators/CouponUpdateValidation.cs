using FluentValidation;
using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Models.Validators
{
    public class CouponUpdateValidation : AbstractValidator<CouponUpdateVM>
    {
        public CouponUpdateValidation()
        {
            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Coupon name is required");
            RuleFor(model => model.Percent)
                .NotEmpty().WithMessage("Coupon percente is required")
                .InclusiveBetween(1, 100).WithMessage("Coupon percente must be between 1 and 100");
        }
    }
}
