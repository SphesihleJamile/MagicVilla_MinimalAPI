using AutoMapper;
using MagicVilla_CouponAPI.Models.ViewModels;
using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_CouponAPI.Repositories.Concrete
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDbContext dbContext,
                                IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<CouponReadVM>?> GetAllAsync()
        {
            try
            {
                var model = await _dbContext.Coupons
                            .ToListAsync();

                if (model == null || !model.Any())
                    return null;

                var data = _mapper.Map<List<CouponReadVM>>(model);

                return data.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CouponReadVM?> GetAsync(int id)
        {
            var model = await _dbContext.Coupons
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

            if (model == null)
                return null;

            var data = _mapper.Map<CouponReadVM>(model);

            return data;
        }

        public async Task CreateAsync(CouponCreateVM createCoupon)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(createCoupon);
                await _dbContext.Coupons.AddAsync(coupon);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(int id, CouponUpdateVM updateCoupon)
        {
            try
            {
                var coupon = await GetAsync(id);
                if (coupon == null)
                    return false;
                coupon.Name = updateCoupon.Name;
                coupon.LastUpdated = DateTime.Now;
                coupon.Percent = updateCoupon.Percent;
                coupon.IsActive = updateCoupon.IsActive;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var coupon = await _dbContext.Coupons
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();
                if (coupon == null)
                    return false;
                _dbContext.Coupons.Remove(coupon);
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
