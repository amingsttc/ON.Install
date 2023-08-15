using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Database.Entities;
using Service.Database.Entities;

namespace ON.Mercury.Service.Services;

[Authorize]
public class MemberService : MemberInterface.MemberInterfaceBase
{
    private readonly ILogger<MemberService> _logger;
    private readonly PostgresContext _postgres;

    public MemberService(ILogger<MemberService> logger, PostgresContext postgres)
    {
        _logger = logger;
        _postgres = postgres;
    }
    
    public override async Task<CreateMemberResponse> CreateMember(CreateMemberRequest request, ServerCallContext context)
    {
        try
        {
            var newMember = new MemberEntity()
            {
                Id = request.UserId,
                Username = request.Username
            };

            await _postgres.Members.AddAsync(newMember);
            await _postgres.SaveChangesAsync();
            
            return new CreateMemberResponse()
            {
                IsSuccess = true,
                Error = "",
                Member = newMember.ToPb()
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(e));
            return new CreateMemberResponse()
            {
                IsSuccess = false
            };
        }
    }

    public override async Task<GetMemberResponse> GetMember(GetMemberRequest request, ServerCallContext context)
    {
        try
        {
            var member = await _postgres.Members.Where(m => m.Id == request.MemberId).FirstOrDefaultAsync();
            if (member is null)
            {
                return new GetMemberResponse()
                {
                    IsSuccess = false,
                    Error = "Member Not Found"
                };
            }

            return new GetMemberResponse()
            {
                IsSuccess = true,
                Error = "",
                Member = member.ToPb()
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return new GetMemberResponse()
            {
                IsSuccess = false
            };
        }
    }

    public override Task<GetMemberResponse> GetMembers(GetMembersRequest request, ServerCallContext context)
    {
        return base.GetMembers(request, context);
    }

    public override async Task<UpdateMemberResponse> UpdateMember(UpdateMemberRequest request, ServerCallContext context)
    {
        try
        {
            var foundMember = await _postgres.Members.Where(m => m.Id == request.MemberId).FirstOrDefaultAsync();
            if (foundMember is null)
            {
                return new UpdateMemberResponse()
                {
                    IsSuccess = false,
                    Error = "Member not Found"
                };
            }

            foundMember.Username = request.Username;

            if (request.Roles.Count > 0)
            {
                var roles = new List<RoleEntity>();
                foreach (var role in request.Roles)
                {
                    var json = JsonConvert.SerializeObject(role);
                    var entity = JsonConvert.DeserializeObject<RoleEntity>(json);
                    roles.Add(entity);
                }

                foundMember.Roles = roles;
            }

            _postgres.Members.Update(foundMember);
            await _postgres.SaveChangesAsync();
            
            return new UpdateMemberResponse()
            {
                IsSuccess = true,
                Error = "",
                Member = foundMember.ToPb()
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(e));
            return new UpdateMemberResponse()
            {
                IsSuccess = false
            };
        }
    }

    public override async Task<DeleteMemberResponse> DeleteMember(DeleteMemberRequest request, ServerCallContext context)
    {
        try
        {
            var foundMember = await _postgres.Members.Where(m => m.Id == request.MemberId).FirstOrDefaultAsync();
            if (foundMember is null)
            {
                return new DeleteMemberResponse()
                {
                    IsSuccess = false,
                    Error = "Member not found",
                    DeletedMember = ""
                };
            }

            _postgres.Members.Remove(foundMember);
            await _postgres.SaveChangesAsync();
            return new DeleteMemberResponse()
            {
                IsSuccess = true,
                Error = "",
                DeletedMember = foundMember.Id
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}