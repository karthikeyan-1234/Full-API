using API.Models;
using API.Models.Base;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Contexts
{

    public class DataContext : DbContext, IDataContext
    {
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<City>? Cities { get; set; }
        IConfiguration configration;
        IHttpContextAccessor accessor;
        string? TenantID;

        public DataContext(DbContextOptions<DataContext> options,IConfiguration configuration, IHttpContextAccessor accessor) : base(options)
        {
            this.configration = configuration;
            this.accessor = accessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configration.GetConnectionString("DataContext"));
            var session = accessor?.HttpContext?.Session;
            TenantID = session?.GetString("tenantid");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var Employees = modelBuilder.Entity<Employee>().HasQueryFilter(e => e.TenantId == TenantID);
            var Cities = modelBuilder.Entity<City>().HasQueryFilter(e => e.TenantId == TenantID);


            Employees.HasKey(e => e.id);
            Employees.Property(e => e.id).UseIdentityColumn(1, 1);

            Cities.HasKey(c => c.id);
            Cities.Property(c => c.id).UseIdentityColumn(1, 1);
            //Cities.HasIndex(c => c.name).IsClustered(); Only one clustered index allowed per table. This is only for demo purposes

            Employees.HasOne(e => e.City_obj).WithMany(c => c.Employee_Objs).HasForeignKey(e => e.city_id);


            base.OnModelCreating(modelBuilder);
        }
    }
}
