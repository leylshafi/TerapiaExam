using Microsoft.EntityFrameworkCore;
using TerapiaExam.Data;
using TerapiaExam.ViewModels;

namespace TerapiaExam.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LayoutVM> GetSettingAsync()
        {
            LayoutVM vm = new()
            {
                Setting = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value)
            };
            return vm;
        }
    }
}
