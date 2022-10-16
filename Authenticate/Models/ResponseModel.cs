using Microsoft.AspNetCore.Identity;

namespace Authenticate.Models
{
    public class ResponseModel
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public object? Object { get; set; }
        public int? StatusCode { get; set; }
    }
}
