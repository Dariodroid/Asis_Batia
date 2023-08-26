using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class FormuPrinAsis : ContentPage
{
	IMediaPicker mediaPicker;
	public FormuPrinAsis()
	{
		InitializeComponent();
		BindingContext = new FormuPrinAsisViewModel();
	}
}