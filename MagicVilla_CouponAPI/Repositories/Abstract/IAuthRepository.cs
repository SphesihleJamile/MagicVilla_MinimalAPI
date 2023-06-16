using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Repositories.Abstract
{
    public interface IAuthRepository
    {
        bool IsUserUnique(string username);
        Task<UsersVM> Register(RegistrationRequestVM registrationRequest);
        Task<LoginResponseVM> Login(LoginRequestVM loginRequest);
    }
}
