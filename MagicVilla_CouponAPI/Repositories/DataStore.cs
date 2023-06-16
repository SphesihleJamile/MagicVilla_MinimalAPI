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
    }
}
