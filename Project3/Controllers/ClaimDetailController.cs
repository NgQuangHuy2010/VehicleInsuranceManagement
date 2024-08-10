using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Resources;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;
using System.Data.Entity;
using static System.Net.WebRequestMethods;

namespace Project3.Controllers
{
    public class ClaimDetailController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VehicleInsuranceManagementContext _context;


        public ClaimDetailController(UserManager<ApplicationUser> userManager, VehicleInsuranceManagementContext context)
        {
            _userManager = userManager;
            _context = context;
        }

       
        public async Task<IActionResult>  Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user ==null)
            {
                return NotFound("User not found");
            }
            var billingInfo = _context.CompanyBillingPolicies
           .Where(b => b.CustomerId == user.Id)
           .Select(b => new ClaimDetailViewModel
           {
               PolicyNumber = b.PolicyNumber,

           }).ToList()
           .FirstOrDefault();
            if (billingInfo == null)
            {
                return NotFound("Billing information not found");
            }
            return View(billingInfo);

            
        }
        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("PolicyNumber is required.");
            }

            // Truy vấn bảng CompanyBillingPolicies để tìm PolicyNumber tương ứng với id
            var billingInfo = _context.CompanyBillingPolicies
                .Where(b => b.PolicyNumber == id)
                .FirstOrDefault();

            if (billingInfo == null)
            {
                return NotFound("No billing information found for the provided PolicyNumber.");
            }

            // Trả về view với dữ liệu của billingInfo
            return View(billingInfo);
        }

    }

}



