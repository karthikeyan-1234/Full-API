using API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace API.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        IDataContext db;
        DbSet<T> table;

        public GenericRepo(IDataContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public async Task<T> AddAsync(T entry) { var res = await table.AddAsync(entry); return res.Entity; }
        public void Delete(T entry) => db.Entry(entry).State = EntityState.Deleted;
        public void Update(T entry) => db.Entry(entry).State = EntityState.Modified;
        public async Task<int> SaveChangesAsync() => await db.SaveChangesAsync();
        public async Task<IEnumerable<T>> GetAllAsync() => await table.AsNoTracking().ToListAsync();
        public IEnumerable<T> Find(Func<T, bool> where) => table.AsNoTracking().Where(where).ToList();

        public IEnumerable<T> GetAllWithProperty(Expression<Func<T, object>> includeProperties)
        {
            var res = table.AsNoTracking().Include(includeProperties);
            return res;
        }
    }
}
