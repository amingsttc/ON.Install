using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ON.Mercury.Service.Database.Entities;

namespace Service.Database.Entities
{
    [Table("roles")]
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Hierarchy { get; set; }
        [NotMapped]
        public MapField<string, bool> Permissions { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedOn { get; set; }
        [JsonIgnore]
        [NotMapped]
        public RepeatedField<Channel> Channels { get; set; } = new();
        [NotMapped]
        public RepeatedField<Member> Members { get; set; } = new();
        public static void SetColumnMetadata(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.Hierarchy)
                .HasColumnName("hierarchy")
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.CreatedOn)
                .HasColumnName("created_on")
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.ModifiedOn)
                .HasColumnName("modified_on");

            modelBuilder.Entity<Role>()
                .Property(x => x.DeletedOn)
                .HasColumnName("deleted_on");

            modelBuilder.Entity<Role>()
                .Property(x => x.Permissions)
                .HasColumnName("permissions")
                .HasColumnType("jsonb")
                .IsRequired();
        }

        private static Dictionary<string, bool> ToDictionary(MapField<string, bool> dataIn)
        {
            var dict = new Dictionary<string, bool>();
            foreach (var kvp in dataIn)
            {
                dict.Add(kvp.Key, kvp.Value);
            }
            return dict;
        }

        private static MapField<string, bool> FromDict(Dictionary<string, bool> dataIn)
        {
            var dict = new MapField<string, bool>();
            foreach (var kvp in dataIn)
            {
                dict.Add(kvp.Key, kvp.Value);
            }
            return dict;
        }
    }
}
