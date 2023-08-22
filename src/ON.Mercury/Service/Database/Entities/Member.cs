using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Microsoft.EntityFrameworkCore;
using ON.Mercury.Service.Database.UnionTables;

namespace Service.Database.Entities
{
    [Table("members")]
    public sealed partial class Member : IMessage<Member>
    {
        public string Id { get; set; }
        public string Username { get; set; }
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
        
        public void MergeFrom(Member message)
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

        public bool Equals(Member other)
        {
            throw new System.NotImplementedException();
        }

        public Member Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
