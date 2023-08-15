#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Database.Entities;

namespace Service.Database.Entities;

[Table("roles")]
public class RoleEntity : IPostgresEntity<Role, RoleEntity>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Hierarchy { get; set; }
    public Dictionary<string, bool> Permissions { get; set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; set; }

    [NotMapped] public ICollection<MemberEntity> Members { get; set; } = new List<MemberEntity>();
    [NotMapped] public ICollection<ChannelEntity> Channels { get; set; } = new List<ChannelEntity>();

    public RoleEntity(string name, int hierarchy)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Hierarchy = hierarchy;
        CreatedOn = DateTime.UtcNow;
    }
    
    public RoleEntity() {}
    
    public RoleEntity Clone()
    {
        return new RoleEntity()
        {
            Id = Id,
            Name = Name,
            Hierarchy = Hierarchy,
            CreatedOn = CreatedOn,
            ModifiedOn = ModifiedOn
        };
    }

    public RoleEntity FromPb(Role proto)
    {
        return new RoleEntity()
        {
            Id = proto.Id,
            Name = proto.Name,
            Hierarchy = proto.Hierarchy
        };
    }

    public Role ToPb()
    {
        // TODO: Figure out how to speed this up
        var json = JsonConvert.SerializeObject(this);
        var proto = Google.Protobuf.JsonParser.Default.Parse<Role>(json);
        return proto;
    }

    public static void SetColumnMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleEntity>()
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        modelBuilder.Entity<RoleEntity>()
            .Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        modelBuilder.Entity<RoleEntity>()
            .Property(x => x.Hierarchy)
            .HasColumnName("hierarchy")
            .IsRequired();

        modelBuilder.Entity<RoleEntity>()
            .Property(x => x.CreatedOn)
            .HasColumnName("created_on")
            .IsRequired();

        modelBuilder.Entity<RoleEntity>()
            .Property(x => x.ModifiedOn)
            .HasColumnName("modified_on");

        modelBuilder.Entity<RoleEntity>()
            .Property(x => x.Permissions)
            .HasColumnName("permissions")
            .HasColumnType("jsonb")
            .IsRequired();
    }
}