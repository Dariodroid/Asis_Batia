using System.Windows.Input;

namespace Asis_Batia;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        BindingContext = this;
	}
    public ICommand LogoutCommand => new Command(async () => {
        Preferences.Clear();
        await Shell.Current.GoToAsync("//MainPage",true);
    });
}
