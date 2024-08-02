using Microsoft.AspNetCore.Identity;
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
                    //    // return về trang admin 
                    //    return RedirectToAction("Index", "Dashboard", new { area = "System" });
                    //}
                    //else if (!await _userManager.IsInRoleAsync(user, "User"))
                    //{
                    //    return RedirectToAction("Index", "Dashboard", new { area = "System" });


                    //}
                    //else
                    //{
                    //    //nếu ko phải admin về home cho user
                    //    return RedirectToAction("Index", "Home");
                    //}
                }
                //check fail email and pass
                ModelState.AddModelError("Email", "Incorrect email or password!!!");
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
                    ModelState.AddModelError("Email", "E-mail is being used!!");
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
                    //    // Tạo vai trò "User" nếu chưa tồn tại.
                    //    await _roleManager.CreateAsync(new IdentityRole("User"));
                    //}
                    //// Gán vai trò "User" cho người dùng mới.
                    //await _userManager.AddToRoleAsync(user, "User");

                    // Tạo một token xác nhận email cho người dùng mới.
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // Tạo liên kết xác nhận email chứa token và userId.qua action ConfirmEmail
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    // Gửi email xác nhận với liên kết xác nhận bằng SendEmailAsync email.cs trong model
                    await _emailService.SendEmailAsync(register.Email, "Confirm email",
                        $"Please confirm your account by clicking this link: {confirmationLink}");

                    // Chuyển hướng đến trang thông báo đã gửi email.
                    return RedirectToAction("ConfirmEmailSent");
                }

                // Xử lý các lỗi trả về từ quá trình đăng ký không thành công.
                foreach (var error in result.Errors)
                {
                    // Thêm lỗi vào ModelState để hiển thị cho người dùng.
                    ModelState.AddModelError(string.Empty, error.Description);
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
                return View("ConfirmEmailSuccess");
            }

            return View("Error");
        }

        [Route("confirm-email-sent")]
        public IActionResult ConfirmEmailSent()
        {
            // Trả về view thông báo rằng email xác nhận đã được gửi thành công.
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		//[Route("login")]
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Login()
		//{

		//}
	}
}
