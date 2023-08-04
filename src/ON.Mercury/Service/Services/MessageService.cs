using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ON.Fragments.Mercury;

namespace ON.Mercury.Service.Services;

public class MessageService : MessageInterface.MessageInterfaceBase
{
    private readonly ILogger<MessageService> _logger;

    public MessageService(ILogger<MessageService> logger)
    {
        _logger = logger;
    }

    public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        return base.SendMessage(request, context);
    }

    public override Task<GetMessagesResponse> GetMessages(GetMessagesRequest request, ServerCallContext context)
    {
        return base.GetMessages(request, context);
    }

    public override Task<UpdateMessageResponse> UpdateMessage(UpdateMessageRequest request, ServerCallContext context)
    {
        return base.UpdateMessage(request, context);
    }

    public override Task<DeleteMessageResponse> DeleteMessage(DeleteMessageRequest request, ServerCallContext context)
    {
        return base.DeleteMessage(request, context);
    }
}