using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    public class TipoCambiosController : Controller
    {
        private readonly SistPresupuestosContext _context;
        private readonly ILogger<TipoCambiosController> _logger;

        public TipoCambiosController(SistPresupuestosContext context, ILogger<TipoCambiosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: TipoCambios
        public async Task<IActionResult> Index()
        {
            string fecha = DateTime.Today.ToString("yyyy-MM");
            ViewBag.FechaMaxima = fecha;

            //Primer item de la tabla
            ViewBag.Mostrar = _context.TipoCambios
                .OrderByDescending(tc => tc.Anio)
                .ThenByDescending(tc => tc.Mes)
                .FirstOrDefault();

            return _context.TipoCambios != null ? 
                          View(await _context.TipoCambios
                            .OrderByDescending(tc => tc.Anio)
                            .ThenByDescending(tc => tc.Mes)
                            .ToListAsync()) : Problem("Entity set 'SistPresupuestosContext.TipoCambios'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerValor(string txtMes, string txtAnio)
        {
            ViewBag.Error = "";

            if (txtMes.IsNullOrEmpty() || txtAnio.IsNullOrEmpty())
            {
                ViewBag.Mostrar = _context.TipoCambios
                .OrderByDescending(tc => tc.Anio)
                .ThenByDescending(tc => tc.Mes)
                .FirstOrDefault();

                ViewBag.Error = "Ingrese un mes y año por favor.";
                return View("Index");
            }

            byte mes = byte.Parse(txtMes);
            Int16 anio = Int16.Parse(txtAnio);
            if (_context.TipoCambios.Where(tc => tc.Mes == mes && tc.Anio == anio).FirstOrDefault() == null)
            {
                
                return Json("Combinación de mes y año no ingresada en la base de datos.");
            }

            var mostrar = _context.TipoCambios.Where(tc => tc.Mes == mes && tc.Anio == anio).FirstOrDefault();

            return Json(mostrar);


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
            string fecha = DateTime.Today.ToString("yyyy-MM");
            ViewBag.FechaMaxima = fecha;

            return View();
        }

        // POST: TipoCambios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mes,Anio,Valor")] TipoCambio tipoCambio)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(tipoCambio);
                    await _context.SaveChangesAsync();
                    ViewBag.Mensaje = "Datos ingresados exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException ex)
            {
                ViewBag.Mensaje = "La combinación de año y mes ingresada ya se encuentra en la base de datos.";
                return View(tipoCambio);
            }
            
            ViewBag.Mensaje = "Error al ingresar los datos";
            return View(tipoCambio);
        }

        // POST: TipoCambios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(byte mes,Int16 anio, [Bind("Mes,Anio,Valor")] TipoCambio tipoCambio)
        {
            if (mes != tipoCambio.Mes || anio != tipoCambio.Anio)
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
