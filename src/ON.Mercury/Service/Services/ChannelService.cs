using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ON.Fragments.Mercury;

namespace ON.Mercury.Service.Services;

public class ChannelService : ChannelInterface.ChannelInterfaceBase
{
    private readonly ILogger<ChannelService> _logger;

    public ChannelService(ILogger<ChannelService> logger)
    {
        _logger = logger;
    }

    public override Task<CreateChannelResponse> CreateChannel(CreateChannelRequest request, ServerCallContext context)
    {
        return base.CreateChannel(request, context);
    }
}