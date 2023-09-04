#nullable enable
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Caching;
using Service.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ON.Mercury.Service.Exceptions;
using ON.Mercury.Service.Hubs;
using Role = Service.Database.Entities.Role;

namespace ON.Mercury.Service.Database.Repositories
{
    public class RoleRepository
    {
        // TODO: Look Into CQRS
        private readonly ILogger<RoleRepository> _logger;
        private readonly PostgresContext _postgres;
        private readonly ICachingService _cache;
        private readonly IHubContext<ChatHub> _hubContext;
        
        public RoleRepository(ILogger<RoleRepository> logger, PostgresContext postgres, ICachingService cache, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _postgres = postgres;
            _cache = cache;
            _hubContext = hubContext;
        }

        public async Task<IReadOnlyList<Role>?> GetRolesAsync(CancellationToken  cancellationToken = default)
        {
            // Check the cache for stored roles
            // var roles = await _cache.GetAsync("roles", async () =>
            // {
            //     // If no cached roles, check the db
            //     var rolesFromDb = await _postgres.Roles.ToListAsync();
            //     return rolesFromDb;
            // });

            // return roles;
            var rolesFromDb = _postgres.Roles.ToList();
            return rolesFromDb;
        }

        public async Task<string?> CreateRoleAsync(string name, MapField<string, bool> permissions, int hierarchy, CancellationToken cancellationToken = default)
        {
            var roleEntry = await _postgres.Roles.AddAsync(new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Permissions = permissions,
                Hierarchy = hierarchy,
                CreatedOn =  DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            }, cancellationToken);
            await _postgres.SaveChangesAsync(cancellationToken);
            var role = roleEntry.Entity;
            await _hubContext.Clients.All.SendAsync("RoleCreated", JsonConvert.SerializeObject(role), cancellationToken);
            
            return role.Id;
        }

        public async Task<Role?> UpdateRoleAsync(string id, string name, MapField<string, bool> permissions, int hierarchy, CancellationToken cancellationToken = default)
        {
            var role = await _postgres.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
            if (role is null) throw new NotFoundException("Role", id);

            role.Name = name;
            role.Permissions = permissions;
            role.Hierarchy = hierarchy;

            _postgres.Roles.Update(role);
            await _postgres.SaveChangesAsync(cancellationToken);
            await _hubContext.Clients.All.SendAsync("RoleUpdated", JsonConvert.SerializeObject(role), cancellationToken);
    
            return role;
        }

        public async Task<string?> DeleteRoleAsync(string id, CancellationToken cancellationToken = default)
        {
            var role = await _postgres.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
            if (role is null) throw new NotFoundException("Role", id);

            _postgres.Roles.Remove(role);
            await _postgres.SaveChangesAsync(cancellationToken);
            await _hubContext.Clients.All.SendAsync("RoleDeleted", role.Id, cancellationToken);

            return role.Id;
        }
    }
}
