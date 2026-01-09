using AntPlusMauiClient.GrpcServices;
using AntPlusMauiClient.PageModels;
using AntPlusMauiClient.Pages;
using AntPlusMauiClient.ViewModels;
using AntPlusMauiClient.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Serilog;
using SmallEarthTech.AntPlus.DeviceProfiles.AssetTracker;
using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;
using SmallEarthTech.AntPlus.DeviceProfiles.BikeSpeedAndCadence;
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

                // Register the pages and page models with shell routes
                .AddTransientWithShellRoute<MainPage, MainPageModel>("MainPage")
                .AddTransientWithShellRoute<AntDevicePage, AntDevicePageModel>("AntDevicePage")

                // ViewModels
                .AddTransient<AssetTrackerViewModel>()
                .AddTransient<BicyclePowerViewModel>()
                .AddTransient<CTFViewModel>()
                .AddTransient<BikeSpeedViewModel>()
                .AddTransient<BikeSpeedAndCadenceViewModel>()
                .AddTransient<BikeCadenceViewModel>()
                .AddTransient<FitnessEquipmentViewModel>()
                .AddTransient<GeocacheViewModel>()
                .AddTransient<HeartRateViewModel>()
                .AddTransient<MuscleOxygenViewModel>()
                .AddTransient<SDMViewModel>()
                .AddTransient<UnknownDeviceViewModel>()

                // Views
                .AddTransient<AssetTrackerView>()
                .AddTransient<BicyclePowerView>()
                .AddTransient<CTFView>()
                .AddTransient<BikeSpeedView>()
                .AddTransient<BikeSpeedAndCadenceView>()
                .AddTransient<BikeCadenceView>()
                .AddTransient<FitnessEquipmentView>()
                .AddTransient<GeocacheView>()
                .AddTransient<HeartRateView>()
                .AddTransient<MuscleOxygenView>()
                .AddTransient<SDMView>()
                .AddTransient<UnknownDeviceView>();

            return mauiAppBuilder;
        }
    }
}
