﻿using Asis_Batia.Helpers;
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
    public class FormSegAsisViewModel : BaseViewModel, IQueryAttributable
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public int IdEmpleado { get; set; }
        public int IdInmueble { get; set; }
        public int IdPeriodo { get; set; }


        public string _selectionRadio;

        public string SelectionRadio
        {
            get { return _selectionRadio; }
            set { _selectionRadio = value; OnPropertyChanged(); }
        }

        private string _respuestaTxt;

        public string RespuestaTxt
        {
            get { return _respuestaTxt; }
            set { _respuestaTxt = value; OnPropertyChanged(); }
        }

        private ObservableCollection<PeriodoNominaModel.PeriodoClient> _periodo;

        public ObservableCollection<PeriodoNominaModel.PeriodoClient> Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }


        public byte FileBase64 { get; set; }

        public ICommand BackPageCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoadFileCommand { get; set; }


        public FormSegAsisViewModel()
        {
            BackPageCommand = new Command(async () => await BackPage());
            RegisterCommand = new Command(async () => await Register());
            LoadFileCommand = new Command(async () => await LoadFile());
        }

        private async Task BackPage()
        {
            await Shell.Current.GoToAsync("///FormPrin");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            IdCliente = (int)query["IdCliente"];
            IdEmpleado = (int)query["IdEmpleado"];
            IdInmueble = (int)query["IdInmueble"];
            NombreCliente = (string)query["NombreEmpleado"];
        }

        private async Task Register()
        {
           Location _location = await LocationService.GetCurrentLocation();
            if (_location == null)
            {
                var message = LocationService.Message;
                await DisplayAlert("Mensaje", message, "Cerrar");
                return;
            }

            await GetPeriodo(IdCliente);

            RegistroModel registroModel = new RegistroModel
            {
                Adjuntos = FileBase64,
                Anio = (int)DateTime.Today.Year,
                Confirma = "App",
                Cubierto = 0,
                Fecha = DateTime.Now,
                Idempleado = IdEmpleado,
                Idinmueble = IdInmueble,
                Idperiodo = IdPeriodo,
                Latitud = _location.Latitude.ToString(),
                Longitud = _location.Longitude.ToString(),
                Movimiento = _selectionRadio,
                RespuestaTexto = _respuestaTxt,
                Tipo = "",
            };

            Uri RequestUri = new Uri("http://singa.com.mx:5500/api/RegistroBiometa");
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(registroModel);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(RequestUri, contentJson);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("Mensaje", "Registrado correctamente", "Ok");
                NexTPage();
            }
            else
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar", "Ok");
            }
        }

        private async void NexTPage()
        {
            var data = new Dictionary<string, object>
            {
                {"NombreEmpleado", NombreCliente }

            };
            await Shell.Current.GoToAsync("FormReg", true, data);
        }

        private async Task GetPeriodo(int idCliente)
        {
            // Crear una solicitud HTTP.
            var request = new HttpRequestMessage();

            // Establecer la URL de la solicitud.
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/PeriodoNomina?Idempleado={idCliente}");

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
                var data = JsonConvert.DeserializeObject<ObservableCollection<PeriodoNominaModel.PeriodoClient>>(content);

                // Asignar la colección de clientes a la propiedad 'Clients'.
                Periodo = data;
                IdPeriodo = Periodo[0].id_periodo;
            }
        }


        private async Task LoadFile()
        {
            try
            {
                PickOptions options = new PickOptions();
                options.FileTypes = FilePickerFileType.Pdf;

                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("doc", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        var file = ImageSource.FromStream(() => stream);
                        FileBase64 = ConvertImgToBase64(result.FullPath);
                    }
                }

                //return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
        }

        private byte ConvertImgToBase64(string path)
        {
            byte[] ImageData = File.ReadAllBytes(path);
            byte single;
            using(FileStream fs = new FileStream(path, FileMode.Open))
            {
                single = (byte)fs.ReadByte();
            }
            return single;
            //byte v = ImageData;
            ////string base64String = Convert.ToBase64String(ImageData);
            //return ImageData;
        }
    }
}




