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

namespace ON.Mercury.Service.Database.Repositories
{
    public class ChannelRepository
    {
        // TODO: Look Into CQRS
        private readonly ILogger<ChannelRepository> _logger;
        private readonly PostgresContext _postgres;
        
        public ChannelRepository(ILogger<ChannelRepository> logger, PostgresContext postgres)
        {
            _logger = logger;
            _postgres = postgres;
        }

        public async Task<Channel> CreateChannelAsync(string name, string category = "", string description = "", CancellationToken cancellationToken = default)
        {
            var newChannel = new Channel()
            {
                Id = Guid.NewGuid().ToString(),
                Category = category,
                Description = description,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };
            await _postgres.Channels.AddAsync(newChannel, cancellationToken);
            await _postgres.SaveChangesAsync(cancellationToken);

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

            return channel;
        }

        public async Task<string?> DeleteChannelAsync(string id, CancellationToken cancellationToken = default)
        {
            var channel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (channel is null) return null;

            _postgres.Channels.Remove(channel);
            await _postgres.SaveChangesAsync(cancellationToken);

            return channel.Id;
        }

        public async Task<IReadOnlyList<Message>?> GetMessagesAsync(string channelId, MessageSenderParams messageSenderParams = MessageSenderParams.SenderId, CancellationToken cancellationToken = default)
        {
            var channel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == channelId, cancellationToken);
            if (channel is null) return null;
            var messages = await _postgres.Messages
                .Where(m => m.ChannelId == channelId && m.DeletedOn == null)
                .OrderBy(m => m.SentOn)
                .ToListAsync(cancellationToken);
            return messages;
        }

        public async Task<Message> SendMessageAsync(string channelId, string senderId, string body, CancellationToken cancellationToken = default)
        {
            var newMessage = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                ChannelId = channelId,
                SenderId = senderId,
                Body = body,
                SentOn = DateTime.UtcNow
            };
            await _postgres.Messages.AddAsync(newMessage, cancellationToken);
            await _postgres.SaveChangesAsync(cancellationToken);

            return new Message();
        }
    }
}
