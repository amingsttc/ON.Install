using Grpc.Core;
using Microsoft.Extensions.Logging;
using ON.Fragments.Mercury;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Services
{
    public class EventService : EventInterface.EventInterfaceBase
    {
        private readonly ILogger<EventService> _logger;
        private ConcurrentDictionary<string, Guid> _membersSubscriptions;
        
        public EventService(ILogger<EventService> logger)
        {
            _logger = logger;
            _membersSubscriptions = new ConcurrentDictionary<string, Guid>();
        }

        public override Task Subscribe(SubscribeRequest request, IServerStreamWriter<MercuryEvent> responseStream, ServerCallContext context)
        {
            return base.Subscribe(request, responseStream, context);
        }

        public override Task<UnsubscribeResponse> Unsubscribe(UnsubscribeRequest request, ServerCallContext context)
        {
            return base.Unsubscribe(request, context);
        }

        public override Task Reconnect(ReconnectRequest request, IServerStreamWriter<MercuryEvent> responseStream, ServerCallContext context)
        {
            return base.Reconnect(request, responseStream, context);
        }

        public override Task<QueueEventResponse> QueueEvent(QueueEventRequest request, ServerCallContext context)
        {
            return base.QueueEvent(request, context);
        }
    }
}
