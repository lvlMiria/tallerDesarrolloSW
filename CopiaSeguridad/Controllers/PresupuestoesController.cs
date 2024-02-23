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
        private readonly ILogger<FacturasController> _logger;

        public PresupuestoesController(SistPresupuestosContext context, ILogger<FacturasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Presupuestoes
        public async Task<IActionResult> Index()
        {
            var conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;

            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            var ipc = _context.Ipcs.Where(i=>i.Anio == DateTime.Now.Year-1).Select(i=>i.Valor).FirstOrDefault();
            if (ipc == null)
            {
                //No se ha ingresado el valor del año actual
                ViewBag.Ipcs = "No ingresado";
            }
            else
            {
                //IPC año pasado
                ViewBag.Ipcs = ipc;
            }
                

            var control = await _context.ControlFacturas.ToListAsync();
            ViewBag.Control = control;

            var facturas = await _context.Facturas.ToListAsync();
            ViewBag.Facturas = facturas;

            List<int> meses = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                meses.Add(i);
            }
            ViewBag.Meses = meses;

            List<string> mesesLetras = new List<string>();
            mesesLetras.Add("Enero");
            mesesLetras.Add("Febrero");
            mesesLetras.Add("Marzo");
            mesesLetras.Add("Abril");
            mesesLetras.Add("Mayo");
            mesesLetras.Add("Junio");
            mesesLetras.Add("Julio");
            mesesLetras.Add("Agosto");
            mesesLetras.Add("Septiembre");
            mesesLetras.Add("Octubre");
            mesesLetras.Add("Noviembre");
            mesesLetras.Add("Diciembre");
            ViewBag.MesesLetras = mesesLetras;

            int anioActual = DateTime.Now.Year;
            ViewBag.Presupuestos = _context.Presupuestos.Where(p => p.Anio == anioActual).ToList();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> ObtenerFacturasItem(short codItem, int anio)
        {
            var facturas = await _context.ControlFacturas
            .Where(cf => cf.CodPresupuestoNavigation.CodItem == codItem)
            .Select(cf => cf.CodFacturaNavigation)
            .ToListAsync();

            facturas = facturas.Where(cf => cf.AnioContable == anio).ToList();

            return Ok(facturas);
        }

        public int? CLPaUSD(int monto,byte mes)
        {
            int anioActual = DateTime.Now.Year;
            int? mesDolar = _context.TipoCambios.Where(tc => tc.Mes == mes && tc.Anio == anioActual).Select(tc => tc.Valor).FirstOrDefault();
            if(mesDolar != null)
            {
                int? valor = monto / mesDolar;
                return valor;
            }
            else
            {
                return 0;
            }
        }

        //Obtener presupuestos a partir del item
        public async Task<IActionResult> ObtenerPresupuestos(Int16 codItem)
        {
            int anioActual = DateTime.Now.Year;
            var presupuestos = await _context.Presupuestos
                .Where(p => p.CodItem == codItem && p.Anio == anioActual)
                .ToListAsync();

            return Json(presupuestos);
        }

        // GET: Lista de Presupuestos
        public async Task<IActionResult> ListaPresupuestos()
        {
            var sistPresupuestosContext = _context.Presupuestos.Include(p => p.CodItemNavigation);
            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            var conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;

            return View(await sistPresupuestosContext.ToListAsync());
        }

        // GET: Presupuestoes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Presupuestos == null)
            {
                return NotFound();
            }

            var presupuesto = await _context.Presupuestos
                .Include(p => p.CodItemNavigation)
                .FirstOrDefaultAsync(m => m.CodPresupuesto == id);
            if (presupuesto == null)
            {
                return NotFound();
            }

            return View(presupuesto);
        }

        // GET: Presupuestoes/Create
        public async Task<IActionResult> Create()
        {
            var conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;

            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            var ipc = await _context.Ipcs.ToListAsync();
            ViewBag.Ipcs = ipc;

            var control = await _context.ControlFacturas.ToListAsync();
            ViewBag.Control = control;

            var facturas = await _context.Facturas.ToListAsync();
            ViewBag.Facturas = facturas;

            List<int> meses = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                meses.Add(i);
            }
            ViewBag.Meses = meses;

            List<string> mesesLetras = new List<string>();
            mesesLetras.Add("Enero");
            mesesLetras.Add("Febrero");
            mesesLetras.Add("Marzo");
            mesesLetras.Add("Abril");
            mesesLetras.Add("Mayo");
            mesesLetras.Add("Junio");
            mesesLetras.Add("Julio");
            mesesLetras.Add("Agosto");
            mesesLetras.Add("Septiembre");
            mesesLetras.Add("Octubre");
            mesesLetras.Add("Noviembre");
            mesesLetras.Add("Diciembre");
            ViewBag.MesesLetras = mesesLetras;

            ViewData["Anio"] = new SelectList(_context.Ipcs, "Anio", "Anio");
            ViewData["CodItem"] = new SelectList(_context.Items, "CodItem", "CodItem");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(short codItem, byte mes, int presupuesto)
        {
            if (ModelState.IsValid)
            {
                short anio = (short)(DateTime.Now.Year);
                Presupuesto nuevoPresupuesto = new Presupuesto
                {
                    Mes = mes,
                    Anio = anio,
                    CodItem = codItem,
                    PresupuestoMes = presupuesto
                };

                _context.Add(nuevoPresupuesto);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            // Si hay errores de validación, recupera los datos necesarios para el formulario
            var conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;

            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            var ipc = await _context.Ipcs.ToListAsync();
            ViewBag.Ipcs = ipc;

            var control = await _context.ControlFacturas.ToListAsync();
            ViewBag.Control = control;

            var facturas = await _context.Facturas.ToListAsync();
            ViewBag.Facturas = facturas;

            List<int> meses = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                meses.Add(i);
            }
            ViewBag.Meses = meses;

            List<string> mesesLetras = new List<string>();
            mesesLetras.Add("Enero");
            mesesLetras.Add("Febrero");
            mesesLetras.Add("Marzo");
            mesesLetras.Add("Abril");
            mesesLetras.Add("Mayo");
            mesesLetras.Add("Junio");
            mesesLetras.Add("Julio");
            mesesLetras.Add("Agosto");
            mesesLetras.Add("Septiembre");
            mesesLetras.Add("Octubre");
            mesesLetras.Add("Noviembre");
            mesesLetras.Add("Diciembre");
            ViewBag.MesesLetras = mesesLetras;


            return View();
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CodPresupuesto,Mes,Anio,PresupuestoMes,CodItem")] Presupuesto presupuesto)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(presupuesto);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Anio"] = new SelectList(_context.Ipcs, "Anio", "Anio", presupuesto.Anio);
        //    ViewData["CodItem"] = new SelectList(_context.Items, "CodItem", "CodItem", presupuesto.CodItem);
        //    return View(presupuesto);
        //}

        // GET: Presupuestoes/Edit/5
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(int id, [Bind("CodPresupuesto,Mes,Anio,PresupuestoMes,CodItem")] Presupuesto presupuesto)
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
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Presupuestos == null)
            {
                return NotFound();
            }

            var presupuesto = await _context.Presupuestos
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
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        private bool PresupuestoExists(int id)
        {
          return (_context.Presupuestos?.Any(e => e.CodPresupuesto == id)).GetValueOrDefault();
        }

      
    }
}
