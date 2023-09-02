using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using ON.Mercury.Service.Database.UnionTables;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.Collections;
using Service.Database.Entities;
using System.Linq;

namespace ON.Mercury.Service.Database.Entities
{
    [Table("channels")]
    public sealed partial class Channel : IMessage<Fragments.Mercury.Channel>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped] public RepeatedField<Message> Messages { get; set; } = new();
        [NotMapped] public RepeatedField<Role> Roles { get; set; } = new();
        [NotMapped] public RepeatedField<Message> PinnedMessages { get; set; } = new();

        public static void SetColumnMetadata(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>()
                .Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            modelBuilder.Entity<Channel>()
                .Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            modelBuilder.Entity<Channel>()
                .Property(x => x.Category)
                .HasColumnName("category");

            modelBuilder.Entity<Channel>()
                .Property(x => x.Description)
                .HasColumnName("description");

            modelBuilder.Entity<Channel>()
                .Property(x => x.CreatedOn)
                .HasColumnName("created_on")
                .IsRequired();

            modelBuilder.Entity<Channel>()
                .Property(x => x.ModifiedOn)
                .HasColumnName("modified_on");
            
            modelBuilder.Entity<Channel>()
                .HasMany(x => x.Messages)
                .WithOne(x => x.Channel)
                .HasForeignKey(x => x.ChannelId);
            
            modelBuilder.Entity<Channel>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Channels)
                .UsingEntity<ChannelsRoles>(
                    j => j.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId),
                    j => j.HasOne(x => x.Channel).WithMany().HasForeignKey(x => x.ChannelId)
                );
            
            modelBuilder.Entity<Channel>()
                .HasMany(c => c.PinnedMessages)
                .WithMany()
                .UsingEntity<ChannelsPinnedMessages>(
                    j => j.HasOne(p => p.Message).WithMany().HasForeignKey(p => p.MessageId),
                    j => j.HasOne(p => p.Channel).WithMany().HasForeignKey(p => p.ChannelId)
                );
        }

        public void MergeFrom(Fragments.Mercury.Channel message)
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

        public bool Equals(Fragments.Mercury.Channel other)
        {
            throw new System.NotImplementedException();
        }

        public Fragments.Mercury.Channel Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
