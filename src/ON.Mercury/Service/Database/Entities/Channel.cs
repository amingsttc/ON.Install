using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using ON.Mercury.Service.Database.UnionTables;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using pb = global::Google.Protobuf;

namespace ON.Mercury.Service.Models.Channels
{
    [Table("channels")]
    public sealed partial class Channel : IMessage<Fragments.Mercury.Channel>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Timestamp CreatedOn { get; set; }
        public Timestamp? ModifiedOn { get; set; }

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
                .HasConversion(
                    v => v.ToDateTime(),
                    v => Timestamp.FromDateTime(v.ToUniversalTime()))
                .IsRequired();

            modelBuilder.Entity<Channel>()
                .Property(x => x.ModifiedOn)
                .HasColumnName("modified_on")
                .HasConversion(
                    v => v.ToDateTime(),
                    v => Timestamp.FromDateTime(v.ToUniversalTime()));
            
            // modelBuilder.Entity<ChannelEntity>()
            //     .HasMany(x => x.Messages)
            //     .WithOne(x => x.Channel)
            //     .HasForeignKey(x => x.ChannelId);
            //
            // modelBuilder.Entity<ChannelEntity>()
            //     .HasMany(x => x.Roles)
            //     .WithMany(x => x.Channels)
            //     .UsingEntity<ChannelsRoles>(
            //         j => j.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId),
            //         j => j.HasOne(x => x.Channel).WithMany().HasForeignKey(x => x.ChannelId)
            //     );

            // modelBuilder.Entity<Channel>()
            //     .HasMany(x => x.Messages)
            //     .WithOne(x => x.Channel)
            //     .HasForeignKey(x => x.ChannelId);
            //
            // modelBuilder.Entity<Channel>()
            //     .HasMany(x => x.Roles)
            //     .WithMany(x => x.Channels)
            //     .UsingEntity<ChannelsRoles>(
            //         j => j.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId),
            //         j => j.HasOne(x => x.Channel).WithMany().HasForeignKey(x => x.ChannelId)
            //     );
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
