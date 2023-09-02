using Microsoft.EntityFrameworkCore;
using ON.Mercury.Service.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Database.Entities
{
    [Table("channels_pinned_messages")]
    public class ChannelsPinnedMessages
    {
        public string ChannelId { get; set; }
        public Channel Channel { get; set; }
        public string MessageId { get; set; }
        public Message Message { get; set; }

        public static void SetColumnMetadata(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelsPinnedMessages>()
                .HasKey(pm => new
                {
                    pm.ChannelId,
                    pm.MessageId
                });

            modelBuilder.Entity<ChannelsPinnedMessages>()
                .Property(pm => pm.ChannelId)
                .HasColumnName("channel_id");

            modelBuilder.Entity<ChannelsPinnedMessages>()
                .Property(pm => pm.MessageId)
                .HasColumnName("message_id");

            modelBuilder.Entity<ChannelsPinnedMessages>()
                .HasOne(pm => pm.Channel)
                .WithMany()
                .HasForeignKey(pm => pm.ChannelId)
                .HasConstraintName("FK_channel_id")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelsPinnedMessages>()
                .HasOne(pm => pm.Message)
                .WithMany()
                .HasForeignKey(pm => pm.MessageId)
                .HasConstraintName("FK_message_id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
