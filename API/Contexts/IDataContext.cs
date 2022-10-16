using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Contexts
{
    public interface IDataContext
    {
        DbSet<City>? Cities { get; set; }
        DbSet<Employee>? Employees { get; set; }

        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry Entry(Object entity);
    }
}