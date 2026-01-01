using AntPlusServer.Services;
using SmallEarthTech.AntRadioInterface;
using SmallEarthTech.AntUsbStick;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IAntRadio, AntRadio>();
builder.Services.AddSingleton<IAntRadioSubscriberFactory, AntRadioSubscriberFactory>();
builder.Services.AddSingleton<IAntChannelSubscriberFactory, AntChannelSubscriberFactory>();

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
    logger.LogCritical(ex.Message);
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
