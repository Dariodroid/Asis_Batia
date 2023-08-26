using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class RegExitoso : ContentPage
{
	public RegExitoso()
	{
		InitializeComponent();
		BindingContext = new RegExitosoViewModel();
	}

    private async void bntNext4_Clicked(object sender, EventArgs e)
    {
		//await Navigation.PushAsync(new MainPage());
    }
}