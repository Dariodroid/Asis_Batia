using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asis_Batia.Model
{
    public class InfoEmpleadoModel
    {
        public int idEmpleado { get; set; }
        public int idCliente { get; set; }
        public int idInmueble { get; set; }
        public int idEstado { get; set; }
        public string empleado { get; set; }
        public string cliente { get; set; }
        public string puntoAtencion { get; set; }
        public string estado { get; set; }
    }
}
