using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presentacion.Models;
using static Presentacion.Controllers.FacturasController;


namespace Presentacion.Controllers
{
    public class LFacturaViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Concepto> Conceptos { get; set; }
        public IEnumerable<Factura> Facturas { get; set; }
        public IEnumerable<ControlFactura> Controles { get; set; }
        public IEnumerable<Presupuesto> Presupuestos { get; set; }

    }
    public class FacturasController : Controller
    {
        private readonly SistPresupuestosContext _context;
        private readonly ILogger<FacturasController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public FacturasController(SistPresupuestosContext context, ILogger<FacturasController> logger, IHttpClientFactory clientFactory)
        {
            _context = context;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        // GET: Facturas
        public async Task<IActionResult> Index()
        {
            var sistPresupuestosContext = _context.Facturas
                .Include(f => f.TipoCambio);

            ViewBag.Items = await _context.Items.ToListAsync();
            ViewBag.Control = await _context.ControlFacturas.ToListAsync();

            return View(await sistPresupuestosContext.ToListAsync());
        }

        public async Task<IActionResult> ObtenerFacturas(Int16 codItem)
        {
            var facturas = await _context.Facturas
                .Where(f => f.ControlFacturas.Any(cf => cf.CodPresupuestoNavigation.CodItem == codItem))
                .ToListAsync();

            return Json(facturas);
        }
        public class Empresa
        {
            public string RutEmpresa { get; set; }
            public string Verificador { get; set; }
            public string NomEmpresa { get; set; }
        }
        //INTENTO DE CONEXIÓN A AS400. FALTA DRIVER
        public ActionResult ObtenerEmpresas()
        {

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("AS400");
            var queryString = "SELECT RUT_PRVD,DIGRUT_PRVD,NOMBRE_PRVD FROM SI2RHO.PRVD WHERE STATUS_PRVD = 'A' ORDER BY NOMBRE_PRVD";
            //status I or A

            List<Empresa> retorno = new List<Empresa>();

            using (OdbcConnection connection = new(connectionString))
            {
                OdbcCommand command = new(queryString, connection);

                connection.Open();
                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Empresa empresa = new Empresa
                    {
                        RutEmpresa = reader["RUT_PRVD"].ToString(),
                        Verificador = reader["RUT_PRVD"].ToString(),
                        NomEmpresa = reader["NOMBRE_PRVD"].ToString()
                    };
                    retorno.Add(empresa);
                }
                reader.Close();
            }
            return Json(retorno);
        }
    

        // GET: Lista de Facturas
        public async Task<IActionResult> ListaFacturas()
        {
            ViewBag.Items = await _context.Items.ToListAsync();

            ViewBag.Conceptos = await _context.Conceptos.ToListAsync();

            var items = _context.Items.ToList();
            var conceptos = _context.Conceptos.ToList();
            var facturas = _context.Facturas.Include(f => f.TipoCambio).ToList();
            var control = _context.ControlFacturas.ToList();
            var presupuesto = _context.Presupuestos.ToList();

            var viewModel = new LFacturaViewModel
            {
                Items = items,
                Conceptos = conceptos,
                Facturas = facturas,
                Controles = control,
                Presupuestos = presupuesto
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ResumenFacturas()
        {
            ViewBag.Items = await _context.Items.ToListAsync();
            ViewBag.Conceptos = await _context.Conceptos.ToListAsync();

            var items = _context.Items.ToList();
            var conceptos = _context.Conceptos.ToList();
            var facturas = _context.Facturas.Include(f => f.TipoCambio).ToList();
            var control = _context.ControlFacturas.ToList();
            var presupuesto = _context.Presupuestos.ToList();

            var viewModel = new LFacturaViewModel
            {
                Items = items,
                Conceptos = conceptos,
                Facturas = facturas,
                Controles = control,
                Presupuestos = presupuesto
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ObtenerItem(int codFactura)
        {
            //Ninguno debería dar null, pero por si acaso
            //Busco el control factura correspondiente a la factura
            var controlFactura = _context.ControlFacturas.FirstOrDefault(cf => cf.CodFactura == codFactura);
            if (controlFactura != null)
            {
                //Luego el presupuesto que esta asociado a ese control
                //Puede haber mas de uno, pero como el objetivo es el item, no necesitamos todos
                var presupuesto = _context.Presupuestos.FirstOrDefault(p => p.CodPresupuesto == controlFactura.CodPresupuesto);
                if (presupuesto != null)
                {
                    //Luego consigo el item del presupuesto
                    var item = _context.Items.FirstOrDefault(i => i.CodItem == presupuesto.CodItem);
                    return Json(item.DescItem);

                }

            }

            return Json("No identificado");

        }
        public async Task<ActionResult> ObtenerConcepto(int codFactura)
        {
            //OPTIMIZAR: Intentar usar la funcion BuscarItem aquí.

            //Busco el control factura correspondiente a la factura
            var controlFactura = _context.ControlFacturas.FirstOrDefault(cf => cf.CodFactura == codFactura);
            if (controlFactura != null)
            {
                //Luego el presupuesto que esta asociado a ese control
                //Puede haber mas de uno, pero como el objetivo es el item, no necesitamos todos
                var presupuesto = _context.Presupuestos.FirstOrDefault(p => p.CodPresupuesto == controlFactura.CodPresupuesto);
                if (presupuesto != null)
                {
                    //Luego consigo el item del presupuesto
                    var item = _context.Items.FirstOrDefault(i => i.CodItem == presupuesto.CodItem);
                    if (item != null)
                    {
                        //Y finalmente su concepto
                        var concepto = _context.Conceptos.FirstOrDefault(c => c.CodConcepto == item.CodConcepto);
                        return Json(concepto.DescConcepto);
                    }

                }

            }

            return Json("No identificado");

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

            ViewBag.Items = await _context.Items.ToListAsync();
            ViewBag.Control = await _context.ControlFacturas.ToListAsync();

            var datos = new Dictionary<string, object>();

            var controlFactura = _context.ControlFacturas.FirstOrDefault(cf => cf.CodFactura == id);
            var presupuesto = _context.Presupuestos.FirstOrDefault(p => p.CodPresupuesto == controlFactura.CodPresupuesto);
            var item = _context.Items.FirstOrDefault(i => i.CodItem == presupuesto.CodItem);

            datos["Item"] = item;
            datos["Factura"] = _context.Facturas.FirstOrDefault(f=>f.CodFactura==id);
            datos["Control"] = controlFactura;

            ViewBag.Datos = datos;

            return View(factura);
        }

        // GET: Facturas/Create
        public IActionResult Create()
        {
            var items = _context.Items.ToList();
            ViewBag.Items = items;

            var recepcion = _context.ControlFacturas.OrderByDescending(cf => cf.FechaRecepcion).Select(cf => cf.FechaRecepcion).FirstOrDefault();
            ViewBag.Recepcion = recepcion;

            var mesContable = _context.Facturas.OrderByDescending(f => f.AnioContable).OrderByDescending(f => f.MesContable).Select(f => f.MesContable).FirstOrDefault();
            var anioContable = _context.Facturas.OrderByDescending(f => f.AnioContable).OrderByDescending(f => f.MesContable).Select(f => f.AnioContable).FirstOrDefault();
            ViewBag.Contable = new { Mes = mesContable, Anio = anioContable };

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
            //Se busca el último CodFactura
            var ultimoCodFactura = _context.Facturas.OrderByDescending(f => f.CodFactura).Select(f => f.CodFactura).FirstOrDefault();


            if (ModelState.IsValid)
            {
                factura.CodFactura = ultimoCodFactura + 1;

                _context.Add(factura);
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
                    _logger.LogInformation(error);
                }

                ViewData["MesContable"] = new SelectList(_context.TipoCambios, "Mes", "Mes", factura.MesContable);
                return View(factura);
            }
        }

        [HttpGet]
        public async Task<ActionResult> BuscarDolar(byte mes, short anio)
        {
            var cambio = _context.TipoCambios.Where(tc=>tc.Anio == anio && tc.Mes == mes).FirstOrDefault();
            if(cambio == null)
            {
                return Json(null);
            }
            else
            {
                return Json("Validado");
            }
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("CodFactura,NumFactura,Monto,MesContable,AnioContable,Empresa")] Factura factura)
        {
            List<string> errores = new List<string>();
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
                ViewData["MesContable"] = new SelectList(_context.TipoCambios, "Mes", "Mes", factura.MesContable);
                return View(factura);
            }
            else
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogInformation(error.ErrorMessage);
                    }
                }
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
