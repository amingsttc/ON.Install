using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ON.Mercury.Service.Database.UnionTables;

namespace Service.Database.Entities
{
    [Table("members")]
    public class Member 
    {
        public string Id { get; set; }
        public string Username { get; set; }
        [NotMapped]
        public RepeatedField<Role> Roles { get; set; } = new();

        public static void SetColumnMetadata(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasKey(e => e.Id)
                .HasName("PK_members");

            modelBuilder.Entity<Member>()
                .Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            modelBuilder.Entity<Member>()
                .Property(e => e.Username)
                .HasColumnName("username")
                .IsRequired();

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Members)
                .UsingEntity<MembersRoles>();
        }
    }
}
