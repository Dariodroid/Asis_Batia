using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class MainPage : ContentPage
{
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
            // El usuario no concedió permiso de cámara, maneja esta situación
        }
        if (locationStatus != Microsoft.Maui.ApplicationModel.PermissionStatus.Granted)
        {
            // El usuario no concedió permiso de ubicación, maneja esta situación
        }
        // Resto del código de inicialización de la aplicación
    }
    //private async void bntNext_Clicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.GoToAsync("FormPrin",true);
    //}
}