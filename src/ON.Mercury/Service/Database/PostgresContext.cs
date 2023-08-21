using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ON.Mercury.Service.Database.Entities;
using ON.Mercury.Service.Database.UnionTables;
using Service.Database.Entities;

namespace ON.Mercury.Service.Database;

public sealed class PostgresContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<ChannelEntity> Channels { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<MemberEntity> Members { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<ChannelsRoles> ChannelRoles { get; set; }
    public DbSet<MembersRoles> MemberRoles { get; set; }

    public PostgresContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Channels = Set<ChannelEntity>();
        Messages = Set<MessageEntity>();
        Members = Set<MemberEntity>();
        Roles = Set<RoleEntity>();
        ChannelRoles = Set<ChannelsRoles>();
        MemberRoles = Set<MembersRoles>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ChannelEntity.SetColumnMetadata(modelBuilder);
        MessageEntity.SetColumnMetadata(modelBuilder);
        MemberEntity.SetColumnMetadata(modelBuilder);
        RoleEntity.SetColumnMetadata(modelBuilder);
        ChannelsRoles.SetColumnMetadata(modelBuilder);
        MembersRoles.SetColumnMetadata(modelBuilder);
    }
}