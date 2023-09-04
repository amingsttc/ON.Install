using Grpc.Net.Client;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Caching;
using ON.Mercury.Service.Database.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ON.Fragments.Authentication;
using ON.Mercury.Service.Exceptions;
using ON.Mercury.Service.Hubs;
using Service.Database.Entities;
using System;
using System.Threading;

namespace ON.Mercury.Service.Database.Repositories
{
    public class MemberRepository
    {
        private readonly ILogger<MemberRepository> _logger;
        private readonly ICachingService _cache;
        private readonly PostgresContext _postgres;
        private readonly IHubContext<ChatHub> _hubContext;
        
        public MemberRepository(ICachingService cache, ILogger<MemberRepository> logger, PostgresContext postgres, IHubContext<ChatHub> hubContext)
        {
            _cache = cache;
            _logger = logger;
            _postgres = postgres;
            _hubContext = hubContext;
        }

        public async Task<Member> CreateMember(string id, string username) 
        {
            var newMember = new Member()
            {
                Id = id,
                Username = username,
            };
            await _postgres.Members.AddAsync(newMember);
            await _postgres.SaveChangesAsync();

            return newMember;
        }

        public async Task<Member> GetOrCreateMember(string id, string username)
        {
            Member? member = await GetMember(id);
            if (member is null)
            {
                member = await CreateMember(id, username);
                return member;
            }

            return member;
        }

        public async Task<Member?> GetMember(string id)
        {
            var member = await _postgres.Members.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (member is null)
                return null;
            
            var roles = await _postgres.MemberRoles.Where(m => m.MemberId == member.Id).Include(m => m.Role).ToListAsync();
            foreach (var memberRole in roles)
            {
                member.Roles.Add(memberRole.Role);
            }
            
            return member;
        }

        public async Task<IEnumerable<Member>> GetMembers()
        {
            var members = await _postgres.Members.ToListAsync();
            return members;
        }

        // TODO: Handle Duplicate Update Exception
        // TODO: Refresh Token and return
        public async Task<Member?> GrantRolesAsync(string memberId, IEnumerable<string> roleIds, CancellationToken cancellationToken = default)
        {
            
            var member = await _postgres.Members.FirstOrDefaultAsync(m => m.Id ==  memberId,  cancellationToken);
            if (member is null)
            {
                throw new NotFoundException("Member", memberId);
            }
            
            var matchingRoles = await _postgres.Roles
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if (!matchingRoles.Any())
            {
                throw new Exception("No Matching Roles Found");
            }
            
            member.Roles.Add(matchingRoles);
            _postgres.Members.Update(member);
            await _postgres.SaveChangesAsync(cancellationToken);
            
            return member;
        }

        // TODO: Refresh Token and return
        public async Task<Member?> RemoveRolesAsync(string memberId, IEnumerable<string> roleIds, CancellationToken cancellationToken = default)
        {

            var member = await _postgres.Members.Include(member => member.Roles).FirstOrDefaultAsync(m => m.Id == memberId, cancellationToken);
            if (member is null)
            {
                throw new NotFoundException("Member", memberId);
            }

            foreach (var roleId in roleIds)
            {
                var role = member.Roles.FirstOrDefault(r => r.Id == roleId);
                if (role is not null)
                {
                    member.Roles.Remove(role);
                }
            }
            
            _postgres.Members.Update(member);
            await _postgres.SaveChangesAsync(cancellationToken);
            
            return member;
        }
    }
}
