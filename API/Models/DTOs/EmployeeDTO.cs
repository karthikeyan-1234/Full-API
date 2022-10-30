using API.Models.Base;

namespace API.Models.DTOs
{
    public class EmployeeDTO : BaseEntity
    {
        public int id { get; set; }
        public string? name { get; set; }
        public float age { get; set; }
        public string? email_id { get; set; }
        public int city_id { get; set; }
    }
}
