using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TerapiaExam.Data;
using TerapiaExam.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(op =>
{
    op.Password.RequireNonAlphanumeric = false;
    op.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(c =>
{
    c.LoginPath = $"/Admin/Auth/Login/{c.ReturnUrlParameter}";
});

builder.Services.AddScoped<AppDbContextInitializer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
using(var scope = app.Services.CreateScope())
{
    var init = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
    init.Initialize().Wait();
    init.CreateRole().Wait();
    init.CreateAdmin().Wait();
}

app.UseAuthentication();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
