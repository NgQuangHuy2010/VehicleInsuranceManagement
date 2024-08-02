﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
// Add services to the container.
builder.Services.AddControllersWithViews();
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
