namespace Asis_Batia;
using Asis_Batia.ViewModel;

public partial class MainPage : ContentPage
{
    private readonly IMediaPicker mediaPicker;

    public MainPage(IMediaPicker mediaPicker)
	{
		InitializeComponent();
        this.mediaPicker = mediaPicker;
    }

    private async void bntNext_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FormuPrinAsis(mediaPicker));
    }
}

