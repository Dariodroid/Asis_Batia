
using Asis_Batia.Helpers;
using Asis_Batia.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui.Devices.Sensors;
using System.Globalization;
//using static CoreFoundation.DispatchSource;

namespace Asis_Batia.ViewModel
{
    public class FormSegAsisViewModel : BaseViewModel, IQueryAttributable
    {
        int count = 0;
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public int IdEmpleado { get; set; }
        public int IdInmueble { get; set; }
        public int IdPeriodo { get; set; }
        public string Tipo { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
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
        public string UrlFiles { get; set; }

        List<string> archivos = new List<string>();



        public ICommand BackPageCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoadFileCommand { get; set; }
        public ICommand PhotoCommand { get; set; }


        public FormSegAsisViewModel(IMediaPicker mediaPicker)
        {
            BackPageCommand = new Command(async () => await BackPage());
            RegisterCommand = new Command(async () => await Register());
            LoadFileCommand = new Command(async () => await LoadFile());
            PhotoCommand = new Command(async () => await Photo());
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
            Lat = (string)query["Lat"];
            Lng = (string)query["Lng"];
        }

        private void existeRegistro(int idEmple, DateTime fech, string movi)
        {
            string sql = "";

            sql = "select * from tb_empleado_asistencia_op where id_empleado=" + idEmple + " and CONVERT(DATE, fecha)='" + fech + "' and movimiento='" + movi + "'";
        }

        private async Task Register()
        {
            try
            {
                IsEnabled = false;
                count = 0;
                IsBusy = true;
                CultureInfo culture = new CultureInfo("es-MX");// Establece la cultura adecuada para México
                Location _location = await LocationService.GetCurrentLocation();
                double latitud = double.Parse(Lat, culture);
                double longitud = double.Parse(Lng, culture);
                Location TargetDestination = new Location(latitud, longitud);// new Location(19.42857127110338, -99.16356656825693);EJEMPLO DEL PUNTO OBJETIVO DEL INMUEBLE AQUI DEBEN IR LAS COORDENADAS QUE DA EL API
                Location CurrentLocation = _location;// AQUI DEBE IR LAS COORDENADAS ACTUALES DEL GPS DEL MOVIL LAS CUALES LAS TENEMOS EN LA VARIABLE _location EN LA LINEA 109
                //AL FINAL DEBERIA QUEDARTE DE LA SIGUIENTE FORMA PARA OBTENER LA UBICACION ACTUAL DEL MOVIL:Location CurrentLocation = _location;

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
                if (_selectionRadio == "A")
                {
                    if (Math.Round(LocationService.CalcularDistancia(CurrentLocation, TargetDestination) * 1000, 2) > 400)//COMPROBAMOS QUE LA DISTANCIA NO SEA MAYOR A 100CM QUE EQUIVALE A 1 METRO, SI NECESITAS CAMBIAR LA DISTANCIA A COMPAR DEBES PONER EN CM LA DISTANCIA
                    {//EL METODO CalcularDistancia() YA ME REGRESA UN VALOR CALCULADO EN KM X LO CUAL SE DEBE CONVERTIR A METROS
                     //Y ES POR ELLO QUE SE MULTIPLICA POR 1000 QUE SERIA 1KM Y LA CLASE MATH.ROUND ES PARA REDONDEAR LOS DECIMALES DE LOS METROS EN ESTE CASO A 2 DECIMALES
                        if (count == 0)
                        {
                          
                            count++;
                            var result = await DisplayAlert("Acción no permitida", "Parece que estas lejos de tu servicio, ¿Deseas registrarte en otro servicio?", "Si", "No");
                            if (result)
                            {
                                var data = new Dictionary<string, object>
                                {
                                    {"NombreEmpleado", NombreCliente },
                                    {"IdEmpleado", IdEmpleado },
                                    {"Lat", Lat},
                                    {"Lng", Lng},
                                    {"IdInSele",IdInmueble},
                                    {"IdClSele",IdCliente}
                                };
                                await Shell.Current.GoToAsync("//SelectInmu", true, data);

                            }
                            else
                            {
                                count = 0;
                                IsBusy = false;
                                IsEnabled = true;
                                return;
                            }
                            IsBusy = false;
                            IsEnabled = true ;
                            return;
                        }
                    }
                }

                else if (_selectionRadio == "N")
                { }

                if (await SendFiles())
                { }
                else
                {
                    await DisplayAlert("Error", "No fué posible guardar los archivos", "Cerrar");
                    IsEnabled = true;
                    IsBusy = false;
                    return;
                }
            
                    IsEnabled = false; 
                await GetPeriodo(IdEmpleado);
                RegistroModel registroModel = new RegistroModel
                {
                    Adjuntos = PathFile == null ? "" : PathFile,
                    Anio = (int)DateTime.Today.Year,
                    Confirma = "BIOMETA",
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
                    Foto = PathPhoto == null ? "" : PathPhoto,
                };

                Uri RequestUri = new Uri("http://singa.com.mx:5500/api/RegistroBiometa");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(registroModel);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    IsBusy = false;
                    IsEnabled = false;
                    await DisplayAlert("Mensaje", "Registrado correctamente", "Ok");
                    NexTPage();
                }
                    //else if (content == "err1")
                    //{
                    //    IsBusy = false;
                    //    IsEnabled = true;
                    //    await DisplayAlert("Mensaje", "Su registro fue exitoso 😊", "Ok");
                    //}
                    //else
                else 
                {
                    IsBusy = false;
                    IsEnabled = true;
                    await DisplayAlert("Error", "Ocurrió un error al registrar", "Ok");
                }
         }
            catch (Exception ex)
            {
                IsBusy = false;
                await DisplayAlert("Error", ex.Message, "Cerrar");
            }
        }


        // Esta función recibe dos objetos de tipo Location, que representan las coordenadas de los puntos
        // y devuelve la distancia en metros entre ellos, usando el método CalculateDistance de la clase Location

        private async void NexTPage()
        {// intentemos otra vez vale11111
            var data = new Dictionary<string, object>
            {
                {"NombreEmpleado", NombreCliente }

            };
            await Shell.Current.GoToAsync("//FormReg", true, data);
        }

        private async Task GetPeriodo(int IdEmpleado)
        {
            IsBusy = true;// aqui vamos a ver toda la info que estamos perdon obteniendo seria ya qu es get

            // Crear una solicitud HTTP.
            var request = new HttpRequestMessage();

            // Establecer la URL de la solicitud.
            request.RequestUri = new Uri($"http://singa.com.mx:5500/api/PeriodoNomina?Idempleado={IdEmpleado}");

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

                var resultOptions = await FilePicker.Default.PickMultipleAsync(options);
                if (resultOptions != null)
                {
                    foreach (var result in resultOptions)
                    {
                        if (result.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase) ||
                            result.FileName.EndsWith("doc", StringComparison.OrdinalIgnoreCase))
                        {
                            using var stream = await result.OpenReadAsync();
                            var file = ImageSource.FromStream(() => stream);
                            FileBase64 = ConvertToBase64(result.FullPath);
                        }
                        PathFile = result.FullPath;
                        archivos.Add(PathFile);
                    }
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

        public async Task<bool> SendFiles()
        {
            if (PathPhoto != null)
                archivos.Add(PathPhoto);
            string urlarch = string.Empty;
            foreach (var item in archivos)
            {
                urlarch += item;
            }
            //await DisplayAlert("Url que se envían al Servidor", $"{urlarch}", "Ok");

            UrlFiles = await UploadFiles(archivos, "Doctos");
            string[] splits = UrlFiles.Split("|");// AQUI DEBEMOS INCLUIR EL SIGNO "|" SIN ESPAICIOS
            PathFile = string.Empty;
            foreach (string split in splits)
            {
                if (split.Contains(".pdf"))
                {
                    // Si la extensión es PDF, asigna a pathFile y rompe el bucle
                    PathFile += $"{split}|";
                }
                else if (split.Contains(".jpg") || split.Contains(".jpeg") || split.Contains(".png"))
                {
                    // Si es una imagen (JPG, JPEG o PNG), asigna a pathPhoto y continúa el bucle
                    PathPhoto = split;
                }
            }
            var EndPath = PathFile.TrimEnd('|');
            PathFile = EndPath;
            //await DisplayAlert("Url que se reciben del Servidor", $"{UrlFiles}", "Ok");
            return true;
        }

        // Se crea una instancia de HttpClient que se puede reutilizar
        private static readonly HttpClient client = new HttpClient();

        // Este método envía una lista de archivos y un nombre de carpeta al API
        public async Task<string> UploadFiles(List<string> files, string folderName)
        {
            // Se crea un objeto MultipartFormDataContent para contener los datos del formulario
            var formData = new MultipartFormDataContent();

            // Se agrega el nombre de la carpeta como un parámetro de cadena
            formData.Add(new StringContent(folderName), "folderName");

            // Se recorre la lista de archivos y se agregan como parámetros de archivo
            foreach (var file in files)
            {
                // Se crea un objeto StreamContent a partir del contenido del archivo
                var fileContent = new StreamContent(File.OpenRead(file));

                // Se agrega el contenido del archivo al formulario con el nombre del archivo
                formData.Add(fileContent, "files", Path.GetFileName(file));
            }

            // Se envía una solicitud POST al API con el formulario como contenido
            var response = await client.PostAsync("http://singa.com.mx:5500/api/FilesAsis/CargaMul", formData);

            // Se verifica si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Se lee el contenido de la respuesta como una cadena
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Se lanza una excepción si la respuesta fue fallida
                await DisplayAlert("Error", $"La solicitud al API falló con el código {response.StatusCode}", "Cerrar");
                return null;
            }
        }
    }
}