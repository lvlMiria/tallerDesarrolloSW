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
    public class TipoCambiosController : Controller
    {
        private readonly SistPresupuestosContext _context;

        public TipoCambiosController(SistPresupuestosContext context)
        {
            _context = context;
        }

        // GET: TipoCambios
        public async Task<IActionResult> Index()
        {
              return _context.TipoCambios != null ? 
                          View(await _context.TipoCambios.ToListAsync()) :
                          Problem("Entity set 'SistPresupuestosContext.TipoCambios'  is null.");
        }

        // GET: TipoCambios/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.TipoCambios == null)
            {
                return NotFound();
            }

            var tipoCambio = await _context.TipoCambios
                .FirstOrDefaultAsync(m => m.Mes == id);
            if (tipoCambio == null)
            {
                return NotFound();
            }

            return View(tipoCambio);
        }

        // GET: TipoCambios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoCambios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mes,Anio,Valor")] TipoCambio tipoCambio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoCambio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoCambio);
        }

        // GET: TipoCambios/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.TipoCambios == null)
            {
                return NotFound();
            }

            var tipoCambio = await _context.TipoCambios.FindAsync(id);
            if (tipoCambio == null)
            {
                return NotFound();
            }
            return View(tipoCambio);
        }

        // POST: TipoCambios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("Mes,Anio,Valor")] TipoCambio tipoCambio)
        {
            if (id != tipoCambio.Mes)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoCambio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoCambioExists(tipoCambio.Mes))
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
            return View(tipoCambio);
        }

        // GET: TipoCambios/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.TipoCambios == null)
            {
                return NotFound();
            }

            var tipoCambio = await _context.TipoCambios
                .FirstOrDefaultAsync(m => m.Mes == id);
            if (tipoCambio == null)
            {
                return NotFound();
            }

            return View(tipoCambio);
        }

        // POST: TipoCambios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.TipoCambios == null)
            {
                return Problem("Entity set 'SistPresupuestosContext.TipoCambios'  is null.");
            }
            var tipoCambio = await _context.TipoCambios.FindAsync(id);
            if (tipoCambio != null)
            {
                _context.TipoCambios.Remove(tipoCambio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoCambioExists(byte id)
        {
          return (_context.TipoCambios?.Any(e => e.Mes == id)).GetValueOrDefault();
        }
    }
}
