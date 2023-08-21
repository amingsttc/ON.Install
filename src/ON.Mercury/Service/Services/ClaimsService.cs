using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ON.Fragments.Authorization;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database.Repositories;
using System;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Services
{
    public class ClaimsService : ClaimsInterface.ClaimsInterfaceBase
    {
        private readonly ILogger<ClaimsService> _logger;
        private readonly MemberRepository _members;
        
        public ClaimsService(ILogger<ClaimsService> logger, MemberRepository members)
        {
            _logger = logger;
            _members = members;
        }

        public async Task<GetClaimsResponse> GetClaims(GetClaimsRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.UserID))
                return new GetClaimsResponse();

            var res = new GetClaimsResponse();
            var member = await _members.GetMember(request.UserID);

            if (member is null)
                return new GetClaimsResponse();
            
            res.Claims.Add(new ClaimRecord()
            {
                Name = "MercuryUsername",
                Value = member.Username,
                ExpiresOnUTC = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.MaxValue)
            });

            if (!member.Roles.IsNullOrEmpty())
            {
                RepeatedField<Role> roles = new RepeatedField<Role>();
                foreach (var role in member.Roles)
                {
                    //  TODO: Maybe add a claim for each role to reduce Parse time
                    roles.Add(role.ToPb());
                }

                res.Claims.Add(new ClaimRecord()
                {
                    Name = "MercuryRoles",
                    Value = JsonConvert.SerializeObject(roles)
                });
            }

            return res;
        }
    }
}
