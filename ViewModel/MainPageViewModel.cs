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
    public class MainPageViewModel : BaseViewModel
    {
        private string _idEmpleado;

        public string IdEmpleado
        {
            get { return _idEmpleado; }
            set { _idEmpleado = value; OnPropertyChanged(); }
        }

        private ObservableCollection<InfoEmpleadoModel> _infoEmpleado;

        public ObservableCollection<InfoEmpleadoModel> InfoEmpleado
        {
            get { return _infoEmpleado; }
            set { _infoEmpleado = value; }
        }

        public ICommand GetInfoEmpleadoCommand { get; set; }


        public MainPageViewModel()
        {
            int UID = Convert.ToInt32(Preferences.Get("UserId", 0));
            if (UID > 0)
            {
                IdEmpleado = UID.ToString();
                _ = GetInfoEmpleado();
            }
            else
            {
                GetInfoEmpleadoCommand = new Command(async () => await GetInfoEmpleado());
            }
        }

        private async Task GetInfoEmpleado()
        {

            IsBusy = true;
            if (string.IsNullOrEmpty(IdEmpleado) || IdEmpleado == "0")
            {
                await DisplayAlert("Error", "Ingrese su ID", "Cerrar");
                return;
            }

            // Crear una solicitud HTTP.
            var request = new HttpRequestMessage();

            // Establecer la URL de la solicitud.
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/EmpleadoApp?idempleado={IdEmpleado}");

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
                var data = JsonConvert.DeserializeObject<ObservableCollection<InfoEmpleadoModel>>(content);
                InfoEmpleado = data;
                IsBusy = false;
                if (InfoEmpleado.Count > 0)
                {
                    Preferences.Set("UserId", InfoEmpleado[0].idEmpleado);
                    await NextPage();
                }
                else
                {
                    await DisplayAlert("Error", "No se encontró ningún usuario", "Cerrar");
                }
            }
        }

        private async Task NextPage()
        {
            var data = new Dictionary<string, object>
            {
                {"Empleado", InfoEmpleado[0].empleado },
                {"Cliente", InfoEmpleado[0].cliente },
                {"PuntoAtencion", InfoEmpleado[0].puntoAtencion },
                {"Estado", InfoEmpleado[0].estado },
                {"IdCliente", InfoEmpleado[0].idCliente },
                {"idEmpleado", InfoEmpleado[0].idEmpleado },
                {"idInmueble", InfoEmpleado[0].idInmueble },
                {"idEstado", InfoEmpleado[0].idEstado }


            };
            await Shell.Current.GoToAsync("//FormPrin", true, data);
        }
    }
}
