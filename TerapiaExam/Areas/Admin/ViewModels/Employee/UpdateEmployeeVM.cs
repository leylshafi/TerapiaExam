namespace TerapiaExam.Areas.Admin.ViewModels
{
    public class UpdateEmployeeVM
    {
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Job { get; set; }
    }
}
