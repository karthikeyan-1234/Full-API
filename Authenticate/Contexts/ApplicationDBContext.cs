using Authenticate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authenticate.Contexts
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tenant> ?Tenants { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var ApplicationUsers = builder.Entity<ApplicationUser>();
            var Tenants = builder.Entity<Tenant>();

            Tenants.HasKey(t => t.TenantId).IsClustered();
            Tenants.Property(t => t.id).ValueGeneratedOnAdd();
            ApplicationUsers.HasOne(a => a.Tenant_obj).WithMany(t => t.ApplicationUser_Objs).HasForeignKey(a => a.TenantID);

            base.OnModelCreating(builder);
        }
    }
}
