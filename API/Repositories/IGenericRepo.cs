using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Repositories
{
    public interface IGenericRepo<T> where T : class
    {
        Task<EntityEntry> AddAsync(T entry);
        void Delete(T entry);
        IEnumerable<T> Find(Func<T, bool> where);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> SaveChangesAsync();
        void Update(T entry);
    }
}