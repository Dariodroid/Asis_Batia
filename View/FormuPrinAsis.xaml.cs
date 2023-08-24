namespace Asis_Batia.ViewModel;
using Asis_Batia.View;

public partial class FormuPrinAsis : ContentPage
{
    private readonly IMediaPicker mediaPicker;

    public FormuPrinAsis(IMediaPicker mediaPicker)
	{
		InitializeComponent();
        BindingContext = new FormuPrinAsisViewModel(mediaPicker);
        this.mediaPicker = mediaPicker;
    }

    private async void bntNext2_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FormSegAsis(mediaPicker));
    }
}