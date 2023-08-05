using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;

namespace ON.Mercury.Service.Services;

public class MemberService : MemberInterface.MemberInterfaceBase
{
    private readonly ILogger<MemberService> _logger;
    private readonly PostgresContext _postgres;

    public MemberService(ILogger<MemberService> logger, PostgresContext postgres)
    {
        _logger = logger;
        _postgres = postgres;
    }

    public override Task<CreateMemberResponse> CreateMember(CreateMemberRequest request, ServerCallContext context)
    {
        return base.CreateMember(request, context);
    }

    public override Task<GetMemberResponse> GetMember(GetMemberRequest request, ServerCallContext context)
    {
        return base.GetMember(request, context);
    }

    public override Task<UpdateMemberResponse> UpdateMember(UpdateMemberRequest request, ServerCallContext context)
    {
        return base.UpdateMember(request, context);
    }
}