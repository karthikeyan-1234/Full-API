using API.Models.Base;

namespace API.Models
{
    public class Product : BaseEntity
    {
        public string? productName { get; set; }
        public string? category { get; set; }
        public string? freshness { get; set; }
        public float price { get; set; }
        public DateTime date { get; set; }
        public string? comment { get; set; }
    }
}
