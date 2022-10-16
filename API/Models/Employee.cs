namespace API.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string ?name { get; set; }
        public float age { get; set; }
        public string ?email_id { get; set; }

        public int city_id { get; set; }

        public City ?City_obj { get; set; }
    }
}
