using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    public class PresupuestoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ListaPresupuestos()
        {
            return View();
        }

        // GET: PresupuestoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PresupuestoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PresupuestoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PresupuestoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PresupuestoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PresupuestoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PresupuestoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
