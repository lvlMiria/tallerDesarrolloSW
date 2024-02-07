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
    public class ItemsController : Controller
    {
        private readonly SistPresupuestosContext _context;
        private readonly ILogger<IpcsController> _logger;

        public ItemsController(SistPresupuestosContext context, ILogger<IpcsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        { 
            //Primer item de la tabla
            ViewBag.PrimerItem = _context.Items.FirstOrDefault();
            //Obtener conceptos:
            ViewBag.Conceptos = await _context.Conceptos.ToListAsync();

            var sistPresupuestosContext = _context.Items.Include(i => i.CodConceptoNavigation);
            return View(await sistPresupuestosContext.ToListAsync());
        }

        //Para moverse entre registros en index Items (flechas)
        public IActionResult RegistroAnterior(string codActual)
        {
            var registroAnterior = _context.Items.OrderByDescending(i=>i.CodItem)
                .FirstOrDefault(i=> int.Parse(i.CodItem) < int.Parse(codActual));

            return Json(registroAnterior);
        }

        public IActionResult RegistroSiguiente(string codActual)
        {
            var registroSiguiente = _context.Items.OrderBy(i => i.CodItem)
                .FirstOrDefault(i => int.Parse(i.CodItem) > int.Parse(codActual));
            _logger.LogInformation("============="+registroSiguiente);

            return Json(registroSiguiente);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.CodConceptoNavigation)
                .FirstOrDefaultAsync(m => m.CodItem == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public async Task<IActionResult> Create()
        {
            List<Concepto> conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;
            //ViewData["CodConcepto"] = new SelectList(_context.Conceptos, "CodConcepto", "CodConcepto");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodItem,DescItem,GastoInversion,ContNuevo,CodConcepto,Estado")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodConcepto"] = new SelectList(_context.Conceptos, "CodConcepto", "CodConcepto", item.CodConcepto);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CodConcepto"] = new SelectList(_context.Conceptos, "CodConcepto", "CodConcepto", item.CodConcepto);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CodItem,DescItem,GastoInversion,ContNuevo,CodConcepto,Estado")] Item item)
        {
            if (id != item.CodItem)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.CodItem))
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
            ViewData["CodConcepto"] = new SelectList(_context.Conceptos, "CodConcepto", "CodConcepto", item.CodConcepto);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.CodConceptoNavigation)
                .FirstOrDefaultAsync(m => m.CodItem == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'SistPresupuestosContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(string id)
        {
          return (_context.Items?.Any(e => e.CodItem == id)).GetValueOrDefault();
        }

    }
}
