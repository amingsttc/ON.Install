#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database.UnionTables;
using Service.Database.Entities;

namespace ON.Mercury.Service.Database.Entities;

[Table("members")]
public class MemberEntity : IPostgresEntity<Member, MemberEntity>
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();

    public MemberEntity Clone()
    {
        return new MemberEntity()
        {
            Id = Id,
            Username = Username
        };
    }

    public MemberEntity FromPb(Member memberProto)
    {
        return new MemberEntity()
        {
            Id = memberProto.Id,
            Username = memberProto.Username
        };
    }

    public Member ToPb()
    {
        // TODO: Figure out how to speed this up
        var json = JsonConvert.SerializeObject(this);
        return Google.Protobuf.JsonParser.Default.Parse<Member>(json);
    }
    
    public static void SetColumnMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MemberEntity>()
            .HasKey(e => e.Id)
            .HasName("PK_members");

        modelBuilder.Entity<MemberEntity>()
            .Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        modelBuilder.Entity<MemberEntity>()
            .Property(e => e.Username)
            .HasColumnName("username")
            .IsRequired();

        modelBuilder.Entity<MemberEntity>()
            .HasMany(e => e.Roles)
            .WithMany(e => e.Members)
            .UsingEntity<MembersRoles>();
    }
}