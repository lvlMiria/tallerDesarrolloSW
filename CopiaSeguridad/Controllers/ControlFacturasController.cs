using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    public class ControlFacturasController : Controller
    {
        private readonly SistPresupuestosContext _context;
        private readonly ILogger<FacturasController> _logger;
        public ControlFacturasController(SistPresupuestosContext context, ILogger<FacturasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ControlFacturas
        public async Task<IActionResult> Index()
        {
            var sistPresupuestosContext = _context.ControlFacturas
                .Include(f => f.CodPresupuesto);
            return View(await sistPresupuestosContext.ToListAsync());
        }

        // POST: ControlFacturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(short mes, string textoSIT, Int16 codItem, Int16 anio, [Bind("CodControl,CodPresupuesto,CodFactura,Origen,FechaRecepcion,FechaEntrega,Comentario")] ControlFactura controlFactura)
        {
            var cambio = _context.TipoCambios.Where(tc => tc.Mes == mes && tc.Anio == anio).FirstOrDefault();
            if (cambio != null)
            {
                //Se busca el último CodFactura
                var ultimoCodFactura = _context.Facturas.OrderByDescending(f => f.CodFactura).Select(f => f.CodFactura).FirstOrDefault();
                controlFactura.CodFactura = ultimoCodFactura + 1;

                //Se busca el último CodControl
                var ultimoCodControl = _context.ControlFacturas.OrderByDescending(cf => cf.CodControl).Select(cf => cf.CodControl).FirstOrDefault();
                controlFactura.CodControl = ultimoCodControl + 1;

                //Para combinar el sit con su código
                if (controlFactura.Origen != "contrato")
                {
                    controlFactura.Origen = textoSIT;
                }

                //es el codPresupuesto donde esté el codItem seleccionado y año contables
                //Dejé de considerar año ya que hay casos que no se compra el mismo mes que se presupuestó
                var codPresupuesto = _context.Presupuestos
                .Where(p =>
                        p.CodItem == codItem &&
                        //p.Mes == factura.MesContable &&
                        p.Anio == anio)
                    .Select(p => p.CodPresupuesto)
                    .FirstOrDefault();
                controlFactura.CodPresupuesto = codPresupuesto;

                if (controlFactura.Comentario == null)
                {
                    controlFactura.Comentario = "Sin comentario";
                }

                if (ModelState.IsValid)
                {
                    _context.Add(controlFactura);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errores = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                    foreach (var error in errores)
                    {
                        _logger.LogInformation("AAAAAAAAAAAA " + error);
                    }
                }
                return View(controlFactura);
            }

            return Json("No se ha ingresado el valor del dólar de este periodo de tiempo.");
        }

        //// GET: Ipcs/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.ControlFacturas == null)
        //    {
        //        return NotFound();
        //    }

        //    var controlFactura = await _context.ControlFacturas.FindAsync(id);
        //    if (controlFactura == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(controlFactura);
        //}

        // POST: Ipcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
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
                var sistPresupuestosContext = _context.Facturas
                .Include(f => f.TipoCambio);

                var items = await _context.Items.ToListAsync();
                ViewBag.Items = items;

                ViewBag.Control = await _context.ControlFacturas.ToListAsync();

                return RedirectToAction("Index","Facturas", sistPresupuestosContext);
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
