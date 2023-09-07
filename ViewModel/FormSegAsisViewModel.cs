using Asis_Batia.Helpers;
using Asis_Batia.Model;
using Newtonsoft.Json;
using Org.Apache.Http.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
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
        public string Tipo { get; set; }

        public string PathPhoto { get; set; }
        public string PathFile { get; set; }



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
        public byte Foto { get; set; }

        private bool _isEnabled;
        private readonly IMediaPicker mediaPicker;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; OnPropertyChanged(); }
        }


        public ICommand BackPageCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoadFileCommand { get; set; }
        public ICommand PhotoCommand { get; set; }
        public ICommand SendFilesCommand { get; set; }


        public FormSegAsisViewModel(IMediaPicker mediaPicker)
        {
            BackPageCommand = new Command(async () => await BackPage());
            RegisterCommand = new Command(async () => await Register());
            LoadFileCommand = new Command(async () => await LoadFile());
            PhotoCommand = new Command(async () => await Photo());
            SendFilesCommand = new Command(async () => await SendFiles());
            this.mediaPicker = mediaPicker;
        }

        private async Task BackPage()
        {
            await Shell.Current.GoToAsync("///FormPrin");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {// aqui estamos recibiendo el diccionario que enviamos desde el form anterior
            IdCliente = (int)query["IdCliente"];// te fijas que una ves establecido el valor del cliente
            IdEmpleado = (int)query["IdEmpleado"];//lo estamos usando para hacer la peticion en el api
            IdInmueble = (int)query["IdInmueble"];// fijate que ya nos llegaron los datos
            NombreCliente = (string)query["NombreEmpleado"];
        }

        private async Task Register()
        {
            IsBusy = true;
            IsEnabled = false;
            Location _location = await LocationService.GetCurrentLocation();
            if (_location == null)
            {
                var message = LocationService.Message;
                await DisplayAlert("Mensaje", message, "Cerrar");
                return;
            }
            if (_selectionRadio == null)
            {
                await DisplayAlert("Mensaje", "Seleccione una opción de envío", "Cerrar");
                IsEnabled = true;
                IsBusy = false;
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
                RespuestaTexto = _respuestaTxt == null ? "" : _respuestaTxt,
                Tipo = Tipo,
                Foto = Foto,
            };// si te fijas ahora ya tenemos toda la info para enviar

            Uri RequestUri = new Uri("http://singa.com.mx:5500/api/RegistroBiometa");
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(registroModel);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(RequestUri, contentJson);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                IsBusy = false;
                IsEnabled = false;
                await DisplayAlert("Mensaje", "Registrado correctamente", "Ok");
                NexTPage();
            }
            else
            {
                IsBusy = false;
                IsEnabled = true;
                await DisplayAlert("Error", "Ocurrió un error al registrar", "Ok");
            }
        }

        private async void NexTPage()
        {// intentemos otra vez vale11111
            var data = new Dictionary<string, object>
            {
                {"NombreEmpleado", NombreCliente }

            };
            await Shell.Current.GoToAsync("FormReg", true, data);
        }

        private async Task GetPeriodo(int idCliente)
        {
            IsBusy = true;// aqui vamos a ver toda la info que estamos perdon obteniendo seria ya qu es get

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
                Tipo = Periodo[0].descripcion;
                IsBusy = false;

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
                        FileBase64 = ConvertToBase64(result.FullPath);
                    }
                    PathFile = result.FullPath;
                }

                //return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
        }

        private byte ConvertToBase64(string path)
        {
            byte[] ImageData = File.ReadAllBytes(path);
            byte single;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                single = (byte)fs.ReadByte();
            }
            return single;
            //byte v = ImageData;
            ////string base64String = Convert.ToBase64String(ImageData);
            //return ImageData;
        }

        private async Task Photo()
        {
            try
            {
                if (this.mediaPicker.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.CapturePhotoAsync();
                    if (photo != null)
                    {
                        string LocalFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                        using (Stream source = await photo.OpenReadAsync())
                        {
                            using FileStream localFile = File.OpenWrite(LocalFilePath);
                            await source.CopyToAsync(localFile);

                        }
                        var f = LocalFilePath;
                        PathPhoto = LocalFilePath;
                        Foto = ConvertToBase64(f);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cerrar");
            }
        }

        private async Task<string> SendFiles()
        {
            try
            {
                List<string> listaDeArchivos = new List<string>();
                string folder = "test_files";
                if(PathPhoto != null)
                    listaDeArchivos.Add(PathPhoto);
                if(PathFile != null)
                    listaDeArchivos.Add(PathFile);

                // URL del controlador ASP.NET Core
                Uri RequestUri = new Uri("http://singa.com.mx:5500/api/CargaArchivo");

                // Crear un objeto HttpClient
                var client = new HttpClient();

                // Crear contenido multipart/form-data para enviar archivos
                var content = new MultipartFormDataContent();

                // Agregar cada archivo a enviar
                foreach (var filePath in listaDeArchivos)
                {
                    var fileContent = new StreamContent(File.OpenRead(filePath));
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                    content.Add(fileContent, name:"files", fileName: Path.GetFileName(filePath));
                }

                // Agregar el encabezado 'folder' como parte de los encabezados de la solicitud
                content.Headers.Add("folder", folder); // Utiliza la variable 'folder' que deseas enviar

                // Realizar la solicitud HTTP POST
                var response = await client.PostAsync(RequestUri, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // La solicitud se realizó con éxito
                    // Puedes manejar la respuesta aquí si es necesario
                }
                else
                {
                    // Manejar otros códigos de estado si es necesario
                }
                return "";
            }
            catch (Exception ex)
            {
                // Manejar excepciones si ocurren
                return ex.Message;
            }
        }
    }
}





