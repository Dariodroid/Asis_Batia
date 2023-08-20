using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asis_Batia.Model
{
    public class EmpleadosModel
    {

        public class Rootobject
        {
            public Empleado[] EmpleadoList { get; set; }
        }

        public class Empleado
        {
            public int id_empleado { get; set; }
            public string empleado { get; set; }
        }

    }
}
