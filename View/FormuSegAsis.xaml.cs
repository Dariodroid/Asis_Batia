using Asis_Batia.Model;
using Asis_Batia.ViewModel;
using Microsoft.Maui.Media;

namespace Asis_Batia.View;

public partial class FormuSegAsis : ContentPage
{
    FormSegAsisViewModel formSegAsisViewModel;
    private readonly IMediaPicker mediaPicker;
    public FormuSegAsis(IMediaPicker mediaPicker)
    {
        InitializeComponent();
        this.mediaPicker = mediaPicker;
        formSegAsisViewModel = new FormSegAsisViewModel(mediaPicker);
        BindingContext = formSegAsisViewModel;
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radio = sender as RadioButton;
        if (radio.IsChecked)
        {
            formSegAsisViewModel.SelectionRadio = radio.Value.ToString();
        }
    }

    //private async void bntNext5_Clicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.GoToAsync("FormReg", true);
    //}
}