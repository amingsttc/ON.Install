using System;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ON.Mercury.Service.Database.Entities;

namespace Service.Database.Entities;

[Table("messages")]
public sealed partial class Message : IMessage<Message>
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

public void MergeFrom(Message message)
    {
        throw new System.NotImplementedException();
    }

    public void MergeFrom(CodedInputStream input)
    {
        throw new System.NotImplementedException();
    }

    public void WriteTo(CodedOutputStream output)
    {
        throw new System.NotImplementedException();
    }

    public int CalculateSize()
    {
        throw new System.NotImplementedException();
    }

    public MessageDescriptor Descriptor { get; }
    public bool Equals(Message obj)
    {
        if (obj is null || GetType() != obj.GetType())
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        Message otherMessage = (Message)obj;

        return Id == otherMessage.Id
               && ChannelId == otherMessage.ChannelId
               && SenderId == otherMessage.SenderId
               && Body == otherMessage.Body
               && SentOn == otherMessage.SentOn
               && ModifiedOn == otherMessage.ModifiedOn
               && DeletedOn == otherMessage.DeletedOn;
    }
    public Message Clone()
    {
        throw new System.NotImplementedException();
    }
}