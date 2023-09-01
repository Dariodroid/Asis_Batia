using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();
	}

    private async void bntNext_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("FormPrin",true);
    }
}