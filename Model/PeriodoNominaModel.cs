using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asis_Batia.Model
{
    public class PeriodoNominaModel
    {
        public PeriodoClient[] PeriCli { get; set; }

        public class PeriodoClient
        {
            public int id_empleado { get; set; }
            public int id_periodo { get; set; }
            public string descripcion { get; set; }
            public int anio { get; set; }
            public string Idperiodo { get; set; }
        }

    }
}
