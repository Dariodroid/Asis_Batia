using Asis_Batia.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public int IdEmpleado { get; set; }
        public int IdInmueble { get; set; }

        private string _selectionRadio;

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
        }

        private async Task Register()
        {
            RegistroModel registroModel = new RegistroModel
            {
                Adjuntos = 1,
                Anio = (int)DateTime.Today.Year,
                Confirma = "",
                Cubierto = 1,
                Fecha = DateTime.Today,
                Idempleado = IdEmpleado,
                Idinmueble = IdInmueble,
                Idperiodo = IdCliente,
                Latitud = "000",
                Longitud = "000",
                Movimiento = "",
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
            }
            else
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar", "Ok");
            }
        }

        private async Task LoadFile()
        {
            try
            {
                PickOptions options = new PickOptions();
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("doc", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        var file = ImageSource.FromStream(() => stream);
                    }
                }

                //return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
        }
    }
}





