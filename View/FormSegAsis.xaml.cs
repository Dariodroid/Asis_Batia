namespace Asis_Batia.View;

public partial class FormSegAsis : ContentPage
{
    private readonly IMediaPicker mediaPicker;

    public FormSegAsis(IMediaPicker mediaPicker)
	{
		InitializeComponent();
        this.mediaPicker = mediaPicker;
    }

    private async void bntNext3_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegExitoso());
    }

    private async void btn_camara_Clicked(object sender, EventArgs e)
    {
        if(mediaPicker.IsCaptureSupported)
        {
            FileResult photo = await mediaPicker.CapturePhotoAsync();
            if(photo != null)
            {
                string LocalFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using Stream source =  await photo.OpenReadAsync();
                using FileStream localFile = File.OpenWrite(LocalFilePath);
                await source.CopyToAsync(localFile);
            }
        }
    }
}