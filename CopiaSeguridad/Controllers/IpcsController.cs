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
    public class IpcsController : Controller
    {
        private readonly SistPresupuestosContext _context;

        public IpcsController(SistPresupuestosContext context)
        {
            _context = context;
        }

        // GET: Ipcs
        public async Task<IActionResult> Index()
        {
              return _context.Ipcs != null ? 
                          View(await _context.Ipcs.ToListAsync()) :
                          Problem("Entity set 'SistPresupuestosContext.Ipcs'  is null.");
        }

        // GET: Ipcs/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Ipcs == null)
            {
                return NotFound();
            }

            var ipc = await _context.Ipcs
                .FirstOrDefaultAsync(m => m.Anio == id);
            if (ipc == null)
            {
                return NotFound();
            }

            return View(ipc);
        }

        // GET: Ipcs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ipcs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Anio,Valor")] Ipc ipc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ipc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ipc);
        }

        // GET: Ipcs/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Ipcs == null)
            {
                return NotFound();
            }

            var ipc = await _context.Ipcs.FindAsync(id);
            if (ipc == null)
            {
                return NotFound();
            }
            return View(ipc);
        }

        // POST: Ipcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("Anio,Valor")] Ipc ipc)
        {
            if (id != ipc.Anio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ipc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IpcExists(ipc.Anio))
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
            return View(ipc);
        }

        // GET: Ipcs/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Ipcs == null)
            {
                return NotFound();
            }

            var ipc = await _context.Ipcs
                .FirstOrDefaultAsync(m => m.Anio == id);
            if (ipc == null)
            {
                return NotFound();
            }

            return View(ipc);
        }

        // POST: Ipcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Ipcs == null)
            {
                return Problem("Entity set 'SistPresupuestosContext.Ipcs'  is null.");
            }
            var ipc = await _context.Ipcs.FindAsync(id);
            if (ipc != null)
            {
                _context.Ipcs.Remove(ipc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IpcExists(short id)
        {
          return (_context.Ipcs?.Any(e => e.Anio == id)).GetValueOrDefault();
        }
    }
}
