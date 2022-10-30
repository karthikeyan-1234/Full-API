using Authenticate.Contexts;
using Authenticate.Models;
using Microsoft.EntityFrameworkCore;

namespace Authenticate.Services
{
    public class TenantService : ITenantService
    {
        private ApplicationDBContext db;
        private DbSet<Tenant> tenants;

        public TenantService(ApplicationDBContext db)
        {
            this.db = db;
            this.tenants = db.Tenants;
        }

        public async Task<ResponseModel> AddNewTenantAsync(RegisterTenantModel tenant)
        {
            var newTenant = tenants.Add(new Tenant { TenantId = tenant.TenantId });
            await db.SaveChangesAsync();
            return new ResponseModel() { Object = newTenant.Entity, Message = "new Tenant Added", Status = "Success", StatusCode = StatusCodes.Status200OK };
        }
    }
}
