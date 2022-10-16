namespace API.Models
{
    public class City
    {
        public int id { get; set; }
        public string ?name { get; set; }

        public IEnumerable<Employee> ?Employee_Objs { get; set; }
    }
}
