namespace MagicVilla_CouponAPI.Repositories.Abstract
{
    public interface IUnitOfWork
    {
        public ICouponRepository CouponRepository { get; }
        public IAuthRepository AuthRepository { get; }
        Task SaveChangesAsync();
        bool Check(string username);
    }
}
