using ngo_webapp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Entity Framework and DbContext
//User connect!
builder.Services.AddDbContext<NgoManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));

builder.Services.AddDbContext<NgoManagementContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("AdminConnectionString")), ServiceLifetime.Scoped);
// Configure authentication services here (if you decide to use ASP.NET Core Identity or similar)


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseSession();

app.MapRazorPages();

//Admin
app.UseEndpoints(endpoints =>
{
endpoints.MapControllerRoute(
       name: "admin",
       pattern: "Admin/{controller=Home}/{action=Index}/{id?}",
       new { area = "Admin" } // Đặt area là "Admin"
   );
});
app.Run();
