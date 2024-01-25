using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerapiaExam.Areas.Admin.ViewModels;
using TerapiaExam.Data;
using TerapiaExam.Models;

namespace TerapiaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _context.Settings.ToListAsync();
            return View(settings);
        }

        public async Task<IActionResult>Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            UpdateSettingVM vm = new()
            {
                Key = existed.Key,
                Value = existed.Value,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult>Update(int id, UpdateSettingVM settingVM)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            if (await _context.Settings.AnyAsync(s => s.Key == settingVM.Key && s.Id!=id))
            {
                ModelState.AddModelError("Key", "This key already exists");
                return View(settingVM);
            }
            existed.Key= settingVM.Key;
            existed.Value= settingVM.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
