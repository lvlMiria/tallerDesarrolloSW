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
    public class FacturasController : Controller
    {
        private readonly SistPresupuestosContext _context;

        public FacturasController(SistPresupuestosContext context)
        {
            _context = context;
        }

        // GET: Facturas
        public async Task<IActionResult> Index()
        {
            var sistPresupuestosContext = _context.Facturas
                .Include(f => f.TipoCambio);
            
            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            //Primer item de la tabla
            var primerItemF = _context.Facturas.FirstOrDefault();
            ViewBag.PrimerItemF = primerItemF;

            var primerItemCF = await _context.ControlFacturas.FirstOrDefaultAsync(cf => cf.CodFactura == primerItemF.CodFactura);
            ViewBag.PrimerItemCF = primerItemCF;

            var primerItemP = await _context.Presupuestos.FirstOrDefaultAsync(p => p.CodPresupuesto == primerItemCF.CodPresupuesto);
            ViewBag.PrimerItemP = primerItemP;

            return View(await sistPresupuestosContext.ToListAsync());
        }

        // GET: Lista de Facturas
        public async Task<IActionResult> ListaFacturas()
        {
            var sistPresupuestosContext = _context.Facturas
                .Include(i => i.TipoCambio)
                .Include(f => f.ControlFacturas);
            return View(await sistPresupuestosContext.ToListAsync());
        }

        // GET: Facturas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.TipoCambio)
                .FirstOrDefaultAsync(m => m.CodFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // GET: Facturas/Create
        public IActionResult Create()
        {
            var items = _context.Items.ToList();
            ViewBag.Items = items;

            ViewData["MesContable"] = new SelectList(_context.TipoCambios, "Mes", "Mes");
            return View();
        }

        // POST: Facturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodFactura,NumFactura,Monto,MesContable,AnioContable,Empresa")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(factura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesContable"] = new SelectList(_context.TipoCambios, "Mes", "Mes", factura.MesContable);
            return View(factura);
        }

        // GET: Facturas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            ViewData["MesContable"] = new SelectList(_context.TipoCambios, "Mes", "Mes", factura.MesContable);
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CodFactura,NumFactura,Monto,MesContable,AnioContable,Empresa")] Factura factura)
        {
            if (id != factura.CodFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaExists(factura.CodFactura))
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
            ViewData["MesContable"] = new SelectList(_context.TipoCambios, "Mes", "Mes", factura.MesContable);
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.TipoCambio)
                .FirstOrDefaultAsync(m => m.CodFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Facturas == null)
            {
                return Problem("Entity set 'SistPresupuestosContext.Facturas'  is null.");
            }
            var factura = await _context.Facturas.FindAsync(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaExists(string id)
        {
          return (_context.Facturas?.Any(e => e.CodFactura == id)).GetValueOrDefault();
        }
    }
}
