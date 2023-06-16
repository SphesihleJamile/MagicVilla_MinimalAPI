using AutoMapper;
using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_CouponAPI.Repositories
{
    public static class DataStore
    {
        public static async Task<IEnumerable<CouponReadVM>?> GetAsync(IMapper mapper)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            var model = await dbContext.Coupons
                            .ToListAsync();

            if (model == null || !model.Any())
                return null;

            var data = mapper.Map<List<CouponReadVM>>(model);

            return data.ToList();
        }

        public static async Task CreateAsync(IMapper mapper, CouponCreateVM createCoupon)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            try
            {
                var coupon = mapper.Map<Coupon>(createCoupon);
                await dbContext.Coupons.AddAsync(coupon);
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<bool> UpdateAsync(int id, CouponUpdateVM updateCoupon)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            try
            {
                var coupon = await dbContext.Coupons.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (coupon == null)
                    return false;
                coupon.Name = updateCoupon.Name;
                coupon.LastUpdated = DateTime.Now;
                coupon.Percent = updateCoupon.Percent;
                coupon.IsActive = updateCoupon.IsActive;
                await dbContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
