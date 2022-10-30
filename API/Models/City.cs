using API.Models.Base;
using System.ComponentModel;

namespace API.Models
{
    public class City : BaseEntity
    {
        public int id { get; set; }
        public string ?name { get; set; }

        public IEnumerable<Employee> ?Employee_Objs { get; set; }
    }
}
