using FluentValidation;
using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Models.Validators
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateVM>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Coupon Name is Required");
            RuleFor(model => model.Percent)
                .NotEmpty().WithMessage("Coupon Percente is Required")
                .InclusiveBetween(1, 100).WithMessage("Coupon Message must be between 1 & 100 inclusive");
        }
    }
}
