using TerapiaExam.Models;

namespace TerapiaExam.Areas.Admin.ViewModels
{
    public class UpdateEmployeeVM
    {
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PositionId { get; set; }
        public List<Position>? Positions { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? LinkedinLink { get; set; }
    }
}
