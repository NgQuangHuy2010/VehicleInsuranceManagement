using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView.Identity;
using Project3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// S? d?ng m?t chu?i k?t n?i duy nh?t cho c? EcommerceContext v� ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<VehicleInsuranceManagementContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
// ??ng k� Identity
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
// C?u h�nh cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    //options.LoginPath = "/account/login";  //???ng d?n m� ng??i d�ng s? ???c chuy?n h??ng ??n khi h? c?n ph?i ??ng nh?p ?? truy c?p v�o m?t ph?n c?a ?ng d?ng y�u c?u x�c th?c(n?u ch?a login)
    //options.AccessDeniedPath = "/"; // Chuy?n h??ng v? trang ch? c?a User khi b? t? ch?i quy?n truy c?p
});




builder.Services.AddSingleton<CarService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);


app.Run();
