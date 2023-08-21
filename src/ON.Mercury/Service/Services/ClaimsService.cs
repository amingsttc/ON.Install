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
using ON.Authentication;

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

        public override async Task<GetClaimsResponse> GetClaims(GetClaimsRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.UserID))
                return new GetClaimsResponse();
            
            var res = new GetClaimsResponse();
            var member = await _members.GetMember(request.UserID);

            if (member is null)
                return new GetClaimsResponse();
            
            var serialized = JsonConvert.SerializeObject(member, settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
                
            res.Claims.Add(new ClaimRecord()
            {
                Name = "MercuryProfile",
                Value = serialized,
                ExpiresOnUTC = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.MaxValue.ToUniversalTime())
            });

            return res;
        }
    }
}
