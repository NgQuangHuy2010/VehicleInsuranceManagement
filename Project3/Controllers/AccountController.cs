﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project3.Models;
using Project3.ModelsView.Identity;

namespace Project3.Controllers
{
    public class AccountController : Controller
    {
        VehicleInsuranceManagementContext db = new VehicleInsuranceManagementContext();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly UserRoleService _userRoleService;
        private readonly IConfiguration _configuration;
        private readonly IEmail _emailService;
        public AccountController(/*UserRoleService userRoleService,*/ UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmail emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            //_userRoleService = userRoleService;
            _configuration = configuration;
            _emailService = emailService;
        }


        [Route("login")]
        public IActionResult Login()
        {
            //kiểm tra xem user đã đăng nhập chưa nếu có quay về home
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel login)
        {
            if (ModelState.IsValid)
            {
                //lockoutOnFailure: false, Không khóa tài khoản sau nhiều lần đăng nhập thất bại
                //isPersistent: false, Cookie xác thực sẽ là cookie phiên, và người dùng sẽ bị đăng xuất khi đóng trình duyệt
                //Hàm PasswordSignInAsync xác thực người dùng bằng cách kiểm tra mật khẩu
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded) // nếu check thành công
                {
                    return RedirectToAction("Index", "Home");
                    //// tiếp tục tìm email đang đăng nhập 
                    //var user = await _userManager.FindByEmailAsync(login.Email);

                    ////kiểm tra nếu role là Admin (cấu hình ở program.cs) 
                    //if (await _userManager.IsInRoleAsync(user, "Admin"))
                    //{
                    //	// return về trang admin 
                    //	return RedirectToAction("Index", "Dashboard", new { area = "System" });
                    //}
                    //else if (!await _userManager.IsInRoleAsync(user, "User"))
                    //{
                    //	return RedirectToAction("Index", "Dashboard", new { area = "System" });


                    //}
                    //else
                    //{
                    //	//nếu ko phải admin về home cho user
                    //	return RedirectToAction("Index", "Home");
                    //}
                }
                //check fail email and pass
                ModelState.AddModelError("Email", "Email or password is incorrect!!!");
            }
            return View(login);
        }


        [Route("register")]
        public IActionResult Register()
        {
            //kiểm tra xem user đã đăng nhập chưa nếu có quay về home
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {

            if (ModelState.IsValid)
            {
                // Tìm kiếm người dùng theo địa chỉ email đã nhập để kiểm tra sự tồn tại.
                var existingUser = await _userManager.FindByEmailAsync(register.Email);
                if (existingUser != null)
                {
                    // Thêm lỗi vào ModelState nếu email đã tồn tại trong hệ thống.
                    ModelState.AddModelError("Email", "E-mail is being used!!!");
                    // Trả về view đăng ký với lỗi email đã tồn tại.
                    return View(register);
                }


                var user = new ApplicationUser
                {
                    UserName = register.Email,  // Đặt UserName của người dùng bằng email.
                    Email = register.Email,  // Đặt Email của người dùng bằng email đã nhập.
                    Fullname = register.Fullname,  // Đặt tên đầy đủ của người dùng.
                    Phone = register.Phone  // Đặt số điện thoại của người dùng.
                };

                // Đăng ký người dùng mới với mật khẩu đã nhập.
                var result = await _userManager.CreateAsync(user, register.Password);

                // Kiểm tra xem việc tạo người dùng có thành công không.
                if (result.Succeeded)
                {
                    // Kiểm tra xem vai trò "User" đã tồn tại chưa.
                    //if (!await _roleManager.RoleExistsAsync("User"))
                    //{
                    //	// Tạo vai trò "User" nếu chưa tồn tại.
                    //	await _roleManager.CreateAsync(new IdentityRole("User"));
                    //}
                    //// Gán vai trò "User" cho người dùng mới.
                    //await _userManager.AddToRoleAsync(user, "User");

                    // Tạo một token xác nhận email cho người dùng mới.
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // Tạo liên kết xác nhận email chứa token và userId.qua action ConfirmEmail
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    // Gửi email xác nhận với liên kết xác nhận bằng SendEmailAsync email.cs trong model
                    await _emailService.SendEmailAsync(register.Email, "Confirm Email",
                        $"Please confirm your account by clicking this link: {confirmationLink}");
                    //gui thong bao den viewlogin
                    TempData["ConfirmEmailSent"] = "Please check your email to login.";
                    // Chuyển hướng đến trang thông báo đã gửi email.
                    return RedirectToAction("Login");
                }

                // Xử lý các lỗi trả về từ quá trình đăng ký không thành công.
                foreach (var error in result.Errors)
                {
                    if (error.Code == "PasswordTooShort")
                    {
                        ModelState.AddModelError("PasswordTooShort", error.Description);
                    }
                    else if (error.Code == "PasswordRequiresLower")
                    {
                        ModelState.AddModelError("PasswordRequiresLower", error.Description);
                    }
                    else
                    {
                        // Thêm lỗi vào ModelState để hiển thị cho người dùng.
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }

            // Nếu ModelState không hợp lệ, trả về view đăng ký với các lỗi.
            return View(register);
        }


        [Route("confirm-email")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // Kiểm tra xem userId và token có hợp lệ không.
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Xác nhận email của người dùng với token.
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["ConfirmEmailSuccess"] = "Email verification successful. You can now login.";

                return View("Login");
            }

            return View("Error");
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm kiếm người dùng bằng email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // Kiểm tra xem người dùng có tồn tại ko và đã dc xác thực email chưa
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError("Email", "Email does not exist!!!");
                    return View(model);
                }
                // tạo token để reset pass
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(  //callback tới url có action ResetPassword với url là token + email 
                    "ResetPassword",
                    "Account",
                    new { token, email = user.Email },
                protocol: HttpContext.Request.Scheme);

                // gửi mail bằng email.cs (trong model) đã dc cấu hình 
                await _emailService.SendEmailAsync(
                    model.Email,
                    "Reset Password",
                    $"Please reset your password by clicking here: {callbackUrl}");

                TempData["ForgotPasswordMessage"] = "Password reset link has been sent to your email.";

                // Redirect to the Login action
                return RedirectToAction("Login");
            }

            // Nếu có lỗi, hiển thị lại biểu mẫu
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token = null, string email = null)
        {
            // kiểm tra xem token và email có null ko
            if (token == null || email == null)
            {
                return RedirectToAction("Error");

            }
            //tìm email 
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("Error");
            }
            // xử lý vô hiệu hóa token đã dc dùng
            var tokenIsValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token);
            if (!tokenIsValid)
            {
                return RedirectToAction("Error");
            }
            //gửi về view token và email (có input hidden ở view)
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // tìm email 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {

                return RedirectToAction("Login");
            }

            // Reset password với token và pass mới 
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                //lưu thông báo bằng tempdata
                TempData["ResetPasswordConfirmation"] = "Your password has been reset";

                // Redirect to the Login action
                return RedirectToAction("Login");

            }

            foreach (var error in result.Errors)
            {
                if (error.Code == "PasswordTooShort")
                {
                    ModelState.AddModelError("PasswordTooShort", error.Description);
                }
                else if (error.Code == "PasswordRequiresLower")
                {
                    ModelState.AddModelError("PasswordRequiresLower", error.Description);
                }
                else
                {
                    // Thêm lỗi vào ModelState để hiển thị cho người dùng.
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View(model);
        }



        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


    }
}
