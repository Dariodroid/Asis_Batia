using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asis_Batia.Model
{
    public class ClientModel
    {
        public Client[] ClientList { get; set; }


        public class Client
        {
            public int idCliente { get; set; }
            public string nombre { get; set; }
        }

    }
}
