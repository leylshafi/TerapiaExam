using TerapiaExam.Models.Base;

namespace TerapiaExam.Models
{
    public class Employee:BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? LinkedinLink { get; set; }
    }
}
