using System;
using System.Linq;
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

        foreach (var proto in channels.Select(channel => channel.ToPb()))
        {
            response.Channels.Add(proto);
        }

        return response;
    }

    public override async Task<UpdateChannelResponse> UpdateChannel(UpdateChannelRequest request, ServerCallContext context)
    {
        var foundChannel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (foundChannel is null)
        {
            return new UpdateChannelResponse()
            {
                    IsSuccess = false,
                    Error = "Channel Not Found"
            };
        }

        foundChannel.Name = request.Name;
        foundChannel.Description = request.Description;
        foundChannel.Category = request.Category;
        // foundChannel.Roles.Clear();
        // foundChannel.Roles = request.Roles;

        try
        {
            _postgres.Channels.Update(foundChannel);
            await _postgres.SaveChangesAsync();

            return new UpdateChannelResponse()
            {
                IsSuccess = true,
                Error = "",
                Channel = foundChannel.ToPb()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new UpdateChannelResponse()
            {
                IsSuccess = false,
                Error = "Error Updating Channel"
            };
        }
    }

    public override async Task<DeleteChannelResponse> DeleteChannel(DeleteChannelRequest request, ServerCallContext context)
    {
        try
        {
            var foundChannel = await _postgres.Channels.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (foundChannel is null)
            {
                return new DeleteChannelResponse()
                {
                    IsSuccess = false,
                    Error = "Channel Not Found"
                };
            }

            _postgres.Channels.Remove(foundChannel);
            await _postgres.SaveChangesAsync();
            return new DeleteChannelResponse()
            {
                IsSuccess = true,
                Error = "",
                ChannelId = foundChannel.Id
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DeleteChannelResponse()
            {
                IsSuccess = false,
                Error = "Server error"
            };
        }
    }

    public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        return base.SendMessage(request, context);
    }

    public override Task<GetMessagesResponse> GetMessages(GetMessagesRequest request, ServerCallContext context)
    {
        return base.GetMessages(request, context);
    }

    public override Task<UpdateMessageResponse> UpdateMessage(UpdateMessageRequest request, ServerCallContext context)
    {
        return base.UpdateMessage(request, context);
    }

    public override Task<DeleteMessageResponse> DeleteMessage(DeleteMessageRequest request, ServerCallContext context)
    {
        return base.DeleteMessage(request, context);
    }
}