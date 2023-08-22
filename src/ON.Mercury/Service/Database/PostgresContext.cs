using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database.Entities;
using ON.Mercury.Service.Database.UnionTables;
using Service.Database.Entities;
using System;
using Channel = ON.Mercury.Service.Database.Entities.Channel;
using Member = Service.Database.Entities.Member;
using Role = Service.Database.Entities.Role;

namespace ON.Mercury.Service.Database;

public sealed class PostgresContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Channel> Channels { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ChannelsRoles> ChannelRoles { get; set; }
    public DbSet<MembersRoles> MemberRoles { get; set; }
    public DbSet<AuditItem> AuditLog { get; set; }

    public PostgresContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Channels = Set<Channel>();
        Messages = Set<MessageEntity>();
        Members = Set<Member>();
        Roles = Set<Role>();
        ChannelRoles = Set<ChannelsRoles>();
        MemberRoles = Set<MembersRoles>();
        AuditLog = Set<AuditItem>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //ChannelEntity.SetColumnMetadata(modelBuilder);
        MessageEntity.SetColumnMetadata(modelBuilder);
        Member.SetColumnMetadata(modelBuilder);
        Role.SetColumnMetadata(modelBuilder);
        ChannelsRoles.SetColumnMetadata(modelBuilder);
        MembersRoles.SetColumnMetadata(modelBuilder);
        Channel.SetColumnMetadata(modelBuilder);
        

        modelBuilder.Entity<AuditItem>()
            .ToTable("audit_items");
        
        modelBuilder.Entity<AuditItem>()
            .HasKey(i => i.Id)
            .HasName("PK_audit_item_id");
        
        modelBuilder.Entity<AuditItem>()
            .Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        modelBuilder.Entity<AuditItem>()
            .Property(i => i.CallerId)
            .HasColumnName("caller_id");

        modelBuilder.Entity<AuditItem>()
            .Property(i => i.Action)
            .HasColumnName("action");
        
        modelBuilder.Entity<AuditItem>()
            .Property(i => i.CreatedOn)
            .HasColumnName("created_on")
            .HasConversion(
                v => v.ToDateTime(),
                v => Timestamp.FromDateTime(v.ToUniversalTime())); 
    }
}