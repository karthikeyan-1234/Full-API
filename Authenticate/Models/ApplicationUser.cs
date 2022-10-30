using Microsoft.AspNetCore.Identity;

namespace Authenticate.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string? TenantID { get; set; }
        public virtual Tenant Tenant_obj { get; set; }
    }
}
