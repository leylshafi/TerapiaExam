using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TerapiaExam.Enumerations;
using TerapiaExam.Models;

namespace TerapiaExam.Data
{
    public class AppDbContextInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        public AppDbContextInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task Initialize()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task CreateRole()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = role.ToString()
                    });
                }
            }
           
        }

        public async Task CreateAdmin()
        {
            AppUser admin = new() { 
                Name = "admin",
                Surname = "admin",
                Email = "admin@gmail.com",
                UserName = "admin"
            };
            await _userManager.CreateAsync(admin, _config["Admin:Password"]);
            await _userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());

        }
    }
}
