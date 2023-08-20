namespace Asis_Batia;
using Asis_Batia.ViewModel;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private async void bntNext_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FormuPrinAsis());
    }
}

