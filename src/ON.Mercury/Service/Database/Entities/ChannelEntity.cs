#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Database.UnionTables;

namespace Service.Database.Entities;

[Table("channels")]
public class ChannelEntity : IPostgresEntity<Channel, ChannelEntity>
{
    public string Id { get; set;}
    public string Name { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedOn { get; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; set; }
    
    
    [NotMapped] public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    [NotMapped] public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();

    public ChannelEntity(string name, string category = "", string description = "")
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Category = category;
        Description = description;
    }
    
    public ChannelEntity Clone()
    {
        throw new System.NotImplementedException();
    }

    public ChannelEntity FromPb(Channel proto)
    {
        throw new System.NotImplementedException();
    }

    public Channel ToPb()
    {
        return new Channel()
        {
            Id = Id,
            Name = Name,
            Category = Category,
            Description = Description,
            CreatedOn = Timestamp.FromDateTime(CreatedOn)
        };
    }

    public static void SetColumnMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChannelEntity>()
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        modelBuilder.Entity<ChannelEntity>()
            .Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        modelBuilder.Entity<ChannelEntity>()
            .Property(x => x.Category)
            .HasColumnName("category");

        modelBuilder.Entity<ChannelEntity>()
            .Property(x => x.Description)
            .HasColumnName("description");

        modelBuilder.Entity<ChannelEntity>()
            .Property(x => x.CreatedOn)
            .HasColumnName("created_on")
            .IsRequired();

        modelBuilder.Entity<ChannelEntity>()
            .Property(x => x.ModifiedOn)
            .HasColumnName("modified_on")
            .IsRequired();

        modelBuilder.Entity<ChannelEntity>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Channel)
            .HasForeignKey(x => x.ChannelId);
        
        modelBuilder.Entity<ChannelEntity>()
            .HasMany(x => x.Roles)
            .WithMany(x => x.Channels)
            .UsingEntity<ChannelsRoles>(
                j => j.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId),
                j => j.HasOne(x => x.Channel).WithMany().HasForeignKey(x => x.ChannelId)
            );
    }
}