using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ON.Mercury.Service.Database.Entities;
using ON.Mercury.Service.Models.Channels;
using Service.Database.Entities;

namespace ON.Mercury.Service.Database.UnionTables;

[Table("channels_roles")]
public class ChannelsRoles
{
    public string ChannelId { get; set; }
    public Channel Channel { get; set; }
    public string RoleId { get; set; }
    public Role Role { get; set; }

    public static void SetColumnMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChannelsRoles>()
            .HasKey(cr => new { cr.ChannelId, cr.RoleId });
        
        modelBuilder.Entity<ChannelsRoles>()
            .Property(x => x.ChannelId)
            .HasColumnName("channel_id");

        modelBuilder.Entity<ChannelsRoles>()
            .Property(x => x.RoleId)
            .HasColumnName("role_id");

        modelBuilder.Entity<ChannelsRoles>()
            .HasOne(x => x.Channel)
            .WithMany()
            .HasForeignKey(x => x.ChannelId)
            .HasConstraintName("FK_channel_id")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChannelsRoles>()
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .HasConstraintName("FK_role_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}