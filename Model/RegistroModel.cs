using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asis_Batia.Model
{
    public class RegistroModel
    {
        public int Idperiodo { get; set; }
        public int Anio { get; set; }
        public string Tipo { get; set; }
        public int Idinmueble { get; set; }
        public int Idempleado { get; set; }
        public string Confirma { get; set; }
        public DateTime Fecha { get; set; }
        public string Movimiento { get; set; }
        public int Cubierto { get; set; }
        public string Adjuntos { get; set; }
        public string RespuestaTexto { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Foto { get; set; }

    }
}
