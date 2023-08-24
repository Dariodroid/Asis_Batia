using Asis_Batia.View;
using Asis_Batia.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Media;

namespace Asis_Batia;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
		builder.Services.AddTransient<FormuPrinAsisViewModel>();
		return builder.Build();
	}
}
