using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using PayPal.Api;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;
using Project3.Services;
using Serilog;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Session;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Http;
using Project3.Helpers;


namespace Project3.Controllers
{
    public class CompanyBillingPolicyController : Controller
    {
        private readonly CarService _carService;
        private readonly VehicleInsuranceManagementContext _context;
        private readonly BillingCalculationService _billingCalculationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EstimatesController> _logger;
        public CompanyBillingPolicyController(ILogger<EstimatesController> logger, VehicleInsuranceManagementContext context, BillingCalculationService billingCalculationService, UserManager<ApplicationUser> userManager, CarService carService)
        {
            _context = context;
            _billingCalculationService = billingCalculationService;
            _userManager = userManager;
            _carService = carService;
            _logger = logger;
        }
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanyBillingPolicies.ToListAsync());
        }
        [Route("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingPolicy = await _context.CompanyBillingPolicies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billingPolicy == null)
            {
                return NotFound();
            }

            return View(billingPolicy);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {


            //get user info
            //var user = await _userManager.GetUserAsync(User);

            var sessionData = HttpContext.Session.GetObject<InsuranceProcessViewModel>("InsuranceData");
            var sessionData1 = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            var sessionData2 = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            var collectInfoData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");
            if (sessionData == null || sessionData1 == null || sessionData2 == null || collectInfoData == null)
            {
                return RedirectToAction("Index", "Home"); // or any appropriate action
            }

            // Calculate the amount using the billing calculation service
            float amount = _billingCalculationService.CalculateAmount(collectInfoData);

            // Parse the PolicyDate string to DateTime
            DateTime? policyDate = null;
            if (DateTime.TryParse(sessionData.PolicyDate, out DateTime parsedDate))
            {
                policyDate = parsedDate;
            }

            var model = new CompanyBillingPolicyViewModel
            {
                CustomerId = sessionData.CustomerId,
                CustomerName = sessionData.CustomerName,
                CustomerPhoneNumber = sessionData.CustomerPhoneNumber,
                CustomerAddProve = collectInfoData.CustomerAdd,
                PolicyNumber = sessionData.PolicyNumber,

                VehicleName = sessionData.VehicleName,
                VehicleModel = sessionData.VehicleModel,
                VehicleRate = sessionData.VehicleRate,
                VehicleBodyNumber = sessionData.VehicleBodyNumber,
                VehicleEngineNumber = sessionData.VehicleEngineNumber,
                Date = policyDate,
                Amount = amount // Set the calculated amount


            };

            return View(model);
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyBillingPolicyViewModel model)
        {
            Random random = new Random();
            string randomNumber = random.Next(10000000, 99999999).ToString();
            var user = await _userManager.GetUserAsync(User);
            var collectInfoData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");
            var sessionData = HttpContext.Session.GetObject<InsuranceProcessViewModel>("InsuranceData");

            DateTime? policyDate = null;
            if (DateTime.TryParse(sessionData.PolicyDate, out DateTime parsedDate))
            {
                policyDate = parsedDate;
            }

            var amount = _billingCalculationService.CalculateAmount(collectInfoData);

            if (ModelState.IsValid)
            {
                var companyBillingPolicy = new CompanyBillingPolicy
                {
                    CustomerId = user.Id,
                    CustomerName = model.CustomerName,
                    CustomerPhoneNumber = model.CustomerPhoneNumber,
                    CustomerAddProve = collectInfoData.CustomerAdd,
                    PolicyNumber = model.PolicyNumber,
                    BillNo = randomNumber,
                    VehicleName = model.VehicleName,
                    VehicleModel = model.VehicleModel,
                    VehicleRate = model.VehicleRate,
                    VehicleBodyNumber = model.VehicleBodyNumber,
                    VehicleEngineNumber = model.VehicleEngineNumber,
                    Date = policyDate,
                    Amount = amount
                };

                HttpContext.Session.SetObject("companyBilling", companyBillingPolicy);
                HttpContext.Session.SetString("Amount", amount.ToString(CultureInfo.InvariantCulture));
                
                return RedirectToAction("Payment");

                //var companysession = HttpContext.Session.GetObject<CompanyBillingPolicy>("companyBilling");
                //var amountdata = HttpContext.Session.GetObject<CompanyBillingPolicy>("Amount");
                //_logger.LogInformation("Session Insurance Process: {@SessionData}", sessionData);
                //_logger.LogInformation("Session Insurance Process: {@SessionData}", sessionData);
            }
            return View(model);
            
        }

        public IActionResult Payment()
        {
            var companysession = HttpContext.Session.GetObject<CompanyBillingPolicy>("companyBilling");

            _logger.LogInformation("Session Insurance Process: {@SessionData}", companysession);

            return View();
        }

        //     3. **Edit Action(GET)**:
        //- Fetches the billing policy by ID.
        //- Prepares the view model with all necessary data, including the fields for recalculating the amount.


        //[Route("edit")]
        //public async Task<IActionResult> Edit(int? id, CollectInfoViewModel viewCollectInfo)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var sessionData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");

        //    var cars = await _carService.GetAllCarsAsync();
        //    ViewBag.Manufacturers = cars.Select(c => new { c.Code, c.Name }).ToList();

        //    var viewModel = new CompanyBillingPolicyViewModel
        //    {
        //        VehicleName = sessionData.VehicleName,
        //        VehicleModel = sessionData.VehicleModel,
        //        VehicleVersion = sessionData.VehicleVersion,
        //        VehicleRate = sessionData.VehicleRate,
        //        VehicleBodyNumber = sessionData.VehicleBodyNumber,
        //        VehicleEngineNumber = sessionData.VehicleEngineNumber,
        //        DriverAge = sessionData.DriverAge,
        //        DriverGender = sessionData.DriverGender,
        //        DrivingHistory = sessionData.DrivingHistory,
        //        CustomerAdd = sessionData.CustomerAdd,
        //        Usage = sessionData.Usage,
        //        AntiTheftDevice = sessionData.AntiTheftDevice,
        //        MultiPolicy = sessionData.MultiPolicy,
        //        SafeDriver = sessionData.SafeDriver,
        //    };


        //    return View(viewModel);
        //}
        //[Route("edit")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, CompanyBillingPolicyViewModel model)
        //{
        //    var sessionData = HttpContext.Session.GetObject<CollectInfoViewModel>("AmountCalData");
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var insurances = new InsuranceProcess
        //            {

        //                VehicleId = insuranceProcess.VehicleId,
        //                VehicleName = insuranceProcess.VehicleName,
        //                VehicleModel = insuranceProcess.VehicleModel,
        //                VehicleVersion = insuranceProcess.VehicleVersion,
        //                VehicleRate = insuranceProcess.VehicleRate,
        //                VehicleBodyNumber = insuranceProcess.VehicleBodyNumber,
        //                VehicleEngineNumber = insuranceProcess.VehicleEngineNumber
        //            };

        //            HttpContext.Session.SetObject("InsuranceData", insurances);

        //            // Recalculate the amount
        //            var collectInfoData = new CompanyBillingPolicy
        //            {
        //                VehicleRate = model.VehicleRate,
        //                Amount = _billingCalculationService.CalculateAmount();
        //            };


        //            _context.Update(collectInfoData);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BillingPolicyExists(model.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}
        private bool BillingPolicyExists(int id)
        {
            return _context.CompanyBillingPolicies.Any(e => e.Id == id);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingPolicy = await _context.CompanyBillingPolicies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billingPolicy == null)
            {
                return NotFound();
            }

            return View(billingPolicy);
        }
        [Route("delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billingPolicy = await _context.CompanyBillingPolicies.FindAsync(id);
            _context.CompanyBillingPolicies.Remove(billingPolicy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult PaymentFail()
        {
            return View();
        }
        public IActionResult PaymentSuccess()
        {
            return View();
        }


        private static readonly HttpClient client = new HttpClient();

        private async Task<string> ExecPostRequest(string url, string data)
        {
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        [HttpPost]
        public async Task<IActionResult> MomoPayment()
        {
            var companysession = HttpContext.Session.GetObject<CompanyBillingPolicy>("companyBilling");
            double amount1 = companysession.Amount;


            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMOBKUN20180529";
            string accessKey = "klm05TvNBzhg7h7j";
            string secretKey = "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa";
            string orderInfo = "Pay via MoMo";
            //string amount = ;
            string orderId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string redirectUrl = Url.Action("PaymentSuccess", "CompanyBillingPolicy", null, Request.Scheme);
            string ipnUrl = Url.Action("PaymentFail", "CompanyBillingPolicy", null, Request.Scheme);
            string extraData = "";
            string requestId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string requestType = "payWithATM"; //captureWallet

            string rawHash = $"accessKey={accessKey}&amount={amount1.ToString("F0")}&extraData={extraData}&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType={requestType}";
            string signature;

            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
                signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            var data = new
            {
                partnerCode,
                partnerName = "Test",
                storeId = "MomoTestStore",
                requestId,
                amount = amount1.ToString("F0"),
                orderId,
                orderInfo,
                redirectUrl,
                ipnUrl,
                lang = "vi",
                extraData,
                requestType,
                signature
            };

            string jsonData = JsonConvert.SerializeObject(data);
            string result = await ExecPostRequest(endpoint, jsonData);
            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            // Log the entire response to check for keys and values
            foreach (var key in jsonResult.Keys)
            {
                Log.Information($"{key}: {jsonResult[key]}");
            }

            // Check if 'payUrl' is present in the response
            if (jsonResult.ContainsKey("payUrl"))
            {
                var companyBillingPolicy = HttpContext.Session.GetObject<CompanyBillingPolicy>("companyBilling");
                if (companyBillingPolicy != null)
                {
                    companyBillingPolicy.PaymentStatus = "Paid";
                    _context.CompanyBillingPolicies.Update(companyBillingPolicy);
                    _context.SaveChangesAsync();
                }
                return Redirect(jsonResult["payUrl"]);
                
            }
            else
            {
                // Log an error message if 'payUrl' is not found
                Log.Error("payUrl key not found in MoMo API response.");
                // Handle the error, for example by displaying a message to the user
                TempData["error"] = "Unable to process payment. Please try again.";
                return RedirectToAction("PaymentFail");
            }
            
        }

        [HttpGet]
        public IActionResult PaymentConfirm()
        {
            // Handle the payment confirmation
            return View();
        }

        [HttpPost]
        public IActionResult PaymentNotify()
        {
            // Handle the IPN (Instant Payment Notification)
            return Ok();
        }
        


        [HttpGet]
        public IActionResult PaymentWithPayPal()
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();

            try
            {
                string payerId = Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/CompanyBillingPolicy/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, $"{baseURI}guid={guid}");

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    HttpContext.Session.SetString("PaymentID", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var paymentId = HttpContext.Session.GetString("PaymentID");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("PaymentFail");
                    }
                    // If payment is successful, update the status of the billing policy
                    var companyBillingPolicy = HttpContext.Session.GetObject<CompanyBillingPolicy>("companyBilling");
                    if (companyBillingPolicy != null)
                    {
                        companyBillingPolicy.PaymentStatus = "Paid";
                        _context.CompanyBillingPolicies.Update(companyBillingPolicy);
                        _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return View("PaymentFail");
            }

            return View("PaymentSuccess");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList() { items = new List<Item>() };

            string amountString = HttpContext.Session.GetString("Amount");
            var amountValue = decimal.Parse(amountString);

            itemList.items.Add(new Item()
            {
                name = "Insurance Payment",
                currency = "USD",
                price = amountValue.ToString(CultureInfo.InvariantCulture),
                quantity = "1",
                sku = "insurance_payment"
            });

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = amountValue.ToString(CultureInfo.InvariantCulture)
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = amountValue.ToString(CultureInfo.InvariantCulture),
                details = details
            };

            var transactionList = new List<Transaction>
    {
        new Transaction()
        {
            description = "Insurance Payment",
            invoice_number = DateTime.Now.Ticks.ToString(),
            amount = amount,
            item_list = itemList
        }
    };

            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
        }
    }
}
