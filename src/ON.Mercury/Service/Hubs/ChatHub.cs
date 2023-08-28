using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Services;

namespace ON.Mercury.Service.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private ChatInterface.ChatInterfaceClient _chatClient;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
        var channel = GrpcChannel.ForAddress("http://localhost:7015");
        _chatClient = new ChatInterface.ChatInterfaceClient(channel);
    }

    public override Task OnConnectedAsync()
    {
        var user = ONUserHelper.ParseUser(Context.GetHttpContext());
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    // TODO: Create new return type that sets message dates to DateTime
    public async Task SendMessage(SendMessageRequest request)
    {
        var res = _chatClient.SendMessage(request);
        if (!res.IsSuccess)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Error Sending Message");
        }
        else
        {
            await Clients.All.SendAsync("ReceiveMessage", res.Message);
        }
    }
}