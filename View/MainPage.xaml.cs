using Asis_Batia.ViewModel;
using Xamarin.Essentials;
using Permissions = Xamarin.Essentials.Permissions;

namespace Asis_Batia.View;


public partial class MainPage : ContentPage
{
    //public static string MapAutomationPropertiesHelpText { get; private set; }

    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Solicitar permiso de cámara
        var cameraStatus = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.Camera>();
        //var permissionStatus = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.StorageRead>();
        //var permission = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.StorageWrite>();
        var permissionStatus1 = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.Media>();
        // Solicitar permiso de ubicación
        var locationStatus = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.LocationWhenInUse>();
        // Verificar el estado de los permisos y actuar en consecuencia
        if (cameraStatus != Microsoft.Maui.ApplicationModel.PermissionStatus.Granted)
        {
            await DisplayAlert("Permisos necesarios", "Los permisos de Camara son obligatorios para continuar.", "OK");
            cameraStatus = (Microsoft.Maui.ApplicationModel.PermissionStatus)await Permissions.RequestAsync<Permissions.Camera>();
            // El usuario no concedió permiso de cámara, maneja esta situación
        }
        if (locationStatus != Microsoft.Maui.ApplicationModel.PermissionStatus.Granted)
        {
            // El usuario no concedió permiso de ubicación, maneja esta situación
            await DisplayAlert("Permisos necesarios", "Los permisos de ubicación son obligatorios para continuar.", "OK");

            // Volver a solicitar los permisos
            locationStatus = (Microsoft.Maui.ApplicationModel.PermissionStatus)await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }
        // Resto del código de inicialización de la aplicación
    }
    //private async void bntNext_Clicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.GoToAsync("FormPrin",true);
    //}
    //public event EventHandler LoggedOut;

    //public void OnLoggedOut()
    //{
    //    MainPage.MapAutomationPropertiesHelpText = string.Empty;
    //    LoggedOut?.Invoke(this, EventArgs.Empty);
    //}
}
