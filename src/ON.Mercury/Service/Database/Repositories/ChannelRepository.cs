#nullable enable
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Models.Channels;
using Service.Database.Entities;
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

        public async Task<ChannelEntity> CreateChannelAsync(string name, string category = "", string description = "", CancellationToken cancellationToken = default)
        {
            var newChannel = new ChannelEntity(name, category, description);
            await _postgres.Channels.AddAsync(newChannel, cancellationToken);
            await _postgres.SaveChangesAsync(cancellationToken);

            return newChannel;
        }

        public async Task<IReadOnlyList<ChannelEntity>> GetChannelsAsync(CancellationToken cancellationToken = default)
        {
            var channels = await _postgres.Channels.ToListAsync(cancellationToken);
            return channels;
        }

        public async Task<ChannelEntity?> UpdateChannelAsync(string id, string name, string category = "", string description = "", CancellationToken cancellationToken = default)
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

        public async Task<IReadOnlyList<MessageEntity>?> GetMessagesAsync(string channelId, MessageSenderParams messageSenderParams = MessageSenderParams.SenderId, CancellationToken cancellationToken = default)
        {
            var channel = await _postgres.Channels.FirstOrDefaultAsync(c => c.Id == channelId, cancellationToken);
            if (channel is null) return null;
            var messages = await _postgres.Messages.Where(m => m.ChannelId == channelId && m.DeletedOn == null).OrderByDescending(m => m.SentOn).ToListAsync(cancellationToken);
            return messages;
        }

        public async Task<MessageEntity> SendMessageAsync(string channelId, string senderId, string body, CancellationToken cancellationToken = default)
        {
            var newMessage = new MessageEntity(channelId, senderId, body);
            await _postgres.Messages.AddAsync(newMessage, cancellationToken);
            await _postgres.SaveChangesAsync(cancellationToken);

            return newMessage;
        }
    }
}
