using AntConfigurationGrpcService;
using AntControlGrpcService;
using AntCryptoGrpcService;
using AntRadioGrpcService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using SmallEarthTech.AntRadioInterface;

namespace AntPlusMauiClient.GrpcServices
{
    /// <summary>
    /// Service for interacting with ANT radio using gRPC.
    /// </summary>
    public partial class AntRadioService : IAntRadio
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<AntRadioService> _logger;
        private readonly CancellationToken _cancellationToken;
        private readonly GrpcChannelOptions _grpcChannelOptions;
        private gRPCAntRadio.gRPCAntRadioClient? _client;
        private GrpcChannel? _grpcChannel;

        /// <summary>
        /// Gets the fully qualified domain name (FQDN) of the tailnet associated with this application.
        /// </summary>
        public static string TailnetFqdn => "hidem-laptop.tail7aec11.ts.net";

        public UriBuilder UriBuilder => new("http", "localhost", 5073);

        /// <inheritdoc/>
        public int NumChannels => throw new NotImplementedException();

        /// <inheritdoc/>
        public string ProductDescription { get; private set; } = string.Empty;

        /// <inheritdoc/>
        public uint SerialNumber { get; private set; }

        /// <inheritdoc/>
        public string Version { get; private set; } = string.Empty;

        /// <inheritdoc/>
        public event EventHandler<AntResponse>? RadioResponse;

        /// <summary>
        /// Event triggered when an RPC exception is received.
        /// </summary>
        public event EventHandler<RpcException>? RpcExceptionReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="AntRadioService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        /// <param name="grpcChannelOptions">Optional gRPC channel configuration options.</param>
        public AntRadioService(
            ILoggerFactory loggerFactory, CancellationTokenSource cancellationTokenSource,
            GrpcChannelOptions? grpcChannelOptions = default)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<AntRadioService>();
            _cancellationToken = cancellationTokenSource.Token;
            _grpcChannelOptions = grpcChannelOptions ?? new GrpcChannelOptions();
        }

        /// <summary>
        /// Creates a gRPC channel to the ANT radio server and retrieves its properties.
        /// </summary>
        /// <returns>A bool indicating success (true), or failure (false).</returns>
        public async Task<bool> FindAntRadioServerAsync()
        {
            // use Tailnet fully qualified domain name to connect to server
            //UriBuilder uriBuilder = new("http", AntRadioService.TailnetFqdn, 5073);
            try {
                _grpcChannel = GrpcChannel.ForAddress(UriBuilder.Uri, _grpcChannelOptions);
                _client = new gRPCAntRadio.gRPCAntRadioClient(_grpcChannel);

                // get properties from server
                PropertiesReply reply = await _client.GetPropertiesAsync(new Empty(), cancellationToken: _cancellationToken);
                ProductDescription = reply.ProductDescription;
                SerialNumber = reply.SerialNumber;
                Version = reply.Version;

                // create other service clients
                _control = new gRPCAntControl.gRPCAntControlClient(_grpcChannel);
                _config = new gRPCAntConfiguration.gRPCAntConfigurationClient(_grpcChannel);
                _crypto = new gRPCAntCrypto.gRPCAntCryptoClient(_grpcChannel);

                // subscribe to radio response updates
                HandleRadioResponseUpdates(_cancellationToken);

                return true;
            }
            catch (RpcException ex)
            {
                _logger.LogError("FindAntRadioServerAsync: RpcException {Status}, {Message}", ex.Status, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Handles radio response updates.
        /// </summary>
        /// <param name="cancellationToken">Cancels subscription to ChannelResponseUpdate.</param>
        private async void HandleRadioResponseUpdates(CancellationToken cancellationToken)
        {
            using var response = _client!.Subscribe(new Empty(), cancellationToken: cancellationToken);
            try
            {
                await foreach (AntResponseReply? update in response.ResponseStream.ReadAllAsync(cancellationToken))
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("OnDeviceResponse: {Channel}, {ResponseId}, {Data}", update.ChannelNumber, (MessageId)update.ResponseId, BitConverter.ToString(update.Payload.ToByteArray()));
                    }
                    RadioResponse?.Invoke(this, new GrpcAntResponse(update));
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                _logger.LogInformation("RpcException: unavailable");
                RpcExceptionReceived?.Invoke(this, ex);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                _logger.LogInformation("RpcException: operation cancelled");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("OperationCanceledException");
            }
        }

        /// <inheritdoc/>
        public void CancelTransfers(int cancelWaitTime)
        {
            _client!.CancelTransfers(new CancelTransfersRequest { WaitTime = cancelWaitTime });
        }

        /// <inheritdoc/>
        public IAntChannel GetChannel(int num)
        {
            _ = _client!.GetChannel(new GetChannelRequest { ChannelNumber = (byte)num });
            return new AntChannelService(_loggerFactory.CreateLogger<AntChannelService>(), (byte)num, _grpcChannel!);
        }

        /// <inheritdoc/>
        public async Task<DeviceCapabilities> GetDeviceCapabilities(bool forceNewCopy = false, uint responseWaitTime = 1500)
        {
            GetDeviceCapabilitiesReply caps = await _client!.GetDeviceCapabilitiesAsync(new GetDeviceCapabilitiesRequest { ForceCopy = forceNewCopy, WaitResponseTime = responseWaitTime });
            return new GrpcDeviceCapabilities(caps);
        }

        /// <inheritdoc/>
        public async Task<IAntChannel[]> InitializeContinuousScanMode()
        {
            if (_grpcChannel == null)
            {
                throw new InvalidOperationException("gRPC channel is not initialized. Invoke FindAntRadioServerAsync method prior to invoking this method.");
            }

            InitScanModeReply reply = await _client!.InitializeContinuousScanModeAsync(new Empty());
            AntChannelService[] channels = new AntChannelService[reply.NumChannels];
            for (byte i = 0; i < reply.NumChannels; i++)
            {
                channels[i] = new AntChannelService(_loggerFactory.CreateLogger<AntChannelService>(), i, _grpcChannel);
            }
            channels[0].HandleChannelResponseUpdates(_cancellationToken);
            return channels;
        }

        /// <inheritdoc/>
        public AntResponse ReadUserNvm(ushort address, byte size, uint responseWaitTime = 500)
        {
            AntResponseReply response = _client!.ReadUserNvm(new ReadUserNvmRequest { Address = address, Size = size, WaitResponseTime = responseWaitTime });
            return new GrpcAntResponse(response);
        }

        /// <inheritdoc/>
        public AntResponse RequestMessageAndResponse(SmallEarthTech.AntRadioInterface.RequestMessageID messageID, uint responseWaitTime, byte channelNum = 0)
        {
            AntResponseReply response = _client!.RequestMessageAndResponse(new RequestMessageAndResponseRequest { MsgId = (AntRadioGrpcService.RequestMessageID)messageID, WaitResponseTime = responseWaitTime, ChannelNumber = channelNum });
            return new GrpcAntResponse(response);
        }

        /// <inheritdoc/>
        public bool WriteRawMessageToDevice(byte msgID, byte[] msgData)
        {
            BoolValue reply = _client!.WriteRawMessageToDevice(new WriteRawMessageToDeviceRequest { MsgId = msgID, MsgData = Google.Protobuf.ByteString.CopyFrom(msgData) });
            return reply.Value;
        }
    }
}
