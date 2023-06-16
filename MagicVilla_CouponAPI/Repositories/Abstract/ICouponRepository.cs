using AutoMapper;
using MagicVilla_CouponAPI.Models.ViewModels;

namespace MagicVilla_CouponAPI.Repositories.Abstract
{
    public interface ICouponRepository
    {
        Task<IEnumerable<CouponReadVM>?> GetAllAsync();
        Task<CouponReadVM?> GetAsync(int id);
        Task CreateAsync(CouponCreateVM createCoupon);
        Task<bool> UpdateAsync(int id, CouponUpdateVM updateCoupon);
        Task<bool> DeleteAsync(int id);
    }
}
