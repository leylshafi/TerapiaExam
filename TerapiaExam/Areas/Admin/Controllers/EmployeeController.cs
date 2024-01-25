using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerapiaExam.Areas.Admin.ViewModels;
using TerapiaExam.Data;
using TerapiaExam.Models;
using TerapiaExam.Utilities.Extentions;

namespace TerapiaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page, int take=2)
        {
            double count = await _context.Employees.CountAsync();
            var employees = await _context.Employees.Skip(page*take).Take(take).ToListAsync();
            PaginationVM<Employee> vm = new()
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = employees
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(CreateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid) return View();
            if (!employeeVM.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "Incorrect file type");
                return View();
            }
            if (!employeeVM.Photo.ValidateSize())
            {
                ModelState.AddModelError("Photo", "Incorrect file size");
                return View();
            }

            Employee employee = new()
            {
                Name = employeeVM.Name,
                Surname = employeeVM.Surname,
                Job = employeeVM.Job,
                ImageUrl = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img")
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult>Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            UpdateEmployeeVM vm = new()
            {
                Name = existed.Name,
                Surname = existed.Surname,
                Job = existed.Job,
                ImageUrl = existed.ImageUrl
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult>Update(int id, UpdateEmployeeVM employeeVM)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            employeeVM.ImageUrl = existed.ImageUrl;
            if (!ModelState.IsValid) return View(employeeVM);
            if(employeeVM.Photo is not null)
            {
                if (!employeeVM.Photo.ValidateType())
                {
                    ModelState.AddModelError("Photo", "Incorrect file type");
                    return View(employeeVM);
                }
                if (!employeeVM.Photo.ValidateSize())
                {
                    ModelState.AddModelError("Photo", "Incorrect file size");
                    return View(employeeVM);
                }
                existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.ImageUrl = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            existed.Name = employeeVM.Name;
            existed.Surname = employeeVM.Surname;
            existed.Job = employeeVM.Job;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult>Details(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            return View(existed);
        }

        public async Task<IActionResult>Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img");
            _context.Employees.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
