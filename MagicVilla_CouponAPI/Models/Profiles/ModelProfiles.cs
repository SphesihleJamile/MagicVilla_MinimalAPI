
using AutoMapper;
using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Models.Profiles
{
    public class ModelProfiles : Profile
    {
        public ModelProfiles()
        {
            CreateMap<Coupon, CouponReadVM>()
                .ReverseMap();
            CreateMap<Coupon, CouponCreateVM>()
                .ReverseMap();
            CreateMap<Coupon, CouponUpdateVM>()
                .ReverseMap();
            CreateMap<LocalUser, UsersVM>()
                .ReverseMap();
            CreateMap<LocalUser, RegistrationRequestVM>()
                .ReverseMap();
        }
    }
}
