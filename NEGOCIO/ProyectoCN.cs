using DATOS;
using ENTIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEGOCIO
{
    public class ProyectoCN
    {
        private static ProyectoDALC obj = new ProyectoDALC();

        public static void Agregar(Proyecto pcto)
        {
            obj.Agregar(pcto);
        }

        public static List<Proyecto> ListarProyectos()
        {
            return obj.ListarProyectos().ToList();
        }

        public static Proyecto GetProyectos(int id)
        {
            return obj.GetProyectos(id);
        }

        public static void Editar(Proyecto pcto)
        {
            obj.Editar(pcto);
        }
        public static void Eliminar(int id)
        {
            obj.Eliminar(id);
        }
    }
}
