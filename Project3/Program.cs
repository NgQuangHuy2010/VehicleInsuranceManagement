using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView.Identity;
using Project3.Services;
using Email = Project3.Models.Email;

var builder = WebApplication.CreateBuilder(args);

// Sử dụng 1 chuỗi kết nối duy nhất cho DbContext và ApplicationDbContext
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

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<UserRoleService, UserRoleService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IEmail, Email>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



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

app.Run();
