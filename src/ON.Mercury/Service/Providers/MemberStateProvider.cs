using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ON.Mercury.Service.Services;

public enum ConnectionStates
{
    Online = 1,
    Away = 2,
    Offline = 3,
    Default = 4
}

public class MemberStateProvider
{
    private readonly ILogger<MemberStateProvider> _logger;
    private ConcurrentDictionary<string, ConnectionStates> _connectionStates;
    private object _connectionStatesLock;
    
    public MemberStateProvider(ILogger<MemberStateProvider> logger, MemberService memberService)
    {
        _logger = logger;
        _connectionStates = new ConcurrentDictionary<string, ConnectionStates>();
        _connectionStatesLock = new object();
    }

    public void SetConnectionState(string member, ConnectionStates connectionState)
    {
        lock (_connectionStatesLock)
        {
            
            var found = _connectionStates.TryGetValue(member, out var foundConnectionState);
            if (found)
            {
                _connectionStates.TryUpdate(member, connectionState, foundConnectionState);
            }
            else
            {
                _connectionStates.TryAdd(member, connectionState);
            }
        }
    }

    public ImmutableDictionary<string, ConnectionStates> GetConnectionStates()
    {
        lock (_connectionStatesLock)
        {
            return _connectionStates.ToImmutableDictionary();
        }
    }
}