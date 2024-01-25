using Microsoft.AspNetCore.Mvc;

namespace TerapiaExam.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

    }
}