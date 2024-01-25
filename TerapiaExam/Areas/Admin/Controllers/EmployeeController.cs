using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerapiaExam.Areas.Admin.ViewModels;
using TerapiaExam.Data;
using TerapiaExam.Models;
using TerapiaExam.Utilities.Extentions;

namespace TerapiaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page, int take = 2)
        {
            double count = await _context.Employees.CountAsync();
            var employees = await _context.Employees.Include(e => e.Position).Skip(page * take).Take(take).ToListAsync();
            PaginationVM<Employee> vm = new()
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = employees
            };
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM vm = new()
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                employeeVM.Positions = await _context.Positions.ToListAsync();
                return View(employeeVM);
            }
            if (!await _context.Positions.AnyAsync(p => p.Id == employeeVM.PositionId))
            {
                employeeVM.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("PositionId", "There is no such position");
                return View(employeeVM);
            }
            if (!employeeVM.Photo.ValidateType())
            {
                employeeVM.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "Incorrect file type");
                return View(employeeVM);
            }
            if (!employeeVM.Photo.ValidateSize())
            {
                employeeVM.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "Incorrect file size");
                return View(employeeVM);
            }

            Employee employee = new()
            {
                Name = employeeVM.Name,
                Surname = employeeVM.Surname,
                PositionId = employeeVM.PositionId,
                ImageUrl = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),
                FacebookLink = employeeVM.FacebookLink,
                InstagramLink = employeeVM.InstagramLink,
                LinkedinLink = employeeVM.LinkedinLink,
                TwitterLink = employeeVM.TwitterLink
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            UpdateEmployeeVM vm = new()
            {
                Name = existed.Name,
                Surname = existed.Surname,
                PositionId = existed.PositionId,
                ImageUrl = existed.ImageUrl,
                FacebookLink = existed.FacebookLink,
                InstagramLink = existed.InstagramLink,
                LinkedinLink = existed.LinkedinLink,
                TwitterLink = existed.TwitterLink,
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateEmployeeVM employeeVM)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            employeeVM.ImageUrl = existed.ImageUrl;
            if (!ModelState.IsValid)
            {
                employeeVM.Positions = await _context.Positions.ToListAsync();
                return View(employeeVM);
            }
            if(!await _context.Positions.AnyAsync(p => p.Id == employeeVM.PositionId))
            {
                employeeVM.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("PositionId", "There is no such position");
                return View(employeeVM);
            }
            if (employeeVM.Photo is not null)
            {
                if (!employeeVM.Photo.ValidateType())
                {
                    employeeVM.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Incorrect file type");
                    return View(employeeVM);
                }
                if (!employeeVM.Photo.ValidateSize())
                {
                    employeeVM.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Incorrect file size");
                    return View(employeeVM);
                }
                existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.ImageUrl = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            existed.Name = employeeVM.Name;
            existed.Surname = employeeVM.Surname;
            existed.PositionId = employeeVM.PositionId;
            existed.FacebookLink = employeeVM.FacebookLink;
            existed.InstagramLink = employeeVM.InstagramLink;
            existed.LinkedinLink = employeeVM.LinkedinLink;
            existed.TwitterLink = employeeVM.TwitterLink;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.Include(e=>e.Position).FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            return View(existed);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
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
