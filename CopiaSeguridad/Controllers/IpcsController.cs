using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    public class IpcsController : Controller
    {
        private readonly SistPresupuestosContext _context;
        private readonly ILogger<IpcsController> _logger;
        public IpcsController(SistPresupuestosContext context, ILogger<IpcsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Ipcs
        public async Task<IActionResult> Index()
        {
            ViewBag.Error = "";

            //Primer item de la tabla
            ViewBag.Mostrar = _context.Ipcs
                .OrderByDescending(tc => tc.Anio)
                .FirstOrDefault();

            return _context.Ipcs != null ? 
                          View(await _context.Ipcs.OrderByDescending(tc => tc.Anio).ToListAsync()) :
                          Problem("Entity set 'SistPresupuestosContext.Ipcs'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerValor(string txtAnio)
        {
            if (!txtAnio.IsNullOrEmpty())
            {
                Int16 anio = Int16.Parse(txtAnio);
                if (_context.Ipcs.Where(i => i.Anio == anio).FirstOrDefault() != null)
                {
                    ViewBag.Error = "";
                    ViewBag.Mostrar = _context.Ipcs.Where(i => i.Anio == anio).FirstOrDefault();
                    return View("Index");
                }
                ViewBag.Mostrar = _context.Ipcs
                .OrderByDescending(tc => tc.Anio)
                .FirstOrDefault();

                ViewBag.Error = "Año no ingresado en la base de datos.";
                return View("Index");
            }
            ViewBag.Mostrar = _context.Ipcs
                .OrderByDescending(tc => tc.Anio)
                .FirstOrDefault();

            ViewBag.Error = "Ingrese un año por favor.";
            return View("Index");

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
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Anio,Valor")] Ipc ipc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(ipc);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }catch (DbUpdateException ex)
            {
                ViewBag.Mensaje = "El año ya se encuentra en la base de datos.";
                return View(ipc);
            }
            ViewBag.Mensaje = "Error al ingresar los datos";
            return View(ipc);
        }

        [HttpGet]
        public ActionResult RevisarAnio(short anio)
        {
            var ipc = _context.Ipcs.Where(i=>i.Anio == anio).FirstOrDefault();
            if (ipc != null)
            {
                return Json(ipc);
            }
            else
            {
                return Json(null);
            }
        }

        // GET: Ipcs/Edit/5
        //public async Task<IActionResult> Edit(short? id)
        //{
        //    if (id == null || _context.Ipcs == null)
        //    {
        //        return NotFound();
        //    }

        //    var ipc = await _context.Ipcs.FindAsync(id);
        //    if (ipc == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ipc);
        //}

        // POST: Ipcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(short id, [Bind("Anio,Valor")] Ipc ipc)
        {

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
