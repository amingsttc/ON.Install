using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Mercury.Service.Services;

namespace ON.Mercury.Service.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly ChatService _chatService;

    public ChatHub(ILogger<ChatHub> logger, ChatService chatService)
    {
        _logger = logger;
        _chatService = chatService;
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

    public async Task SendMessage(string message)
        => await Clients.Others.SendAsync("ReceiveMessage", message);
}