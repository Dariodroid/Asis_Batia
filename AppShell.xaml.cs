using System.Windows.Input;
using Asis_Batia.View;
namespace Asis_Batia;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        BindingContext = this;
	}
    public ICommand LogoutCommand => new Command(async () => {
        
       await Shell.Current.GoToAsync("//MainPage",true);
       Preferences.Clear("UserId");

        //var contentPageWithEntry = Shell.Current?.CurrentItem?.CurrentItem?.CurrentItem?.Content as MainPage; // Reemplaza 'YourContentPageType' con el tipo de tu ContentPage

        //// Verifica si se encontró la ContentPage y, si es así, dispara el evento LoggedOut
        //if (contentPageWithEntry != null)
        //{
        //    contentPageWithEntry.onLoggedOut();
        //}
    });
}
