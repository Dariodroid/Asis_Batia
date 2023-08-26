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

    private async void btn_camara_Clicked(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                string LocalFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using Stream source = await photo.OpenReadAsync();
                using FileStream localFile = File.OpenWrite(LocalFilePath);
                await source.CopyToAsync(localFile);
            }
        }
    }

    private async void bntNext3_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new RegExitoso());
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