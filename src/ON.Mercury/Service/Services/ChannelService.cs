using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using Service.Database.Entities;

namespace ON.Mercury.Service.Services;

[Authorize]
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
        try
        {
            var channels = await _postgres.Channels.ToListAsync();
            var response = new GetChannelsResponse()
            {
                IsSuccess = true,
                Error = ""
            };

            if (channels.Count > 0)
            {
                foreach (var channel in channels)
                {
                    response.Channels.Add(channel.ToPb());
                }
            }

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return new GetChannelsResponse()
            {
                IsSuccess = false,
            };
        }
    }

    public override async Task<UpdateChannelResponse> UpdateChannel(UpdateChannelRequest request, ServerCallContext context)
    {
        var foundChannel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == request.ChannelId);
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
            var foundChannel = await _postgres.Channels.Where(c => c.Id == request.ChannelId).FirstOrDefaultAsync();
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

    public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        try
        {
            var newMessage = new MessageEntity(request.ChannelId, request.SenderId, request.Body);

            await _postgres.Messages.AddAsync(newMessage);
            await _postgres.SaveChangesAsync();
            
            return new SendMessageResponse()
            {
                IsSuccess = true,
                Error = ""
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new SendMessageResponse()
            {
                IsSuccess = false,
            };
        }
    }

    public override async Task<GetMessagesResponse> GetMessages(GetMessagesRequest request, ServerCallContext context)
    {
        try
        {
            var channel = await _postgres.Channels.Where(c => c.Id == request.ChannelId).Include(m => m.Messages)
                .FirstOrDefaultAsync();
            if (channel is null)
            {
                return new GetMessagesResponse()
                {
                    IsSuccess = false,
                    Error = "Channel Not Found"
                };
            }
            _logger.LogInformation(JsonConvert.SerializeObject(channel));
            if (channel.Messages.Count == 0)
            {
                return new GetMessagesResponse()
                {
                    IsSuccess = true,
                };
            }

            var response = new GetMessagesResponse()
            {
                IsSuccess = true,
                Error = ""
            };

            foreach (var message in channel.Messages)
            {
                response.Messages.Add(message.ToPb());
            }

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(e));
            return new GetMessagesResponse()
            {
                IsSuccess = false
            };
        }
    }

    public override async Task<UpdateMessageResponse> UpdateMessage(UpdateMessageRequest request, ServerCallContext context)
    {
        try
        {
            var messageFound = await _postgres.Messages.Where(m => m.Id == request.MessageId).FirstOrDefaultAsync();
            if (messageFound is null)
            {
                return new UpdateMessageResponse()
                {
                    IsSuccess = false,
                    Error = "Message not found"
                };
            }

            messageFound.Body = request.Body;
            messageFound.ModifiedOn = DateTime.UtcNow;

            _postgres.Messages.Update(messageFound);
            await _postgres.SaveChangesAsync();
            
            return new UpdateMessageResponse()
            {
                IsSuccess = true,
                Error = "",
                Updated = messageFound.ToPb()
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(e));
            return new UpdateMessageResponse()
            {
                IsSuccess = false
            };
        }
    }

    public override async Task<DeleteMessageResponse> DeleteMessage(DeleteMessageRequest request, ServerCallContext context)
    {
        try
        {
            var messageFound = await _postgres.Messages.Where(m => m.Id == request.MessageId).FirstOrDefaultAsync();
            if (messageFound is null)
            {
                return new DeleteMessageResponse()
                {
                    IsSuccess = false,
                    Error = "Message not found"
                };
            }
            
            messageFound.DeletedOn = DateTime.UtcNow;
            _postgres.Messages.Update(messageFound);
            await _postgres.SaveChangesAsync();
            
            return new DeleteMessageResponse()
            {
                IsSuccess = true,
                Error = "",
                Deleted = messageFound.ToPb()
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(e));
            return new DeleteMessageResponse()
            {
                IsSuccess = false
            };
        }
    }
}