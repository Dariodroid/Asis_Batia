using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class SelectInmueble : ContentPage
{
	public SelectInmueble()
	{
		InitializeComponent();
		BindingContext = new SelectInmuebleViewModel();
	}
}