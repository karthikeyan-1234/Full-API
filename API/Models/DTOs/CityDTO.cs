using API.Models.Base;

namespace API.Models.DTOs
{
    public class CityDTO : BaseEntity
    {
        public int id { get; set; }
        public string? name { get; set; }
    }
}
