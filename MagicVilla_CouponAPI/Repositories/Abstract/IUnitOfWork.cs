namespace MagicVilla_CouponAPI.Repositories.Abstract
{
    public interface IUnitOfWork
    {
        public ICouponRepository CouponRepository { get; }
        Task SaveChangesAsync();
    }
}
