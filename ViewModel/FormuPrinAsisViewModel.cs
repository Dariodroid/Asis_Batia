﻿using Asis_Batia.Model;
using Asis_Batia.View;
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
        private ObservableCollection<EmpleadosModel.Empleado> _empleado;
        public ObservableCollection<EmpleadosModel.Empleado> Empleado
        {
            get { return _empleado; }
            set { _empleado = value; OnPropertyChanged(); }
        }

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
                    GetEmpleadoByIdInmueble(_idInmubleSelected.id_inmueble);
                }
            }
        }

        // Declaración de una propiedad privada llamada '_idInmubleSelected' del tipo 'InmuebleByIdClienteModel.InmuebleModel'.
        private EmpleadosModel.Empleado _idEmpleadoSelected;

        // Declaración de una propiedad pública llamada 'IdInmubleSelected' que encapsula la propiedad privada '_idInmubleSelected'.
        public EmpleadosModel.Empleado IdEmpleadoSelected
        {
            get { return _idEmpleadoSelected; } // Obtener el valor de '_idInmubleSelected'.
            set
            {
                _idEmpleadoSelected = value;
                // Notificar que la propiedad 'IdInmubleSelected' ha cambiado.
                OnPropertyChanged();
            }
        }

        public ICommand NextPageCommand { get; set; }

        // Constructor de la clase FormuPrinAsisViewModel.
        public FormuPrinAsisViewModel()
        {
            // Crear una nueva instancia de HttpClient llamada 'client'.
            client = new HttpClient();

            // Llamar al método 'GetClients' para obtener la información de los clientes.
            GetClients();
            NextPageCommand = new Command(async () => await NextPage());
        }

        // Método asincrónico para obtener la información de los clientes.
        private async Task GetClients()
        {
            IsBusy = true;
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
                IsBusy = false;
            }
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

        // Método asincrónico para obtener información de empleados por ID de inmueble.
        private async Task GetEmpleadoByIdInmueble(int idInmueble)
        {
            IsBusy = true;

            // Crear una solicitud HTTP.
            var request = new HttpRequestMessage();

            // Establecer la URL de la solicitud con el ID de inmueble proporcionado.
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/Empleados?Idinmueble={idInmueble}");

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

                // Deserializar el contenido JSON en una colección observable de empleados.
                var data = JsonConvert.DeserializeObject<ObservableCollection<EmpleadosModel.Empleado>>(content);

                // Asignar la colección de empleados a la propiedad 'Empleado'.
                Empleado = data;
                IsBusy = false;

            }
        }

        private async Task NextPage()
        {
            var data = new Dictionary<string, object>
            {
                {"IdCliente", _idClientSelected.idCliente },
                {"IdInmueble", _idInmubleSelected.id_inmueble },
                {"IdEmpleado", _idEmpleadoSelected.id_empleado },
                {"NombreEmpleado", _idEmpleadoSelected.empleado }
            };
            await Shell.Current.GoToAsync("FormSeg", true, data);
        }
    }
}
