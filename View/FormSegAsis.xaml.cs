namespace Asis_Batia.View;

public partial class FormSegAsis : ContentPage
{
	public FormSegAsis()
	{
		InitializeComponent();
	}

    private async void bntNext3_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegExitoso());
    }
}