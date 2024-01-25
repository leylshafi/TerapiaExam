using TerapiaExam.Models.Base;

namespace TerapiaExam.Models
{
    public class Employee:BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Job { get; set; }
    }
}
