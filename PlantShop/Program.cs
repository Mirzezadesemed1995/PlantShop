using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;
using PlantShop.DbInitializer;
using PlantShop.Helpers;
using PlantShop.Models.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();



var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(
    x => x.UseSqlServer(connectionString)
    );
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;

}).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IFileService , FileService>();        
var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
       name: "areas",
            pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}");

app.MapDefaultControllerRoute();


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    await DbInitializer.SeedAsync(userManager, roleManager);

}
app.Run();
