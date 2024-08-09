using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;

namespace Project3.Areas.System.Controllers
{
    [Area("System")]
    public class VehicleWarrantiesController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public VehicleWarrantiesController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }

        // GET: System/VehicleWarranties
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleWarranties.ToListAsync());
        }

        // GET: System/VehicleWarranties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleWarranty = await _context.VehicleWarranties
                .FirstOrDefaultAsync(m => m.WarrantyId == id);
            if (vehicleWarranty == null)
            {
                return NotFound();
            }

            return View(vehicleWarranty);
        }

        // GET: System/VehicleWarranties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: System/VehicleWarranties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarrantyId,WarrantyType,WarrantyDuration,WarrantyDetails")] VehicleWarranty vehicleWarranty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleWarranty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleWarranty);
        }

        // GET: System/VehicleWarranties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleWarranty = await _context.VehicleWarranties.FindAsync(id);
            if (vehicleWarranty == null)
            {
                return NotFound();
            }
            return View(vehicleWarranty);
        }

        // POST: System/VehicleWarranties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WarrantyId,WarrantyType,WarrantyDuration,WarrantyDetails")] VehicleWarranty vehicleWarranty)
        {
            if (id != vehicleWarranty.WarrantyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleWarranty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleWarrantyExists(vehicleWarranty.WarrantyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleWarranty);
        }

        // GET: System/VehicleWarranties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleWarranty = await _context.VehicleWarranties
                .FirstOrDefaultAsync(m => m.WarrantyId == id);
            if (vehicleWarranty == null)
            {
                return NotFound();
            }

            return View(vehicleWarranty);
        }

        // POST: System/VehicleWarranties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleWarranty = await _context.VehicleWarranties.FindAsync(id);
            if (vehicleWarranty != null)
            {
                _context.VehicleWarranties.Remove(vehicleWarranty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleWarrantyExists(int id)
        {
            return _context.VehicleWarranties.Any(e => e.WarrantyId == id);
        }
    }
}
