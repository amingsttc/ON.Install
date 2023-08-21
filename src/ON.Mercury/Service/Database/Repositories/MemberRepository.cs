using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Caching;
using ON.Mercury.Service.Database.Entities;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ON.Mercury.Service.Database.Repositories
{
    public class MemberRepository
    {
        private readonly ILogger<MemberRepository> _logger;
        private readonly ICachingService _cache;
        private readonly PostgresContext _postgres;
        
        public MemberRepository(ICachingService cache, ILogger<MemberRepository> logger, PostgresContext postgres)
        {
            _cache = cache;
            _logger = logger;
            _postgres = postgres;
        }

        public async Task<MemberEntity> CreateMember(string id, string username) 
        {
            var newMember = new MemberEntity()
            {
                Id = id,
                Username = username,
            };
            await _postgres.Members.AddAsync(newMember);
            await _postgres.SaveChangesAsync();

            return newMember;
        }

        public async Task<MemberEntity> GetOrCreateMember(string id, string username)
        {
            var member = await _postgres.Members.FindAsync(id);
            if (member is not null)
            {
                return member;
            }

            member = await CreateMember(id, username);
            return member;
        }

        public async Task<MemberEntity?> GetMember(string id)
        {
            var member = await _postgres.Members.Where(m => m.Id == id).Include(m => m.Roles).FirstOrDefaultAsync();
            _logger.LogInformation(JsonConvert.SerializeObject(member));
            return member;
        }
    }
}
