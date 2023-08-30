using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Caching;
using ON.Mercury.Service.Database.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ON.Mercury.Service.Hubs;
using Service.Database.Entities;

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
            var member = await GetMember(id);
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
    }
}
