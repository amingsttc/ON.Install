using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ON.Mercury.Service.Database.Entities;
using Service.Database.Entities;

namespace ON.Mercury.Service.Database.UnionTables;

[Table("members_roles")]
public class MembersRoles
{
    public string MemberId { get; set; }
    public Member Member { get; set; }
    public string RoleId { get; set; }
    public Role Role { get; set; }

    public static void SetColumnMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MembersRoles>()
            .HasKey(mr => new
            {
                mr.MemberId,
                mr.RoleId
            });
        
        modelBuilder.Entity<MembersRoles>()
            .Property(x => x.MemberId)
            .HasColumnName("member_id");

        modelBuilder.Entity<MembersRoles>()
            .Property(x => x.RoleId)
            .HasColumnName("role_id");

        modelBuilder.Entity<MembersRoles>()
            .HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .HasConstraintName("FK_MembersRoles_Member") // Descriptive name for Member relationship
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MembersRoles>()
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .HasConstraintName("FK_MembersRoles_Role") // Descriptive name for Role relationship
            .OnDelete(DeleteBehavior.Restrict);
    }
}