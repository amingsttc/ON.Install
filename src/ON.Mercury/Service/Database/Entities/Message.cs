using System;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ON.Mercury.Service.Database.Entities;
using MessageProto = ON.Fragments.Mercury.Message;

namespace Service.Database.Entities;

[Table("messages")]
public class Message
{
    public string Id { get; set; }
    public string? ChannelId { get; set; }
    public string? SenderId { get; set; }
    public string? Body { get; set; }
    public DateTime SentOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    [NotMapped] [JsonIgnore] public Channel Channel { get; set; }

    public static void SetColumnMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        modelBuilder.Entity<Message>()
            .Property(x => x.ChannelId)
            .HasColumnName("channel_id")
            .IsRequired();

        modelBuilder.Entity<Message>()
            .Property(x => x.SenderId)
            .HasColumnName("sender_id")
            .IsRequired();

        modelBuilder.Entity<Message>()
            .Property(x => x.Body)
            .HasColumnName("body")
            .IsRequired();

        modelBuilder.Entity<Message>()
            .Property(x => x.SentOn)
            .HasColumnName("sent_on")
            .IsRequired();

        modelBuilder.Entity<Message>()
            .Property(x => x.ModifiedOn)
            .HasColumnName("modified_on");

        modelBuilder.Entity<Message>()
            .Property(x => x.DeletedOn)
            .HasColumnName("deleted_on");

        modelBuilder.Entity<Message>()
            .HasOne(x => x.Channel)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChannelId)
            .IsRequired();
    }

    public static MessageProto ToPb(Message msg)
    {
        return new MessageProto()
        {
            Id = msg.Id,
            ChannelId = msg.ChannelId,
            SenderId = msg.SenderId,
            Body = msg.Body,
            SentOn = Timestamp.FromDateTime(msg.SentOn),
            ModifiedOn = Timestamp.FromDateTime(msg.SentOn),
            DeletedOn = null
        };
    }
}