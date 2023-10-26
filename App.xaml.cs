using Asis_Batia.View;

namespace Asis_Batia;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
    }
   
}
