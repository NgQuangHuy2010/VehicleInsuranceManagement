using Microsoft.AspNetCore.Mvc;
using Project3.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Controllers
{
    public class PaymentController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public PaymentController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }

        // Display payment form
        [HttpGet]
        public IActionResult Create(int billingPolicyId)
        {
            var billingPolicy = _context.CompanyBillingPolicies.Find(billingPolicyId);
            if (billingPolicy == null)
            {
                return NotFound();
            }

            var model = new Payment
            {
                BillingPolicyId = billingPolicyId,
                Amount = billingPolicy.Amount
            };

            return View(model);
        }

        // Process payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.PaymentDate = DateTime.Now;
                payment.IsConfirmed = false; // Admin will confirm later

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "CompanyBillingPolicy");
            }

            return View(payment);
        }

        // Admin confirms payment
        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.IsConfirmed = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "CompanyBillingPolicy");
        }
    }
}
