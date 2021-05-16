using ENTIDAD;
using System;
using System.Collections.Generic;
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
                return db.Proyecto.ToList();
            }
        }

        public Proyecto GetProyectos(int id)
        {
            using (var db = new dbContext())
            {
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
    }
}
