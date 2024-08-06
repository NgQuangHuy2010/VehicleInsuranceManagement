using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project3.Authorization;
using Project3.Models;
using Project3.ModelsView.Identity;
using Project3.Services;

var builder = WebApplication.CreateBuilder(args);


// sử dụng 1 chuỗi chung cho EcommerceContext và ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<VehicleInsuranceManagementContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
// ??ng ký Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
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

//đăng ký login google
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//    //options.DefaultScheme = GoogleDefaults.AuthenticationScheme;
//    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
//{
//    //clientId và ClientSecret  dc cấu hình ở appsettings.json
//    options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
//    options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
//});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserRoleService, UserRoleService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<IEmail, Email>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddSingleton<CarService>();

var app = builder.Build();
// Tạo vai trò và tài khoản admin mặc định khi khởi động ứng dụng
//CreateRolesAndAdminUser có nhiệm vụ tạo vai trò "Admin" và "User" nếu chúng chưa tồn tại trong hệ thống
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    await CreateRolesAndAdminUser(roleManager, userManager);
}
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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);


app.Run();

// Tạo vai trò và tài khoản admin mặc định
//CreateRolesAndAdminUser có nhiệm vụ tạo vai trò "Admin" và "User" nếu chúng chưa tồn tại trong hệ thống
async Task CreateRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
{
    string[] roleNames = { "Admin", "User" };  //có thể thêm quyền và chạy https để update
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Tạo một tài khoản admin mặc định
    //var admin = new ApplicationUser
    //{
    //    UserName = "huy2010@gmail.com",
    //    Email = "huy2010@gmail.com",
    //    Fullname = "Quang Huy",
    //    Phone = "0123456789"
    //};

    //string adminPassword = "201000";
    //var _admin = await userManager.FindByEmailAsync("huy2010@gmail.com");

    //if (_admin == null)
    //{
    //    var createAdmin = await userManager.CreateAsync(admin, adminPassword);
    //    if (createAdmin.Succeeded)
    //    {
    //        await userManager.AddToRoleAsync(admin, "Admin");
    //    }
    //}
}