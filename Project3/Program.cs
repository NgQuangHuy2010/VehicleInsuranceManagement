using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project3.Authorization;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;
using Project3.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
// sử dụng 1 chuỗi chung cho EcommerceContext và ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<VehicleInsuranceManagementContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Register Identity services
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

// Configure application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    // options.LoginPath = "/account/login";
    // options.AccessDeniedPath = "/";
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
//load th�ng tin c?u h�nh v� l?u v�o ??i t??ng MailSetting
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
//add dependency inject cho MailService
builder.Services.AddTransient<IMailService, MailService>();
var app = builder.Build();

// Create roles and admin user on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();//ApplicationUser
    await CreateRolesAndAdminUser(roleManager, userManager);
}

// Configure the HTTP request pipeline
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
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();

// Method to create roles and admin user
async Task CreateRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) //ApplicationUser
{
    string[] roleNames = { "Admin", "User" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create a default admin user if it does not exist
    // var admin = new ApplicationUser
    // {
    //     UserName = "huy2010@gmail.com",
    //     Email = "huy2010@gmail.com",
    //     Fullname = "Quang Huy",
    //     Phone = "0123456789"
    // };

    // string adminPassword = "201000";
    // var _admin = await userManager.FindByEmailAsync("huy2010@gmail.com");

    // if (_admin == null)
    // {
    //     var createAdmin = await userManager.CreateAsync(admin, adminPassword);
    //     if (createAdmin.Succeeded)
    //     {
    //         await userManager.AddToRoleAsync(admin, "Admin");
    //     }
    // }
    // Create a default admin user if it does not exist
    //var admin = new ApplicationUser
    //{
    //    UserName = "admin@example.com",
    //    Email = "admin@example.com",
    //    Fullname = "Admin User",
    //    PhoneNumber = "0123456789"
    //};

    //string adminPassword = "Admin@123";
    //var _admin = await userManager.FindByEmailAsync("admin@example.com");

    //if (_admin == null)
    //{
    //    var createAdmin = await userManager.CreateAsync(admin, adminPassword);
    //    if (createAdmin.Succeeded)
    //    {
    //        await userManager.AddToRoleAsync(admin, "Admin");
    //    }
    //}
}
