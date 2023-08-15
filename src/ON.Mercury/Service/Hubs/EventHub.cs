using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Services;

namespace ON.Mercury.Service.Hubs;

public class EventHub : Hub
{
    private readonly ILogger<EventHub> _logger;
    private readonly MemberStateProvider _memberStateProvider;

    public EventHub(ILogger<EventHub> logger, MemberStateProvider memberStateProvider)
    {
        _logger = logger;
        _memberStateProvider = memberStateProvider;
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendEvent(string user, string @event)
        => await Clients.All.SendAsync("ReceiveMessage", user, @event);

    public async Task SendEventToCaller(string user, string @event)
        => await Clients.Caller.SendAsync("ReceiveMessage", user, @event);
}