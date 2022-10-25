using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

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
        IEnumerable<T> GetAllWithProperty(Expression<Func<T, object>> includeProperties);
    }
}