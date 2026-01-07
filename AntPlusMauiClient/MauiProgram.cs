using AntPlusMauiClient.GrpcServices;
using AntPlusMauiClient.PageModels;
using AntPlusMauiClient.Pages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Serilog;
using SmallEarthTech.AntPlus.Extensions.Hosting;
using SmallEarthTech.AntRadioInterface;
using Syncfusion.Maui.Toolkit.Hosting;

namespace AntPlusMauiClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Initialize Serilog early, without access to configuration or services
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseAntPlus()
                .RegisterAppServices()
                .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("CascadiaCode.ttf", "CascadiaCode");
                });

#if DEBUG
    		//builder.Logging.AddDebug();
            builder.Logging.AddSerilog(dispose: true);
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {   // Register your services here
            mauiAppBuilder.Services
                .AddSingleton<IAntRadio, AntRadioService>()
                .AddSingleton<CancellationTokenSource>()

                // Register the pages and view models with shell routes
                .AddTransientWithShellRoute<MainPage, MainPageModel>("MainPage")
                .AddTransientWithShellRoute<AntDevicePage, AntDevicePageModel>("AntDevicePage");
            return mauiAppBuilder;
        }
    }
}
