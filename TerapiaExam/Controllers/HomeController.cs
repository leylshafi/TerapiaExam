using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerapiaExam.Data;
using TerapiaExam.ViewModels;

namespace TerapiaExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM vm = new()
            {
                Employees = await _context.Employees.Include(e=>e.Position).ToListAsync()
            };
            return View(vm);
        }

    }
}