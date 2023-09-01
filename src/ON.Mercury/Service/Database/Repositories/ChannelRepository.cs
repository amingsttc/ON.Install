#nullable enable
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Database.Entities;
using ON.Mercury.Service.Models.Channels;
using Service.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ON.Mercury.Service.Database.UnionTables;
using ON.Mercury.Service.Hubs;

namespace ON.Mercury.Service.Database.Repositories
{
    public class ChannelRepository
    {
        // TODO: Look Into CQRS
        private readonly ILogger<ChannelRepository> _logger;
        private readonly PostgresContext _postgres;
        private const string DEFAULT_ROLE = "9bb8d652-cc23-497d-a222-ea4c681c2a64";
        private readonly IHubContext<ChatHub> _hubContext;
        
        public ChannelRepository(ILogger<ChannelRepository> logger, PostgresContext postgres, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _postgres = postgres;
            _hubContext = hubContext;
        }

        public async Task<Channel> CreateChannelAsync(string name, string category = "", string description = "", IEnumerable<Role> roles = null, CancellationToken cancellationToken = default)
        {
            var newChannel = new Channel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Category = category,
                Description = description,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            // TODO: Give ChannelsRoles a primary key
            // if (roles is not null)
            // {
            //     newChannel.Roles.Add(roles);
            // }
            // else
            // {
            //     var defaultRole = await _postgres.Roles.FirstOrDefaultAsync(r => r.Id == DEFAULT_ROLE, cancellationToken);
            //     await _postgres.ChannelRoles.AddAsync(new ChannelsRoles()
            //     {
            //         ChannelId = newChannel.Id,
            //         RoleId = DEFAULT_ROLE
            //     });
            // }
            
            await _postgres.Channels.AddAsync(newChannel, cancellationToken);
            await _postgres.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.All.SendAsync("ChannelCreated", JsonConvert.SerializeObject(newChannel), cancellationToken);

            return newChannel;
        }

        public async Task<IEnumerable<Channel>> GetChannelsAsync(CancellationToken cancellationToken = default)
        {
            var channels = await _postgres.Channels.ToListAsync(cancellationToken);
            return channels;
        }

        public async Task<Channel> UpdateChannelAsync(string id, string name, string category = "", string description = "", CancellationToken cancellationToken = default)
        {
            var channel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (channel is null) return null;

            channel.Name = name;
            channel.Category = category;
            channel.Description = description;

            _postgres.Channels.Update(channel);
            await _postgres.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.All.SendAsync("ChannelUpdated", JsonConvert.SerializeObject(channel),
                cancellationToken);

            return channel;
        }

        public async Task<string?> DeleteChannelAsync(string id, CancellationToken cancellationToken = default)
        {
            var channel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (channel is null) return null;

            _postgres.Channels.Remove(channel);
            await _postgres.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.All.SendAsync("ChannelDeleted", channel.Id, cancellationToken);

            return channel.Id;
        }

        public async Task<IReadOnlyList<Message>?> GetMessagesAsync(string channelId, MessageSenderParams messageSenderParams = MessageSenderParams.SenderId, string lastReceivedId = null, CancellationToken cancellationToken = default)
        {
            var channel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == channelId, cancellationToken);
            if (channel is null) return null;
            if (string.IsNullOrWhiteSpace(lastReceivedId))
            {
                var messages = await _postgres.Messages
                    .Where(m => m.ChannelId == channelId && m.DeletedOn == null)
                    .OrderBy(m => m.SentOn)
                    .ToListAsync(cancellationToken);
                return messages;
            }
            else
            {
                var messages = await _postgres.Messages
                    .Where(m => m.ChannelId == channelId && m.DeletedOn == null)
                    .OrderBy(m => m.SentOn)
                    .ToListAsync(cancellationToken);

                var startIndex = messages.FindIndex(m => m.Id == lastReceivedId);
                if (startIndex >= 0)
                {
                    messages = messages.GetRange(startIndex + 1, messages.Count - startIndex - 1);
                }

                return messages;
            }
        }
    }
}
