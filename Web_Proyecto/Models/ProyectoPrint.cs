using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Proyecto.Models
{
    public class ProyectoPrint
    {
        public int ProyectoId { get; set; }
        public string NombreProyecto { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}