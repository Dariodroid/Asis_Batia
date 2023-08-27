using Asis_Batia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asis_Batia.ViewModel
{
    public class RegExitosoViewModel : BaseViewModel, IQueryAttributable
    {
        private string _nombreCliente;

        public string NombreCliente
        {
            get { return _nombreCliente; }
            set { _nombreCliente = value; OnPropertyChanged(); }
        }

        private DateTime _fecha;

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; OnPropertyChanged(); }
        }


        public RegExitosoViewModel()
        {
            
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            NombreCliente = (string)query["NombreEmpleado"];
            Fecha = DateTime.Now;
        }
    }
}
