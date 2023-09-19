using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class FormuPrinAsis : ContentPage
{
	public FormuPrinAsis()
	{
		InitializeComponent();
		BindingContext = new FormuPrinAsisViewModel();
	}

}