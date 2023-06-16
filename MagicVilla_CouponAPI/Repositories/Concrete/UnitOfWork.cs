using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Repositories.Abstract;

namespace MagicVilla_CouponAPI.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public ICouponRepository CouponRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext,
                            ICouponRepository couponRepository)
        {
            CouponRepository = couponRepository;
            this._dbContext = dbContext;
        }

        public async Task saveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
