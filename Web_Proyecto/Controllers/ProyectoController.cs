using ENTIDAD;
using NEGOCIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Web_Proyecto.Controllers
{
    public class ProyectoController : Controller
    {
        // GET: Proyecto
        public ActionResult Index()
        {
            var pctos = ProyectoCN.ListarProyectos();
            return View(pctos);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }
        // Enviar informacion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Proyecto pcto)
        {
            try
            {
                if (pcto.NombreProyecto == null)
                    return Json(new { ok = false, msg = "Debe ingresar el nombre del proyecto" }, JsonRequestBehavior.AllowGet);

                ProyectoCN.Agregar(pcto);
                return Json(new { ok = true, toRedirect = "Index" }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
                //ModelState.AddModelError("", "Ocurrió un error al agregar un Proyecto");
                //return View(pcto);
            }
        }

        public ActionResult GetProyectos(int id)
        {
            var pcto = ProyectoCN.GetProyectos(id);
            return View(pcto);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var pcto = ProyectoCN.GetProyectos(id);
            return View(pcto);
        }

        [HttpPost]
        public ActionResult Editar(Proyecto pcto)
        {
            try
            {
                if (pcto.NombreProyecto == null)
                {
                    ModelState.AddModelError("", "Debe ingresar un nombre de proyecto");
                    return View(pcto);
                }
                ProyectoCN.Editar(pcto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al editar un proyecto");
                return View(pcto);
            }
        }
        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var pcto = ProyectoCN.GetProyectos(id.Value);
            return View(pcto);
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                ProyectoCN.Eliminar(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al Eliminar un proyecto");
                return View(id);
            }
        }
    }
}