using Asis_Batia.Helpers;
using Asis_Batia.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asis_Batia.ViewModel
{
    internal class SelectInmuebleViewModel : BaseViewModel, IQueryAttributable
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string NombreEmpleado { get; set; }
        public int IdEmpleado { get; set; }
        public int IdInmueble { get; set; }
        public int IdPeriodo { get; set; }
        public string Tipo { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }


        HttpClient client;
        // Declaración de una colección observable privada llamada '_clients' del tipo 'ClientModel.Client'.
        private ObservableCollection<ClientModel.Client> _clients;

        // Declaración de una propiedad pública llamada 'Clients' que encapsula la colección observable privada '_clients'.
        public ObservableCollection<ClientModel.Client> Clients
        {
            get { return _clients; } // Obtener la colección '_clients'.
            set { _clients = value; OnPropertyChanged(); } // Asignar valor a '_clients' y notificar que la propiedad 'Clients' ha cambiado.
        }

        // Declaración similar para la colección observable '_inmueble' y la propiedad 'Inmueble'.
        private ObservableCollection<InmuebleByIdClienteModel.InmuebleModel> _inmueble;
        public ObservableCollection<InmuebleByIdClienteModel.InmuebleModel> Inmueble
        {
            get { return _inmueble; }
            set { _inmueble = value; OnPropertyChanged(); }
        }

        // Declaración similar para la colección observable '_empleado' y la propiedad 'Empleado'.
        //private ObservableCollection<EmpleadosModel.Empleado> _empleado;
        //public ObservableCollection<EmpleadosModel.Empleado> Empleado
        //{
        //    get { return _empleado; }
        //    set { _empleado = value; OnPropertyChanged(); }
        //}

        // Declaración de una propiedad privada llamada '_idSelected' del tipo 'ClientModel.Client'.
        private ClientModel.Client _idClientSelected;

        // Declaración de una propiedad pública llamada 'IdSelected' que encapsula la propiedad privada '_idSelected'.
        public ClientModel.Client IdClientSelected
        {
            get { return _idClientSelected; } // Obtener el valor de '_idSelected'.
            set
            {
                // Comprobar si el valor nuevo es diferente del valor actual.
                if (_idClientSelected != value)
                {
                    // Asignar el valor nuevo a '_idSelected'.
                    _idClientSelected = value;

                    // Notificar que la propiedad 'IdSelected' ha cambiado.
                    OnPropertyChanged();

                    // Llamar al método 'GetInmuebleByIdClient' y pasar el ID del cliente seleccionado.
                    GetInmuebleByIdClient(_idClientSelected.idCliente);
                }
            }
        }


        // Declaración de una propiedad privada llamada '_idInmubleSelected' del tipo 'InmuebleByIdClienteModel.InmuebleModel'.
        private InmuebleByIdClienteModel.InmuebleModel _idInmubleSelected;

        // Declaración de una propiedad pública llamada 'IdInmubleSelected' que encapsula la propiedad privada '_idInmubleSelected'.
        public InmuebleByIdClienteModel.InmuebleModel IdInmubleSelected
        {
            get { return _idInmubleSelected; } // Obtener el valor de '_idInmubleSelected'.
            set
            {
                // Comprobar si el valor nuevo es diferente del valor actual y no es nulo.
                if (_idInmubleSelected != value && value != null)
                {
                    // Asignar el valor nuevo a '_idInmubleSelected'.
                    _idInmubleSelected = value;

                    // Notificar que la propiedad 'IdInmubleSelected' ha cambiado.
                    OnPropertyChanged();

                    // Llamar al método 'GetEmpleadoByIdInmueble' y pasar el ID del inmueble seleccionado.
                    //GetEmpleadoByIdInmueble(_idInmubleSelected.id_inmueble);
                }
            }
        }

        // Declaración de una propiedad privada llamada '_idInmubleSelected' del tipo 'InmuebleByIdClienteModel.InmuebleModel'.
        //private EmpleadosModel.Empleado _idEmpleadoSelected;

        //// Declaración de una propiedad pública llamada 'IdInmubleSelected' que encapsula la propiedad privada '_idInmubleSelected'.
        //public EmpleadosModel.Empleado IdEmpleadoSelected
        //{
        //    get { return _idEmpleadoSelected; } // Obtener el valor de '_idInmubleSelected'.
        //    set
        //    {
        //        _idEmpleadoSelected = value;
        //        // Notificar que la propiedad 'IdInmubleSelected' ha cambiado.
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand NextPageCommand { get; set; }

        // Constructor de la clase FormuPrinAsisViewModel.
        public SelectInmuebleViewModel()
        {
            // Crear una nueva instancia de HttpClient llamada 'client'.
            client = new HttpClient();

            GetClients();
            // Llamar al método 'GetClients' para obtener la información de los clientes.
            NextPageCommand = new Command(async () => await NextPage());
        }

        // Método asincrónico para obtener la información de los clientes.
        private async Task GetClients()
        {
            // Crear una solicitud HTTP.
            var request = new HttpRequestMessage();

            // Establecer la URL de la solicitud.
            request.RequestUri = new Uri("http://singa.com.mx:5500/api/cliente");

            // Establecer el método de la solicitud como GET.
            request.Method = HttpMethod.Get;

            // Agregar un encabezado "Accept" para indicar que se acepta JSON como respuesta.
            request.Headers.Add("Accept", "application/json");

            // Crear una nueva instancia de HttpClient.
            var client = new HttpClient();

            // Enviar la solicitud HTTP y esperar la respuesta.
            HttpResponseMessage response = await client.SendAsync(request);

            // Verificar si la respuesta tiene un estado OK (código 200).
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Leer el contenido de la respuesta como una cadena.
                string content = await response.Content.ReadAsStringAsync();

                // Deserializar el contenido JSON en una colección observable de clientes.
                var data = JsonConvert.DeserializeObject<ObservableCollection<ClientModel.Client>>(content);

                // Asignar la colección de clientes a la propiedad 'Clients'.
                Clients = data;
            }
        }

        // Método asincrónico para obtener información de inmuebles por ID de cliente.
        private async Task GetInmuebleByIdClient(int idCliente)
        {
            // Verificar si la colección 'Inmueble' no es nula y, si no lo es, limpiarla.
            if (Inmueble != null)
                Inmueble.Clear();

            // Crear una solicitud HTTP.
            var request = new HttpRequestMessage();

            // Establecer la URL de la solicitud con el ID de cliente proporcionado.
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/Inmueble?idcliente={idCliente}");

            // Establecer el método de la solicitud como GET.
            request.Method = HttpMethod.Get;

            // Agregar un encabezado "Accept" para indicar que se acepta JSON como respuesta.
            request.Headers.Add("Accept", "application/json");

            // Crear una nueva instancia de HttpClient.
            var client = new HttpClient();

            // Enviar la solicitud HTTP y esperar la respuesta.
            HttpResponseMessage response = await client.SendAsync(request);

            // Verificar si la respuesta tiene un estado OK (código 200).
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Leer el contenido de la respuesta como una cadena.
                string content = await response.Content.ReadAsStringAsync();

                // Deserializar el contenido JSON en una colección observable de inmuebles.
                var data = JsonConvert.DeserializeObject<ObservableCollection<InmuebleByIdClienteModel.InmuebleModel>>(content);

                // Asignar la colección de inmuebles a la propiedad 'Inmueble'.
                Inmueble = data;

            }
        }

        // Método asincrónico para obtener información de empleados por ID de inmueble.
        //private async Task GetEmpleadoByIdInmueble(int idInmueble)
        //{
        //    // Crear una solicitud HTTP.
        //    var request = new HttpRequestMessage();

        //    // Establecer la URL de la solicitud con el ID de inmueble proporcionado.
        //    request.RequestUri = new Uri($"http://singa.com.mx:5500/api/Empleados?Idinmueble={idInmueble}");

        //    // Establecer el método de la solicitud como GET.
        //    request.Method = HttpMethod.Get;

        //    // Agregar un encabezado "Accept" para indicar que se acepta JSON como respuesta.
        //    request.Headers.Add("Accept", "application/json");

        //    // Crear una nueva instancia de HttpClient.
        //    var client = new HttpClient();

        //    // Enviar la solicitud HTTP y esperar la respuesta.
        //    HttpResponseMessage response = await client.SendAsync(request);

        //    // Verificar si la respuesta tiene un estado OK (código 200).
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        // Leer el contenido de la respuesta como una cadena.
        //        string content = await response.Content.ReadAsStringAsync();

        //        // Deserializar el contenido JSON en una colección observable de empleados.
        //        var data = JsonConvert.DeserializeObject<ObservableCollection<EmpleadosModel.Empleado>>(content);

        //        // Asignar la colección de empleados a la propiedad 'Empleado'.
        //        Empleado = data;
        //    }
        //}

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            NombreEmpleado = (string)query["NombreEmpleado"];
            IdEmpleado = (int)query["IdEmpleado"];
            Lat = (string)query["Lat"];
            Lng = (string)query["Lng"];
            IdInmueble = (int)query["IdInSele"];
            IdCliente = (int)query["IdClSele"];
        }

        private async Task NextPage()
        {
            try
            {
                IsBusy = true;
                if (await CheckLocation())
                {
                    var data = new Dictionary<string, object>
                    {
                        {"IdCliente",IdCliente},
                        {"IdInmueble",IdInmueble},
                        {"IdEmpleado", IdEmpleado },
                        {"NombreEmpleado", NombreEmpleado },
                        {"Lat", IdInmubleSelected.latitud},
                        {"Lng", IdInmubleSelected.longitud},
                    };
                    IsBusy = false;
                    await Shell.Current.GoToAsync("//FormSeg", true, data);
                    }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Ocurrió un error al obtener los datos, verifique que todos los campos estén seleccionados o contacte al administrador", "Cerrar");
                return;
            }
        }

      
        private async Task<bool> CheckLocation()
        {
            CultureInfo culture = new CultureInfo("es-MX");
            Location CurrentLocation = await LocationService.GetCurrentLocation();
            Location TargetDestination = new Location(double.Parse(IdInmubleSelected.latitud, culture), double.Parse(IdInmubleSelected.longitud, culture));
            if (CurrentLocation == null)
            {
                var message = LocationService.Message;
                await DisplayAlert("Mensaje", message, "Cerrar");
                return false;
            }
            if (string.IsNullOrEmpty(IdInmubleSelected.latitud) || string.IsNullOrEmpty(IdInmubleSelected.longitud))
            {
                await DisplayAlert("Alerta", "No se encontraron coordenadas del inmueble seleccionado", "Cerrar");
                return false;
            }
            if (Math.Round(LocationService.CalcularDistancia(CurrentLocation, TargetDestination) * 1000, 2) > 60)//COMPROBAMOS QUE LA DISTANCIA NO SEA MAYOR A 100CM QUE EQUIVALE A 1 METRO, SI NECESITAS CAMBIAR LA DISTANCIA A COMPAR DEBES PONER EN CM LA DISTANCIA
            {
                IsBusy = false;
                await DisplayAlert("Acción no permitida", "Está muy lejos del área seleccionada", "Cerrar");
                return false;
            }
            return true;
        }

    }
}

