namespace Asis_Batia.ViewModel;

public partial class FormuPrinAsis : ContentPage
{
	public FormuPrinAsis()
	{
		InitializeComponent();
        BindingContext = new FormuPrinAsisViewModel();
    }
}