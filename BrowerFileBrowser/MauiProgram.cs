//Chase Brower, 2023

using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using BrowerFileBrowser.Containers;
using BrowerFileBrowser.Interfaces;
using BrowerFileBrowser.Factory;

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
        builder.Services.AddSingleton<IDefaultNodeProvider>(DefaultNodeProviderFactory.Create());

        builder.Services.AddMauiBlazorWebView();
		
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		
        builder.Services.AddMudServices();

        return builder.Build();
	}
}
