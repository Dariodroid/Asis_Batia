using Asis_Batia.Model;
using Asis_Batia.ViewModel;
using Microsoft.Maui.Media;

namespace Asis_Batia.View;

public partial class FormuSegAsis : ContentPage
{
    FormSegAsisViewModel formSegAsisViewModel;
    public FormuSegAsis()
    {
        InitializeComponent();
        formSegAsisViewModel = new FormSegAsisViewModel();
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
}