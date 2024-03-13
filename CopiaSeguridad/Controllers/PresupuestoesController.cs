using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
            ViewBag.Conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Items = await _context.Items.ToListAsync();
            ViewBag.Control = await _context.ControlFacturas.ToListAsync();

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

        [HttpGet]
        public IActionResult ItemsPorConcepto(byte codConcepto)
        {
            var items = _context.Items.Where(i => i.Estado == 1).ToList();
            if (codConcepto == 0)
            {   
                return Json(items);
            }
            items = _context.Items.Where(i => i.CodConcepto == codConcepto && i.Estado == 1).ToList();
            return Json(items);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> ObtenerItem(short codItem)
        {
            var item = _context.Items.Where(i => i.CodItem == codItem).FirstOrDefault();
            ViewBag.Concepto = _context.Conceptos.Where(c => c.CodConcepto == item.CodConcepto).Select(c=>c.DescConcepto);

            return Json(item);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> ObtenerConcepto(short codItem)
        {
            var codConcepto = _context.Items.Where(i => i.CodItem == codItem).Select(i=>i.CodConcepto).FirstOrDefault();
            var concepto = _context.Conceptos.Where(c => c.CodConcepto == codConcepto).Select(c => c.DescConcepto);

            return Json(concepto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> ObtenerMontos(short codItem, short anio)
        {
            var facturas = await _context.ControlFacturas
            .Where(cf => cf.CodPresupuestoNavigation.CodItem == codItem)
            .Select(cf => cf.CodFacturaNavigation)
            .ToListAsync();

            var montosPorMeses = Enumerable.Repeat((decimal)0, 12).ToList();
            var facturasDelAnio = facturas.Where(f => f.AnioContable == anio).ToList();
            if (facturasDelAnio != null)
            {
                foreach (var factura in facturasDelAnio)
                {
                    var montoUSD = CLPaUSD(factura.Monto,factura.MesContable,anio);
                    montosPorMeses[factura.MesContable - 1] += montoUSD;
                }
            }

            return Json(montosPorMeses);
        }

        public decimal CLPaUSD(decimal monto,byte mes,short anio)
        {
            decimal mesDolar = _context.TipoCambios.Where(tc => tc.Mes == mes && tc.Anio == anio).Select(tc => tc.Valor).FirstOrDefault();
            if(mesDolar != null)
            {
                decimal valor = Math.Round(monto / mesDolar, 2);
                return valor;
            }
            else
            {
                return 0;
            }
        }

        //Obtener presupuestos a partir del item
        public async Task<IActionResult> ObtenerPresupuestos(short codItem, short anio)
        {
            var presupuestos = _context.Presupuestos.Where(p => p.CodItem == codItem && p.Anio == anio);

            var montosPorMeses = Enumerable.Repeat(0, 12).ToList();
            if (presupuestos != null)
            {
                foreach (var presupuesto in presupuestos)
                {
                    montosPorMeses[(presupuesto.Mes - 1)] += presupuesto.PresupuestoMes;
                }
            }

            return Json(montosPorMeses);
        }

        //Obtener presupuestos a partir del item
        public async Task<IActionResult> ObtenerCodigos(short codItem, short anio)
        {
            var presupuestos = _context.Presupuestos.Where(p => p.CodItem == codItem && p.Anio == anio);

            var codPorMeses = Enumerable.Repeat(0, 12).ToList();
            if (presupuestos != null)
            {
                foreach (var presupuesto in presupuestos)
                {
                    codPorMeses[(presupuesto.Mes - 1)] += presupuesto.CodPresupuesto;
                }
            }

            return Json(codPorMeses);
        }

        //Obtener presupuestos a partir del item
        public async Task<IActionResult> CalcularPresupuestos(short codItem, short anio)
        {
            var facturas = await _context.ControlFacturas
            .Where(cf => cf.CodPresupuestoNavigation.CodItem == codItem)
            .Select(cf => cf.CodFacturaNavigation)
            .ToListAsync();

            facturas = facturas.Where(f => f.AnioContable == anio).ToList();
            var ipc = _context.Ipcs.Where(i=>i.Anio == anio).Select(i=>i.Valor).FirstOrDefault();
            var montosPorMeses = Enumerable.Repeat((decimal)0, 12).ToList();

            if (ipc == null)
            {
                return Json("IPC no ingresado.");
            }
            else
            {
                
                if (facturas != null)
                {
                    foreach(var factura in facturas)
                    {
                        decimal valorPropuesto = (decimal)(factura.Monto + (factura.Monto * ipc/100));
                        valorPropuesto = CLPaUSD(valorPropuesto, factura.MesContable, anio);
                        montosPorMeses[(factura.MesContable - 1)] += valorPropuesto;
                    }
                }
                else
                {
                    return Json("No hay facturas del año pasado.");
                }
            }
            return Json(montosPorMeses);
        }

        // GET: Detalle de Presupuestos
        public async Task<IActionResult> ListaPresupuestos()
        {
            var sistPresupuestosContext = _context.Presupuestos.Include(p => p.CodItemNavigation);
            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            var conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;

            return View(await sistPresupuestosContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CalcularDiferencia(short codItem, short anio)
        {

            //Buscar como usar funciones ObtenerPresupuesto y ObtenerMonto para reducir codigo repetido

            var facturas = await _context.ControlFacturas
            .Where(cf => cf.CodPresupuestoNavigation.CodItem == codItem)
            .Select(cf => cf.CodFacturaNavigation)
            .ToListAsync();

            //Sumar todas las facturas del item
            decimal totalFacturas = 0;
            var facturasDelAnio = facturas.Where(f => f.AnioContable == anio).ToList();
            if (facturasDelAnio != null)
            {
                foreach (var factura in facturasDelAnio)
                {
                    var montoUSD = CLPaUSD(factura.Monto, factura.MesContable, anio);
                    totalFacturas += montoUSD;
                }
            }

            //Sumar todos los presupuestos del item
            var presupuestos = _context.Presupuestos.Where(p => p.CodItem == codItem && p.Anio == anio);
            decimal totalPresu = 0;
            if (presupuestos != null)
            {
                foreach (var presupuesto in presupuestos)
                {
                    totalPresu += presupuesto.PresupuestoMes;
                }
            }

            decimal diferencia = 0;
            decimal difAbsoluta = Math.Abs(totalFacturas - totalPresu);

            if (totalPresu != 0) {
                diferencia = (difAbsoluta / totalPresu) * 100;
            }

            return Json(Math.Round(diferencia, 2));

        }

        // GET: Resumen de Presupuestos
        public async Task<IActionResult> ResumenPresupuestos()
        {
            var sistPresupuestosContext = _context.Presupuestos.Include(p => p.CodItemNavigation);
            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            var conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;

            var control = await _context.ControlFacturas.ToListAsync();
            ViewBag.Control = control;

            var facturas = await _context.Facturas.ToListAsync();
            ViewBag.Facturas = facturas;

            var cambio = await _context.TipoCambios.ToListAsync();
            ViewBag.Cambio = cambio;

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

            //datos específicos
            var item = _context.Items.FirstOrDefault(i => i.CodItem == presupuesto.CodItem);
            ViewBag.Item = item;
            var concepto = _context.Conceptos.FirstOrDefault(c => c.CodConcepto == item.CodConcepto);
            ViewBag.Concepto = concepto;

            //para rellenar la página
            short anioPasado = (short)(DateTime.Now.Year - 1);
            ViewBag.Ipcs = _context.Ipcs.Where(i => i.Anio == anioPasado).Select(i => i.Valor).FirstOrDefault();

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

            return View(presupuesto);
        }

        // GET: Presupuestoes/Create
        public async Task<IActionResult> Create()
        {
            var conceptos = await _context.Conceptos.ToListAsync();
            ViewBag.Conceptos = conceptos;

            var items = await _context.Items.ToListAsync();
            ViewBag.Items = items;

            var ipc = _context.Ipcs.Where(i => i.Anio == DateTime.Now.Year - 1).Select(i => i.Valor).FirstOrDefault();
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

        // POST: Presupuestoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
