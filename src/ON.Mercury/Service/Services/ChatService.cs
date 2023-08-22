using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Caching;
using ON.Mercury.Service.Database;
using Service.Database.Entities;
using System.Collections.Generic;
using Channel = ON.Fragments.Mercury.Channel;

namespace ON.Mercury.Service.Services;

[Authorize]
public class ChatService : ChatInterface.ChatInterfaceBase
{
    private readonly ILogger<ChatService> _logger;
    private readonly PostgresContext _postgres;
    private readonly ICachingService _cache;

    public ChatService(ILogger<ChatService> logger, PostgresContext postgres, ICachingService cache)
    {
        _logger = logger;
        _postgres = postgres;
        _cache = cache;
    }
    
    public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        try
        {
            var newMessage = new MessageEntity(request.ChannelId, request.SenderId, request.Body);

            await _postgres.Messages.AddAsync(newMessage);
            await _postgres.SaveChangesAsync();
            await _cache.AddOrSetAsync($"messages:{request.ChannelId}", new List<Message>()
            {
                newMessage.ToPb()
            });
            
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

    // public override async Task<GetMessagesResponse> GetMessages(GetMessagesRequest request, ServerCallContext context)
    // {
    //     try
    //     {
    //         // TODO: Rework this to use the cache
    //         var channel = await _postgres.Channels.Where(c => c.Id == request.ChannelId).Include(m => m.Messages)
    //             .FirstOrDefaultAsync();
    //         if (channel is null)
    //         {
    //             return new GetMessagesResponse()
    //             {
    //                 IsSuccess = false,
    //                 Error = "Channel Not Found"
    //             };
    //         }
    //         _logger.LogInformation(JsonConvert.SerializeObject(channel));
    //         if (channel.Messages.Count == 0)
    //         {
    //             return new GetMessagesResponse()
    //             {
    //                 IsSuccess = true,
    //             };
    //         }
    //
    //         var response = new GetMessagesResponse()
    //         {
    //             IsSuccess = true,
    //             Error = ""
    //         };
    //
    //         foreach (var message in channel.Messages)
    //         {
    //             response.Messages.Add(message);
    //         }
    //
    //         return response;
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogInformation(JsonConvert.SerializeObject(e));
    //         return new GetMessagesResponse()
    //         {
    //             IsSuccess = false
    //         };
    //     }
    // }

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