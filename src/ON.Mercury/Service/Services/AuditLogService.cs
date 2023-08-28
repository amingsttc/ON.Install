using Grpc.Core;
using Grpc.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ON.Authentication;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using System;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Services
{
    public class AuditLogService : AuditLogInterface.AuditLogInterfaceBase
    {
        private readonly ILogger<AuditLogService> _logger;
        private readonly PostgresContext _postgres;
        
        public AuditLogService(ILogger<AuditLogService> logger, PostgresContext postgres)
        {
            _logger = logger;
            _postgres = postgres;
        }

        public override async Task<CreateAuditResponse> CreateAudit(CreateAuditRequest request, ServerCallContext context)
        {
            var logItem = new AuditItem()
            {
                Id = Guid.NewGuid().ToString(),
                CallerId = request.CallerId,
                Action = request.Action,
                CreatedOn = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
            };
            
            await _postgres.AuditLog.AddAsync(logItem);
            await _postgres.SaveChangesAsync();

            return new CreateAuditResponse()
            {
                Data = logItem
            };
        }

        // TODO: Add Filtering to the request
        public override async Task<GetAuditLogResponse> GetAuditLog(GetAuditLogRequest request, ServerCallContext context)
        {
            var items = await _postgres.AuditLog.ToListAsync();
            var res = new GetAuditLogResponse();
            res.Data.Add(items);

            return res;
        }
    }
}
