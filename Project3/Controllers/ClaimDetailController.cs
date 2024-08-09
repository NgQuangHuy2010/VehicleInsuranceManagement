using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;

namespace Project3.Controllers
{
    public class ClaimDetailController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public ClaimDetailController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }

        // GET: ClaimDetail
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClaimDetails.ToListAsync());
        }
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,ClaimNumber,PolicyNumber,PolicyStartDate,PolicyEndDate,CustomerName,PlaceOfAccident,DateOfAccident,InsuredAmount,ClaimableAmount")] ClaimDetail claimDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(claimDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(claimDetail);
        }

        

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claimDetail = await _context.ClaimDetails.FindAsync(id);
            if (claimDetail != null)
            {
                _context.ClaimDetails.Remove(claimDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaimDetailExists(int id)
        {
            return _context.ClaimDetails.Any(e => e.Id == id);
        }
    }
}
