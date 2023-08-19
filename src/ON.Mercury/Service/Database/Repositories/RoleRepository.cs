#nullable enable
using Google.Rpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Caching;
using Service.Database.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Database.Repositories
{
    public class RoleRepository
    {
        // TODO: Look Into CQRS
        private readonly ILogger<RoleRepository> _logger;
        private readonly PostgresContext _postgres;
        private readonly ICachingService _cache;
        
        public RoleRepository(ILogger<RoleRepository> logger, PostgresContext postgres, ICachingService cache)
        {
            _logger = logger;
            _postgres = postgres;
            _cache = cache;
        }

        public async Task<IReadOnlyList<RoleEntity>?> GetRolesAsync(CancellationToken  cancellationToken = default)
        {
            // Check the cache for stored roles
            // var roles = await _cache.GetAsync("roles", async () =>
            // {
            //     // If no cached roles, check the db
            //     var rolesFromDb = await _postgres.Roles.ToListAsync();
            //     return rolesFromDb;
            // });

            // return roles;
            var rolesFromDb = await _postgres.Roles.ToListAsync(cancellationToken: cancellationToken);
            return rolesFromDb;
        }

        public async Task<string?> CreateRoleAsync(string name, Dictionary<string, bool> permissions, int hierarchy, CancellationToken cancellationToken = default)
        {
            var roleEntry = await _postgres.Roles.AddAsync(new RoleEntity(name, hierarchy)
            {
                Permissions = permissions
            }, cancellationToken);
            await _postgres.SaveChangesAsync(cancellationToken);
            var role = roleEntry.Entity;
            // await _cache.AddOrSetAsync("roles", new List<RoleEntity>()
            // {
            //     role
            // });
            return role.Id;
        }

        public async Task<RoleEntity?> UpdateRoleAsync(string id, string name, Dictionary<string, bool> permissions, int hierarchy, CancellationToken cancellationToken = default)
        {
            var role = await _postgres.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
            if (role is null) return null;

            role.Name = name;
            role.Permissions = permissions;
            role.Hierarchy = hierarchy;

            _postgres.Roles.Update(role);
            await _postgres.SaveChangesAsync(cancellationToken);

            return role;
        }

        public async Task<string?> DeleteRoleAsync(string id, CancellationToken cancellationToken = default)
        {
            var role = await _postgres.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
            if (role is null) return null;

            _postgres.Roles.Remove(role);
            await _postgres.SaveChangesAsync(cancellationToken);

            return role.Id;
        }
    }
}
