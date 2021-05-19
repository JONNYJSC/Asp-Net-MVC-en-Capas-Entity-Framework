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
    [Authorize(Roles = "Admin")]
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
                return Json(new { ok = true, toRedirect = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
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
                    return Json(new { ok = false, msg = "Debe ingresar el nombre del proyecto" }, JsonRequestBehavior.AllowGet);

                ProyectoCN.Editar(pcto);
                return Json(new { ok = true, toRedirect = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
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
                return Json(new { ok = true, toRedirect = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Asignar proyecto
        [HttpGet]
        public ActionResult AsignarProyecto()
        {
            return View(ProyectoCN.ListarAsignaciones());
        }

        [HttpPost]
        public ActionResult AsignarProyecto(int proyectoId, int empleadoId)
        {
            try
            {
                if (ProyectoCN.ExisteAsignacion(proyectoId, empleadoId))
                    return Json(new { ok = false, msg = "Ya existe una relación entre este proyecto y el empleado" });

                if (!ProyectoCN.EsProyectoActivo(proyectoId))
                    return Json(new { ok = false, msg = "El proyecto ya no se encuentra activo." });

                ProyectoCN.AsignarProyecto(proyectoId, empleadoId);
                return Json(new { ok = true, toRedirect = Url.Action("AsignarProyecto") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListarProyectos()
        {
            try
            {
                var lista = ProyectoCN.ListarProyectos();
                return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EliminarAsignacion(int proyectoId, int empleadoId)
        {
            try
            {
                ProyectoCN.Eliminarasignacion(proyectoId, empleadoId);
                return Json(new { ok = true, toRedirect = Url.Action("AsignarProyecto") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}