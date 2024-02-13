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
        private readonly ILogger<FacturasController> _logger;

        public FacturasController(SistPresupuestosContext context, ILogger<FacturasController> logger)
        {
            _context = context;
            _logger = logger;
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
            ViewBag.Factura = primerItemF;

            var primerItemCF = await _context.ControlFacturas.FirstOrDefaultAsync(cf => cf.CodFactura == primerItemF.CodFactura);
            ViewBag.Control = primerItemCF;

            var primerItemP = await _context.Presupuestos.FirstOrDefaultAsync(p => p.CodPresupuesto == primerItemCF.CodPresupuesto);
            ViewBag.Presupuestos = primerItemP;

            return View(await sistPresupuestosContext.ToListAsync());
        }

        public async Task<IActionResult> ObtenerFacturas(Int16 codItem)
        {
            var facturas = await _context.Facturas
                .Where(f => f.ControlFacturas.Any(cf => cf.CodPresupuestoNavigation.CodItem == codItem))
                .Select(f => f.NumFactura)
                .ToListAsync();

            return Json(facturas);
        }



        // GET: Lista de Facturas
        public async Task<IActionResult> ListaFacturas()
        {
            var sistPresupuestosContext = _context.Facturas
                .Include(i => i.TipoCambio)
                .Include(f => f.ControlFacturas);

            ViewBag.Presupuestos = await _context.Presupuestos.ToListAsync();
            ViewBag.Items = await _context.Items.ToListAsync();

            ViewBag.Conceptos = await _context.Conceptos.ToListAsync();

            return View(await sistPresupuestosContext.ToListAsync());
        }

        // GET: Facturas/Details/5
        public async Task<IActionResult> Details(int id)
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
            //Se busca el último CodControl
            var ultimoCodControl = _context.ControlFacturas.OrderByDescending(cf => cf.CodControl).FirstOrDefault();
            if (ultimoCodControl != null) {

                //Se transforma a int para sumarle uno, luego a string y se le agregan los 0 necesarios para dejarlo de 6 cifras
                int nuevoCodControl = ultimoCodControl.CodControl +1;

                if (ModelState.IsValid)
                {
                    var control = new ControlFactura()
                    {
                       CodControl = nuevoCodControl,


                    };

                    _context.Add(factura);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            
            ViewData["MesContable"] = new SelectList(_context.TipoCambios, "Mes", "Mes", factura.MesContable);
            return View(factura);
        }

        // GET: Facturas/Edit/5
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(int id, [Bind("CodFactura,NumFactura,Monto,MesContable,AnioContable,Empresa")] Factura factura)
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
        public async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        private bool FacturaExists(int id)
        {
          return (_context.Facturas?.Any(e => e.CodFactura == id)).GetValueOrDefault();
        }
    }
}
