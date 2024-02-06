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
    public class PresupuestoesController : Controller
    {
        private readonly SistPresupuestosContext _context;

        public PresupuestoesController(SistPresupuestosContext context)
        {
            _context = context;
        }

        // GET: Presupuestoes
        public async Task<IActionResult> Index()
        {
            var sistPresupuestosContext = _context.Presupuestos.Include(p => p.AnioNavigation).Include(p => p.CodItemNavigation);
            List<Item> items = await _context.Items.ToListAsync();
            ViewBag.Items = items;
            return View(await sistPresupuestosContext.ToListAsync());
        }

        // GET: Lista de Presupuestos
        public async Task<IActionResult> ListaPresupuestos()
        {
            var sistPresupuestosContext = _context.Presupuestos.Include(p => p.AnioNavigation).Include(p => p.CodItemNavigation);
            List<Item> items = await _context.Items.ToListAsync();
            ViewBag.Items = items;
            List<Concepto> conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;
            return View(await sistPresupuestosContext.ToListAsync());
        }

        // GET: Presupuestoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Presupuestos == null)
            {
                return NotFound();
            }

            var presupuesto = await _context.Presupuestos
                .Include(p => p.AnioNavigation)
                .Include(p => p.CodItemNavigation)
                .FirstOrDefaultAsync(m => m.CodPresupuesto == id);
            if (presupuesto == null)
            {
                return NotFound();
            }

            return View(presupuesto);
        }

        // GET: Presupuestoes/Create
        public IActionResult Create()
        {
            ViewData["Anio"] = new SelectList(_context.Ipcs, "Anio", "Anio");
            ViewData["CodItem"] = new SelectList(_context.Items, "CodItem", "CodItem");
            return View();
        }

        // POST: Presupuestoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodPresupuesto,Mes,Anio,PresupuestoMes,CodItem")] Presupuesto presupuesto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(presupuesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Anio"] = new SelectList(_context.Ipcs, "Anio", "Anio", presupuesto.Anio);
            ViewData["CodItem"] = new SelectList(_context.Items, "CodItem", "CodItem", presupuesto.CodItem);
            return View(presupuesto);
        }

        // GET: Presupuestoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Presupuestos == null)
            {
                return NotFound();
            }

            var presupuesto = await _context.Presupuestos.FindAsync(id);
            if (presupuesto == null)
            {
                return NotFound();
            }
            ViewData["Anio"] = new SelectList(_context.Ipcs, "Anio", "Anio", presupuesto.Anio);
            ViewData["CodItem"] = new SelectList(_context.Items, "CodItem", "CodItem", presupuesto.CodItem);
            return View(presupuesto);
        }

        // POST: Presupuestoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CodPresupuesto,Mes,Anio,PresupuestoMes,CodItem")] Presupuesto presupuesto)
        {
            if (id != presupuesto.CodPresupuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presupuesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresupuestoExists(presupuesto.CodPresupuesto))
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
            ViewData["Anio"] = new SelectList(_context.Ipcs, "Anio", "Anio", presupuesto.Anio);
            ViewData["CodItem"] = new SelectList(_context.Items, "CodItem", "CodItem", presupuesto.CodItem);
            return View(presupuesto);
        }

        // GET: Presupuestoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Presupuestos == null)
            {
                return NotFound();
            }

            var presupuesto = await _context.Presupuestos
                .Include(p => p.AnioNavigation)
                .Include(p => p.CodItemNavigation)
                .FirstOrDefaultAsync(m => m.CodPresupuesto == id);
            if (presupuesto == null)
            {
                return NotFound();
            }

            return View(presupuesto);
        }

        // POST: Presupuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Presupuestos == null)
            {
                return Problem("Entity set 'SistPresupuestosContext.Presupuestos'  is null.");
            }
            var presupuesto = await _context.Presupuestos.FindAsync(id);
            if (presupuesto != null)
            {
                _context.Presupuestos.Remove(presupuesto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresupuestoExists(string id)
        {
          return (_context.Presupuestos?.Any(e => e.CodPresupuesto == id)).GetValueOrDefault();
        }

      
    }
}
