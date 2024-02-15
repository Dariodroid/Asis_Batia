using Asis_Batia.Model;
using Asis_Batia.View;
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
    public class FormuPrinAsisViewModel : BaseViewModel, IQueryAttributable
    {
        HttpClient client;
        #region Propiedades
        private ObservableCollection<InmuebleByIdClienteModel.InmuebleModel> _inmueble;
        //ObservableCollection Representa una recopilación de datos dinámica que proporciona notificaciones cuando se agregan o eliminan elementos, o cuando se actualiza la lista completa.

        public ObservableCollection<InmuebleByIdClienteModel.InmuebleModel> Inmueble
        {
            get { return _inmueble; }
            set { _inmueble = value; OnPropertyChanged(); }
        }

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
        private EstadoModel _idEstadoSelected;

        // Declaración de una propiedad pública llamada 'IdInmubleSelected' que encapsula la propiedad privada '_idInmubleSelected'.
        public EstadoModel IdEstadoSelected
        {
            get { return _idEstadoSelected; } // Obtener el valor de '_idInmubleSelected'.
            set
            {
                _idEstadoSelected = value;
                // Notificar que la propiedad 'IdInmubleSelected' ha cambiado.
                OnPropertyChanged();
            }
        }

        // Declaración similar para la colección observable '_empleado' y la propiedad 'Empleado'.
        private ObservableCollection<EmpleadosModel.Empleado> _empleado;
        public ObservableCollection<EmpleadosModel.Empleado> Empleado
        {
            get { return _empleado; }
            set { _empleado = value; OnPropertyChanged(); }
        }

        private ObservableCollection<EstadoModel> _estadoList;

        public ObservableCollection<EstadoModel> EstadoList
        {
            get { return _estadoList; }
            set { _estadoList = value; OnPropertyChanged(); }
        }

        #endregion

        private string _nombreEmpleado;

        public string _NombreEmpleado
        {
            get { return _nombreEmpleado; }
            set { _nombreEmpleado = value; OnPropertyChanged(); }
        }

        private string _cliente;

        public string _Cliente
        {
            get { return _cliente; }
            set { _cliente = value; OnPropertyChanged(); }
        }
        private string _puntoAtencion;

        public string _PuntoAtencion
        {
            get { return _puntoAtencion; }
            set { _puntoAtencion = value; OnPropertyChanged(); }
        }
        private string _estado;

        public string _Estado
        {
            get { return _estado; }
            set { _estado = value; OnPropertyChanged(); }
        }

        private int _idCliente;

        public int IdCliente
        {
            get { return _idCliente; }
            set { _idCliente = value; OnPropertyChanged(); }
        }

        private int _idEmpleado;

        public int IdEmpleado
        {
            get { return _idEmpleado; }
            set { _idEmpleado = value; }
        }
        private int _idInmueble;

        public int IdInmueble
        {
            get { return _idInmueble; }
            set { _idInmueble = value; }
        }

        private int _idEstado;

        public int IdEstado
        {
            get { return _idEstado; }
            set { _idEstado = value; }
        }



        public ICommand NextPageCommand { get; set; }

        // Constructor de la clase FormuPrinAsisViewModel.
        public FormuPrinAsisViewModel()
        {
            // Crear una nueva instancia de HttpClient llamada 'client'.
            client = new HttpClient();

            // Llamar al método 'GetClients' para obtener la información de los clientes.
            //GetClients();
            NextPageCommand = new Command(async () => await NextPage());
            _ = GetEstado();

        }


        // Método asincrónico para obtener información de inmuebles por ID de cliente.
        private async Task GetInmuebleByIdClient(int idCliente)
        {
            IsBusy = true;

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
                IsBusy = false;

            }
        }

        private async Task GetEstado()
        {
            IsBusy = true;

            // Verificar si la colección 'Inmueble' no es nula y, si no lo es, limpiarla.
            if (Inmueble != null)
                Inmueble.Clear();

            // Crear una solicitud HTTP.
            var request = new HttpRequestMessage();

            // Establecer la URL de la solicitud con el ID de cliente proporcionado.
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/Estado");

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
                var data = JsonConvert.DeserializeObject<ObservableCollection<EstadoModel>>(content);

                // Asignar la colección de inmuebles a la propiedad 'Inmueble'.
                EstadoList = data;
                IsBusy = false;

            }
        }

        private async Task NextPage()
        {
            if (IdCliente == 130)
            {
                await DisplayAlert("Advertencia","Estimado colaborador, recuerda que tu asistencia debe de pasar por la aplicación\r\nGS Proveedores.\r\nPara mayor información, comuníquese a los siguientes números:\r\n55 5297 9939 \r\n55 4333 0123 ext.133", "Cerrar");
                Exit();
            }

            else
            {

                string latitud = null;
                string longitud = null;

                foreach (var inmueble in Inmueble)
                {
                    if (inmueble.id_inmueble == IdInmueble)
                    {
                        latitud = inmueble.latitud;
                        longitud = inmueble.longitud;
                        break; // Sal del bucle una vez que encuentres la coincidencia
                    }
                }
                var data = new Dictionary<string, object>
            {
                {"IdCliente", IdCliente },// ahora ya deberia funcionar
                {"IdInmueble", IdInmueble },// el error se produce aqui xq no hay nada en el idinmueble
                {"IdEmpleado", IdEmpleado },// ahora veamos si es que la api del main no da ese dato
                {"NombreEmpleado", _NombreEmpleado },
                {"Lat", latitud},
                {"Lng", longitud}
            };

                await Shell.Current.GoToAsync("//FormSeg", true, data);//te fijas que no se necesita legable de inmueble
            }//ya tenemos toda la informacion de ese usuario y ahora se la pasamos al siguiente form
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _NombreEmpleado = (string)query["Empleado"];
            _Cliente = (string)query["Cliente"];
            _PuntoAtencion = (string)query["PuntoAtencion"];
            _Estado = (string)query["Estado"];
            IdCliente = (int)query["IdCliente"];
            IdEmpleado = (int)query["idEmpleado"];
            IdInmueble = (int)query["idInmueble"];
            IdEstado = (int)query["idEstado"];
            _ = IdCliente > 0 ? GetInmuebleByIdClient(IdCliente) : null;
        }
        private void Exit()
        {
            Application.Current.Quit();
        }
    }
}
