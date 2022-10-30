namespace Authenticate.Models
{
    public class Tenant
    {
        public int id { get; set; }
        public string TenantId { get; set; }

        public ICollection<ApplicationUser> ApplicationUser_Objs { get; set; }
    }
}
