
using AutoMapper;
using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Models.Profiles
{
    public class ModelProfiles : Profile
    {
        public ModelProfiles()
        {
            CreateMap<Coupon, CouponVM>()
                .ReverseMap();
            CreateMap<Coupon, CreateCouponVM>()
                .ReverseMap();
            CreateMap<Coupon, UpdateCouponVM>()
                .ReverseMap();
        }
    }
}
