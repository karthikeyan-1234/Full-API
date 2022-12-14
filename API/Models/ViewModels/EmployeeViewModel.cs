using API.Models.Base;

namespace API.Models.ViewModels
{
    public class EmployeeViewModel : BaseEntity
    {
        public int id { get; set; }
        public string? name { get; set; }
        public float age { get; set; }
        public string? email_id { get; set; }
        public string? city_name { get; set; }
    }
}
