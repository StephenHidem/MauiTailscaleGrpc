using AntPlusServer.Services;
using Serilog;
using SmallEarthTech.AntRadioInterface;
using SmallEarthTech.AntUsbStick;

// Initialize Serilog early, without access to configuration or services
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .Build())
    .WriteTo.Seq("http://docker-tailscale.tail7aec11.ts.net")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IAntRadio, AntRadio>();
builder.Services.AddSingleton<IAntRadioSubscriberFactory, AntRadioSubscriberFactory>();
builder.Services.AddSingleton<IAntChannelSubscriberFactory, AntChannelSubscriberFactory>();

builder.Host.UseSerilog();

var app = builder.Build();
try
{
    // attempt to get the AntRadio to ensure it initializes correctly
    var antRadio = app.Services.GetRequiredService<IAntRadio>();
}
catch (Exception ex)
{
    // get the logger, log the exception, and exit with a non-zero code
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Failed to initialize ANT radio.");
    Environment.Exit(ex.HResult);
}

// Configure the HTTP request pipeline.
app.MapGrpcService<AntRadioService>();
app.MapGrpcService<AntChannelService>();
app.MapGrpcService<AntControlService>();
app.MapGrpcService<AntConfigurationService>();
app.MapGrpcService<AntCryptoService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
