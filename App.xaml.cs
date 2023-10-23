using Asis_Batia.View;

namespace Asis_Batia;

public partial class App : Application
{
    private readonly IMediaPicker mediaPicker;

    public App(IMediaPicker mediaPicker)
	{
		InitializeComponent();
        MainPage = new AppShell();

        //MainPage = new NavigationPage(new MainPage(mediaPicker));
        //      this.mediaPicker = mediaPicker;
    }
   
}
