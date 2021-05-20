using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ENTIDAD;
using NEGOCIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_Proyecto.App_Start;

namespace Web_Empleado.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Index()
        {
            var empleado = EmpleadoCN.ListarEmpleados();
            return View(empleado);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }
        // Enviar informacion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Empleado empleado)
        {
            try
            {
                if (empleado.Nombres == null)
                    return Json(new { ok = false, msg = "Debe ingresar el nombre del Empleado" }, JsonRequestBehavior.AllowGet);

                EmpleadoCN.Agregar(empleado);
                return Json(new { ok = true, toRedirect = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
                //ModelState.AddModelError("", "Ocurrió un error al agregar un Empleado");
                //return View(pcto);
            }
        }

        public ActionResult GetEmpleados(int id)
        {
            var empleado = EmpleadoCN.ObtenerEmpleado(id);
            return View(empleado);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var empleado = EmpleadoCN.ObtenerEmpleado(id);
            return View(empleado);
        }

        [HttpPost]
        public ActionResult Editar(Empleado empleado)
        {
            try
            {
                if (empleado.Nombres == null)
                    return Json(new { ok = false, msg = "Debe ingresar el nombre del Empleado" }, JsonRequestBehavior.AllowGet);

                EmpleadoCN.Editar(empleado);
                return Json(new { ok = true, toRedirect = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                EmpleadoCN.Eliminar(id);
                return Json(new { ok = true, toRedirect = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListarEmpleados()
        {
            try
            {
                var lista = EmpleadoCN.ListarEmpleados();
                return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RptEmpleado()
        {
            return View();
        }

        public ActionResult DescargarReporteEmpleado(int codigo, string algo)
        //public ActionResult DescargarReporteEmpleado(int codigo)
        {
            try
            {
                var rptH = new ReportClass();
                rptH.FileName = Server.MapPath("/Reportes/EmpleadoReporte1.rpt");
                rptH.Load();

                // por parametro
                rptH.SetParameterValue("DptoId", codigo);
                //rptH.SetParameterValue("ParamAlgo", algo);

                // Report connection
                var connInfo = CrystalReportsCnn.GetConnectionInfo();
                TableLogOnInfo logonInfo = new TableLogOnInfo();
                Tables tables;
                tables = rptH.Database.Tables;
                foreach (Table table in tables)
                {
                    logonInfo = table.LogOnInfo;
                    logonInfo.ConnectionInfo = connInfo;
                    table.ApplyLogOnInfo(logonInfo);
                }

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                // Descargar en PDF
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                rptH.Dispose();
                rptH.Close();
                return new FileStreamResult(stream, "application/pdf");

                // Descargar en Excel
                //Stream stream = rptH.ExportToStream(ExportFormatType.Excel);
                //stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/vnd.ms-excel", "empleadoRpt.xls");
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }            
        }

        public ActionResult DescargarReporteEmpleadoExcel()
        {
            try
            {
                var rptH = new ReportClass();
                rptH.FileName = Server.MapPath("/Reportes/EmpleadoReporte1.rpt");
                rptH.Load();

                // Report connection
                var connInfo = CrystalReportsCnn.GetConnectionInfo();
                TableLogOnInfo logonInfo = new TableLogOnInfo();
                Tables tables;
                tables = rptH.Database.Tables;
                foreach (Table table in tables)
                {
                    logonInfo = table.LogOnInfo;
                    logonInfo.ConnectionInfo = connInfo;
                    table.ApplyLogOnInfo(logonInfo);
                }

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                // Descargar en PDF
                //Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                //rptH.Dispose();
                //rptH.Close();
                //return new FileStreamResult(stream, "application/pdf");

                // Descargar en Excel
                Stream stream = rptH.ExportToStream(ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/vnd.ms-excel", "empleadoRpt.xls");
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}