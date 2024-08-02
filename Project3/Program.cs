using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// S? d?ng m?t chu?i k?t n?i duy nh?t cho c? EcommerceContext và ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<VehicleInsuranceManagementContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
// ??ng ký Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
// C?u hình cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    //options.LoginPath = "/account/login";  //???ng d?n mà ng??i dùng s? ???c chuy?n h??ng ??n khi h? c?n ph?i ??ng nh?p ?? truy c?p vào m?t ph?n c?a ?ng d?ng yêu c?u xác th?c(n?u ch?a login)
    //options.AccessDeniedPath = "/"; // Chuy?n h??ng v? trang ch? c?a User khi b? t? ch?i quy?n truy c?p
});





var connectionString = builder.Configuration.GetConnectionString("VehicleInsuranceManagementContext");
builder.Services.AddDbContext<VehicleInsuranceManagementContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddSingleton<CarService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.Run();
