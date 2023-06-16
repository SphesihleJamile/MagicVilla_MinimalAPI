using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Repositories.Abstract;

namespace MagicVilla_CouponAPI.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public ICouponRepository CouponRepository { get; private set; }
        public IAuthRepository AuthRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext,
                            ICouponRepository couponRepository,
                            IAuthRepository authRepository)
        {
            AuthRepository = authRepository;
            CouponRepository = couponRepository;
            this._dbContext = dbContext;
        }

        public bool Check(string name)
        {
            if(_dbContext.LocalUsers.Any())
            {
                return _dbContext.LocalUsers.Any(x => x.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
