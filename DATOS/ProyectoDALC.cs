using ENTIDAD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATOS
{
    public class ProyectoDALC
    {
        public void Agregar(Proyecto pcto)
        {
            using (var db = new dbContext())
            {
                db.Proyecto.Add(pcto);
                db.SaveChanges();
            }
        }

        public List<Proyecto> ListarProyectos()
        {
            using (var db = new dbContext())
            {
                db.Configuration.LazyLoadingEnabled = false;

                //var toDay = DateTime.Now.Date;
                //return db.Proyecto.Where(p => p.FechaFin > toDay).ToList();

                return db.Proyecto.ToList();
            }
        }

        public Proyecto GetProyectos(int id)
        {
            using (var db = new dbContext())
            {
                // uso de procedimientos alamacenados
                //var miProyecto = db.Database.SqlQuery<Proyecto>("spObtenerProyecto @ProyectoId",
                //    new SqlParameter("@ProyectoId", id)).FirstOrDefault();
                //return miProyecto;

                return db.Proyecto.Find(id);
            }
        }

        public void Editar(Proyecto pcto)
        {
            using(var db= new dbContext())
            {
                var p = db.Proyecto.Find(pcto.ProyectoId);
                p.NombreProyecto = pcto.NombreProyecto;
                p.FechaInicio = pcto.FechaInicio;
                p.FechaFin = pcto.FechaFin;
                db.SaveChanges();
            }
        }

        public void Eliminar(int id)
        {
            using(var db= new dbContext())
            {
                var pcto = db.Proyecto.Find(id);
                db.Proyecto.Remove(pcto);
                db.SaveChanges();
            }
        }

        public bool ExisteAsignacion(int proyectoId, int empleadoId)
        {
            using (var db = new dbContext())
            {
                var existeAsignacion = db.ProyectoEmpleado
                    .Any(p => p.ProyectoId == proyectoId && p.EmpleadoId == empleadoId);

                return existeAsignacion;
            }
        }

        public bool EsProyectoActivo(int proyectoId)
        {
            using (var db = new dbContext())
            {
                var toDay = DateTime.Now.Date;
                var proyectoActivo = db.Proyecto
                    .Any(p => p.ProyectoId == proyectoId && p.FechaFin > toDay);

                return proyectoActivo;
            }
        }

        public void AsignarProyecto(int proyectoId, int empleadoId)
        {
            var proyectoEmp = new ProyectoEmpleado
            {
                ProyectoId = proyectoId,
                EmpleadoId = empleadoId,
                FechaAlta = DateTime.Now
            };
            using (var db = new dbContext())
            {
                db.ProyectoEmpleado.Add(proyectoEmp);
                db.SaveChanges();
            }
        }

        public List<ProyectoEmpleadoCE> ListarAsignaciones()
        {
            string sql = @"select p.ProyectoId, p.NombreProyecto, e.EmpleadoId, e.Nombres, e.Apellidos, pe.FechaAlta
	                        from ProyectoEmpleado pe
	                        inner join Proyecto p on pe.ProyectoId = p.ProyectoId
	                        inner join Empleado e on pe.EmpleadoId = e.EmpleadoId";

            using (var db= new dbContext())
            {
                return db.Database.SqlQuery<ProyectoEmpleadoCE>(sql).ToList();
            }
        }

        public List<ProyectoEmpleadoCE> ListarAsignaciones(int proyectoId)
        {
            string sql = @"select p.ProyectoId, p.NombreProyecto, e.EmpleadoId, e.Nombres, e.Apellidos, pe.FechaAlta
	                        from ProyectoEmpleado pe
	                        inner join Proyecto p on pe.ProyectoId = p.ProyectoId
	                        inner join Empleado e on pe.EmpleadoId = e.EmpleadoId
                            where p.ProyectoId = @ProyectoId";

            using (var db = new dbContext())
            {
                return db.Database.SqlQuery<ProyectoEmpleadoCE>(sql,
                    new SqlParameter("@ProyectoId", proyectoId)).ToList();
            }
        }

        public void Eliminarasignacion(int proyectoId, int empleadoId)
        {
            using (var db = new dbContext())
            {
                var empProy = db.ProyectoEmpleado
                    .Where(e => e.ProyectoId == proyectoId && e.EmpleadoId == empleadoId)
                    .FirstOrDefault();
                db.ProyectoEmpleado.Remove(empProy);
                db.SaveChanges();
            }
        }
    }
}
