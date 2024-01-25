using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerapiaExam.Areas.Admin.ViewModels;
using TerapiaExam.Data;
using TerapiaExam.Models;

namespace TerapiaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var positions = await _context.Positions.Include(p=>p.Employees).ToListAsync();
            return View(positions);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Create(CreatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return View();
            if(await _context.Positions.AnyAsync(p=>p.Name==positionVM.Name))
            {
                ModelState.AddModelError("Name", "This position already exists");
                return View();
            }

            Position position = new()
            {
                Name = positionVM.Name,
            };
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Positions.FirstOrDefaultAsync(p=>p.Id==id);
            if (existed is null) return NotFound();
            UpdatePositionVM vm = new()
            {
                Name = existed.Name,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult>Update(int id, UpdatePositionVM positionVM)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (existed is null) return NotFound();
            if (!ModelState.IsValid) return View(positionVM);
            if (await _context.Positions.AnyAsync(p => p.Name == positionVM.Name && p.Id!=id))
            {
                ModelState.AddModelError("Name", "This position already exists");
                return View(positionVM);
            }
            existed.Name = positionVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult>Details(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Positions.Include(p=>p.Employees).FirstOrDefaultAsync(p => p.Id == id);
            if (existed is null) return NotFound();
            return View(existed);
        }

        public async Task<IActionResult>Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Positions.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);
            if (existed is null) return NotFound();
            _context.Positions.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
