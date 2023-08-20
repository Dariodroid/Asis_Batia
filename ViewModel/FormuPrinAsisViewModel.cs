using Asis_Batia.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asis_Batia.ViewModel
{
    public class FormuPrinAsisViewModel : BaseViewModel
    {
        HttpClient client;
        private ObservableCollection<ClientModel.Client> _clients;

        public ObservableCollection<ClientModel.Client> Clients
        {
            get { return _clients; }
            set { _clients = value; OnPropertyChanged(); }
        }

        private ObservableCollection<InmuebleByIdClienteModel.InmuebleModel> _inmueble;

        public ObservableCollection<InmuebleByIdClienteModel.InmuebleModel> Inmueble
        {
            get { return _inmueble; }
            set { _inmueble = value; OnPropertyChanged(); }
        }

        private ClientModel.Client _idSelected;

        public ClientModel.Client IdSelected
        {
            get { return _idSelected; }
            set
            {
                if (_idSelected != value)
                {
                    _idSelected = value;
                    OnPropertyChanged();
                    GetInmuebleByIdClient(_idSelected.idCliente);
                }
            }
        }

        private ObservableCollection<EmpleadosModel.Empleado> _empleado;

        public ObservableCollection<EmpleadosModel.Empleado> Empleado
        {
            get { return _empleado; }
            set { _empleado = value; OnPropertyChanged(); }
        }

        private InmuebleByIdClienteModel.InmuebleModel _idInmubleSelected;

        public InmuebleByIdClienteModel.InmuebleModel IdInmubleSelected
        {
            get { return _idInmubleSelected; }
            set
            {
                if (_idInmubleSelected != value)
                {
                    _idInmubleSelected = value;
                    OnPropertyChanged();
                    GetEmpleadoByIdInmueble(_idInmubleSelected.id_inmueble);
                }
            }
        }


        public FormuPrinAsisViewModel()
        {
            client = new HttpClient();
            GetClients();
        }

        private async Task GetClients()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://singa.com.mx:5500/api/cliente");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ObservableCollection<ClientModel.Client>>(content);
                Clients = data;
            }
        }

        private async Task GetInmuebleByIdClient(int idCliente)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/Inmueble?idcliente={idCliente}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ObservableCollection<InmuebleByIdClienteModel.InmuebleModel>>(content);
                Inmueble = data;
            }
        }

        private async Task GetEmpleadoByIdInmueble(int idInmueble)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/Empleados?Idinmueble={idInmueble}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ObservableCollection<EmpleadosModel.Empleado>>(content);
                Empleado = data;
            }
        }

    }
}
