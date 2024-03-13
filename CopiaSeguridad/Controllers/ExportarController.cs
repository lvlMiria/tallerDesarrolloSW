using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presentacion.Models;


namespace Presentacion.Controllers
{
    public class ExportarController : Controller
    {
        private readonly ILogger<FacturasController> _logger;
        private readonly SistPresupuestosContext _context;
        public ExportarController(SistPresupuestosContext context, ILogger<FacturasController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult ExportarExcel()
        {
            // Lógica para generar el archivo Excel
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Datos");

                // Encabezados
                var headers = new string[] { "Item", "Número Factura", "Concepto", "Empresa", "Origen", "Monto CLP", "Monto USD", "Fecha Recepción", "Fecha Entrega", "Mes Contable", "Año Contable", "Comentarios" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                // Datos de la tabla
                var facturas = _context.Facturas.ToList();
                for (int i = 0; i < facturas.Count; i++)
                {
                    var factura = facturas[i];

                    var controlFactura = _context.ControlFacturas.FirstOrDefault(cf => cf.CodFactura == factura.CodFactura);
                    var presupuesto = _context.Presupuestos.FirstOrDefault(p => p.CodPresupuesto == controlFactura.CodPresupuesto);
                    var item = _context.Items.FirstOrDefault(i => i.CodItem == presupuesto.CodItem);
                    var concepto = _context.Conceptos.FirstOrDefault(c => c.CodConcepto == item.CodConcepto);
                    var dolar = _context.TipoCambios.Where(tc => tc.Mes == factura.MesContable && tc.Anio == factura.AnioContable).FirstOrDefault();

                    //Si el dolar no ha sido ingresado para ese mes y año, queda en 0.
                    decimal resultado = 0;
                    if (dolar.Valor != 0 && dolar != null)
                    {
                        resultado = Math.Round(factura.Monto / dolar.Valor, 2);
                    }

                    worksheet.Cells[i + 2, 1].Value = item.DescItem;
                    worksheet.Cells[i + 2, 2].Value = factura.NumFactura;
                    worksheet.Cells[i + 2, 3].Value = concepto.DescConcepto;
                    worksheet.Cells[i + 2, 4].Value = factura.Empresa;
                    worksheet.Cells[i + 2, 5].Value = controlFactura.Origen;
                    worksheet.Cells[i + 2, 6].Value = factura.Monto;
                    worksheet.Cells[i + 2, 7].Value = resultado;
                    worksheet.Cells[i + 2, 8].Value = controlFactura.FechaRecepcion.ToString();
                    worksheet.Cells[i + 2, 9].Value = controlFactura.FechaEntrega.ToString();
                    worksheet.Cells[i + 2, 10].Value = factura.MesContable;
                    worksheet.Cells[i + 2, 11].Value = factura.AnioContable;
                    worksheet.Cells[i + 2, 12].Value = controlFactura.Comentario;
                }

                package.Save();
            }

            stream.Position = 0;

            string fechaDescarga = DateTime.Now.ToString("dd_MM_yyyy");
            string nombreArchivo = $"facturas_{fechaDescarga}.xlsx";
            // Devolver el archivo Excel como un FileResult
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }

        public IActionResult ExportarPresupuestos(List<string> datos)
        {
            if (datos == null || datos.Count == 0)
            {
                return BadRequest("No hay datos para exportar.");
            }

            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Presupuestos");

                    int row = 1;
                    int col = 1;

                    string[] encabezados = { "Item", "Tipo", "Concepto", "Presupuesto anual", "Gasto Real", "Diferencia", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", "Año" };
                    for (int i = 0; i < encabezados.Length; i++)
                    {
                        worksheet.Cells[row, col + i].Value = encabezados[i];
                    }
                    row++;

                    foreach (var dato in datos)
                    {
                        worksheet.Cells[row, col].Value = dato;
                        if(col == 19)
                        {
                            row++;
                            col = 0;
                        }
                        col++;
                    }

                    var fileBytes = package.GetAsByteArray();

                    string fechaDescarga = DateTime.Now.ToString("dd_MM_yyyy");
                    string nombreArchivo = $"presupustos_{fechaDescarga}.xlsx";

                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al generar el archivo Excel: {ex.Message}");
            }
        
        }

        public IActionResult ExportarResumenPres(List<string> datos)
        {
            if (datos == null || datos.Count == 0)
            {
                return BadRequest("No hay datos para exportar.");
            }

            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Presupuestos");

                    int row = 1;
                    int col = 1;

                    string[] encabezados = { "Concepto", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", "Presupuesto anual", "Gasto Real", "Diferencia" };
                    for (int i = 0; i < encabezados.Length; i++)
                    {
                        worksheet.Cells[row, col + i].Value = encabezados[i];
                    }
                    row++;

                    foreach (var dato in datos)
                    {
                        worksheet.Cells[row, col].Value = dato;
                        if (col == 16)
                        {
                            row++;
                            col = 0;
                        }
                        col++;
                    }

                    var fileBytes = package.GetAsByteArray();

                    string fechaDescarga = DateTime.Now.ToString("dd_MM_yyyy");
                    string nombreArchivo = $"resumen_presupustos_{fechaDescarga}.xlsx";

                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al generar el archivo Excel: {ex.Message}");
            }

        }

        public IActionResult ExportarResumenFact(List<string> datos)
        {
            if (datos == null || datos.Count == 0)
            {
                return BadRequest("No hay datos para exportar.");
            }

            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Facturas");

                    int row = 1;
                    int col = 1;

                    string[] encabezados = { "Concepto", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", "Total" };
                    for (int i = 0; i < encabezados.Length; i++)
                    {
                        worksheet.Cells[row, col + i].Value = encabezados[i];
                    }
                    row++;

                    foreach (var dato in datos)
                    {
                        worksheet.Cells[row, col].Value = dato;
                        if (col == 14)
                        {
                            row++;
                            col = 0;
                        }
                        col++;
                    }

                    var fileBytes = package.GetAsByteArray();

                    string fechaDescarga = DateTime.Now.ToString("dd_MM_yyyy");
                    string nombreArchivo = $"resumen_facturas_{fechaDescarga}.xlsx";

                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al generar el archivo Excel: {ex.Message}");
            }

        }
    }
    }
