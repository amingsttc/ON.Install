using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ON.Authentication;
namespace ON.Mercury.Service.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        var token = Context.GetHttpContext().Request.Headers.Authorization;
        var user = ONUserHelper.ParseUser(Context.GetHttpContext());
        _logger.LogInformation($"Connected {user.Id}");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string message)
        => await Clients.Others.SendAsync("ReceiveMessage", message);
}