//Chase Brower, 2023

using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using BrowerFileBrowser.Containers;
using BrowerFileBrowser.Interfaces;
using BrowerFileBrowser.Factory;
using BrowerFileBrowser.DAO;
using BrowerFileBrowser.Models;
using System.Net.Http;

namespace BrowerFileBrowser;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		DatabaseInitialization.InitializeDatabase();

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

        StateContainer.RegisterAllDerived(builder.Services);
        builder.Services.AddSingleton(DefaultNodeProviderFactory.Create());
        builder.Services.AddSingleton<IThumbnailService, ThumbnailService>();

        builder.Services.AddSingleton(new HttpClient(new CustomHttpHandler()) { });

        builder.Services.AddMauiBlazorWebView();

		builder.Services.AddMudServices();
		
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		
        builder.Services.AddMudServices();

        return builder.Build();
	}
}
