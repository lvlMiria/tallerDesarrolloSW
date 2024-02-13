using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    public class ControlFacturasController : Controller
    {
        private readonly SistPresupuestosContext _context;

        public ControlFacturasController(SistPresupuestosContext context)
        {
            _context = context;
        }

        // GET: ControlFacturas
        public async Task<IActionResult> Index()
        {
            var sistPresupuestosContext = _context.ControlFacturas
                .Include(f => f.CodPresupuesto);
            return View(await sistPresupuestosContext.ToListAsync());
        }

        // GET: ControlFacturas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ControlFacturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodControl,CodPresupuesto,CodFactura,Origen,FechaRecepcion,FechaEntrega,Comentario")] ControlFactura controlFactura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controlFactura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(controlFactura);
        }

        // GET: Ipcs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ControlFacturas == null)
            {
                return NotFound();
            }

            var controlFactura = await _context.ControlFacturas.FindAsync(id);
            if (controlFactura == null)
            {
                return NotFound();
            }
            return View(controlFactura);
        }

        // POST: Ipcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodControl,CodPresupuesto,CodFactura,Origen,FechaRecepcion,FechaEntrega,Comentario")] ControlFactura controlFactura)
        {
            if (id != controlFactura.CodControl)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controlFactura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControlFacturaExists(controlFactura.CodControl))
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
            return View(controlFactura);
        }

        // GET: ControlFactura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ControlFacturas == null)
            {
                return NotFound();
            }

            var controlFactura = await _context.ControlFacturas
                .FirstOrDefaultAsync(m => m.CodControl == id);
            if (controlFactura == null)
            {
                return NotFound();
            }

            return View(controlFactura);
        }

        // POST: ControlFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ControlFacturas == null)
            {
                return Problem("Entity set 'SistPresupuestosContext.ControlFacturas'  is null.");
            }
            var controlFacturas = await _context.ControlFacturas.FindAsync(id);
            if (controlFacturas != null)
            {
                _context.ControlFacturas.Remove(controlFacturas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControlFacturaExists(int id)
        {
          return (_context.ControlFacturas?.Any(e => e.CodControl == id)).GetValueOrDefault();
        }
    }
}
