namespace Asis_Batia.ViewModel;
using Asis_Batia.View;

public partial class FormuPrinAsis : ContentPage
{
	public FormuPrinAsis()
	{
		InitializeComponent();
        BindingContext = new FormuPrinAsisViewModel();
    }

    private async void bntNext2_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FormSegAsis());
    }
}