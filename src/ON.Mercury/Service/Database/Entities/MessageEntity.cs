#nullable enable
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;

namespace Service.Database.Entities;

[Table("messages")]
public class MessageEntity : IPostgresEntity<Message, MessageEntity>
{
    public string? Id { get; set; }
    public string? ChannelId { get; set; }
    public string? SenderId { get; set; }
    public string? Body { get; set; }
    public DateTime SentOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    
    [NotMapped] [JsonIgnore] public ChannelEntity Channel { get; set; }
    
    public MessageEntity() {}

    public MessageEntity(string channelId, string senderId, string body)
    {
        Id = Guid.NewGuid().ToString();
        ChannelId = channelId;
        SenderId = senderId;
        Body = body;
        SentOn = DateTime.UtcNow;
    }
    
    public MessageEntity Clone()
    {
        throw new System.NotImplementedException();
    }

    public MessageEntity FromPb(Message proto)
    {
        throw new System.NotImplementedException();
    }

    public Message ToPb()
    {
        // TODO: Figure out how to speed this up
        var json = JsonConvert.SerializeObject(this);
        return Google.Protobuf.JsonParser.Default.Parse<Message>(json);
    }

    public static void SetColumnMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageEntity>()
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        modelBuilder.Entity<MessageEntity>()
            .Property(x => x.ChannelId)
            .HasColumnName("channel_id")
            .IsRequired();

        modelBuilder.Entity<MessageEntity>()
            .Property(x => x.SenderId)
            .HasColumnName("sender_id")
            .IsRequired();

        modelBuilder.Entity<MessageEntity>()
            .Property(x => x.Body)
            .HasColumnName("body")
            .IsRequired();

        modelBuilder.Entity<MessageEntity>()
            .Property(x => x.SentOn)
            .HasColumnName("sent_on")
            .IsRequired();

        modelBuilder.Entity<MessageEntity>()
            .Property(x => x.ModifiedOn)
            .HasColumnName("modified_on");

        modelBuilder.Entity<MessageEntity>()
            .Property(x => x.DeletedOn)
            .HasColumnName("deleted_on");

        modelBuilder.Entity<MessageEntity>()
            .HasOne(x => x.Channel)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChannelId)
            .IsRequired();
    }
}