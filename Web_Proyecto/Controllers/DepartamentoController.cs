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
    public class DepartamentoController : Controller
    {
        // GET: Departamento
        public ActionResult Index()
        {
            var dptos = DepartamentoCN.ListarDepartamentos();
            return View(dptos);
        }
        //Solicitar informacion al servidor 
        [HttpGet] 
        public ActionResult Crear()
        {
            return View();
        }
        // Enviar informacion
        [HttpPost]
        public ActionResult Crear(Departamento dpto)
        {
            try
            {
                if(dpto.NombreDepartamento == null)
                {
                    ModelState.AddModelError("", "Debe ingresar un nombre de departamento");
                    return View(dpto);
                }
                DepartamentoCN.Agregar(dpto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al agregar un departamento");
                return View(dpto);
            }            
        }

        public ActionResult GetDepartamento(int id)
        {
            var dpto = DepartamentoCN.GetDepartamento(id);
            return View(dpto);
        }

        public JsonResult GetDepartamentos()
        {
            var lista = DepartamentoCN.ListarDepartamentos();
            return Json(new { data = lista },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var dpto = DepartamentoCN.GetDepartamento(id);
            return View(dpto);
        }

        [HttpPost]
        public ActionResult Editar(Departamento dpto)
        {
            try
            {
                if (dpto.NombreDepartamento == null)
                {
                    ModelState.AddModelError("", "Debe ingresar un nombre de departamento");
                    return View(dpto);
                }
                DepartamentoCN.Editar(dpto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al editar un departamento");
                return View(dpto);
            }
        }
        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dpto = DepartamentoCN.GetDepartamento(id.Value);
            return View(dpto);
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                DepartamentoCN.Eliminar(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al Eliminar un departamento");
                return View(id);
            }
        }
    }
}