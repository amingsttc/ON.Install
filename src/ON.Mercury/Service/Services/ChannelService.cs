using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using Service.Database.Entities;

namespace ON.Mercury.Service.Services;

public class ChannelService : ChannelInterface.ChannelInterfaceBase
{
    private readonly ILogger<ChannelService> _logger;
    private readonly PostgresContext _postgres;

    public ChannelService(ILogger<ChannelService> logger, PostgresContext postgres)
    {
        _logger = logger;
        _postgres = postgres;
    }

    public override async Task<CreateChannelResponse> CreateChannel(CreateChannelRequest request, ServerCallContext context)
    {
        try
        {
            var newChannel = new ChannelEntity(request.Name, request.Category, request.Description);
            await _postgres.Channels.AddAsync(newChannel);
            await _postgres.SaveChangesAsync();

            return new CreateChannelResponse()
            {
                IsSuccess = true,
                Error = "",
                Channel = newChannel.ToPb()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<GetChannelsResponse> GetChannels(GetChannelsRequest request, ServerCallContext context)
    {
        var channels = await _postgres.Channels.ToListAsync();
        var response = new GetChannelsResponse()
        {
            IsSuccess = true,
            Error = ""
        };

        if (channels.Count == 0) return response;

        foreach (var channel in channels)
        {
            var proto = channel.ToPb();
            response.Channels.Add(proto);
        }

        return response;
    }

    public override Task<UpdateChannelResponse> UpdateChannel(UpdateChannelRequest request, ServerCallContext context)
    {
        return base.UpdateChannel(request, context);
    }

    public override Task<DeleteChannelResponse> DeleteChannel(DeleteChannelRequest request, ServerCallContext context)
    {
        return base.DeleteChannel(request, context);
    }
}