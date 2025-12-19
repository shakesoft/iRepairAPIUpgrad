using CommunityToolkit.Maui;
using BEZNgCore.Maui.Core;
using ZXing.Net.Maui.Controls;

namespace BEZNgCore.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        ApplicationBootstrapper.InitializeIfNeeds<BEZNgCoreMauiModule>();

        var app = builder.Build();
        return app;
    }
}