using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class RegExitoso : ContentPage
{
	public RegExitoso()
	{
		InitializeComponent();
		BindingContext = new RegExitosoViewModel();
	}

    //private void bntNext4_Clicked(object sender, EventArgs e)
    //{
        
    //        Application.Current.Quit();
        
    //}
}