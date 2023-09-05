﻿using Asis_Batia.View;
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
                fonts.AddFont("tw-cen-mt-condensed-3", "Regular");
				fonts.AddFont("Font Awesome 6 Free - Solid - 900", "Solid");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
		builder.Services.AddTransient<FormuSegAsis>();

		Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute("FormPrin", typeof(FormuPrinAsis));
        Routing.RegisterRoute("FormSeg", typeof(FormuSegAsis));
        Routing.RegisterRoute("FormReg", typeof(RegExitoso));

        return builder.Build();
	}
}
