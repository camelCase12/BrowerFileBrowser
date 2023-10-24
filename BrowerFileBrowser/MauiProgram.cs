using Microsoft.Extensions.Logging;
using BrowerFileBrowser.Data;
using MudBlazor.Services;
using BrowerFileBrowser.Containers;

namespace BrowerFileBrowser;

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
			});

        StateContainer.RegisterAllDerived(builder.Services);

        builder.Services.AddMauiBlazorWebView();
		
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<WeatherForecastService>();

        builder.Services.AddMudServices();

        return builder.Build();
	}
}
