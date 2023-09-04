using System;

namespace ON.Mercury.Service.Exceptions
{
    public class ChannelNotFoundException : Exception
    {
        public ChannelNotFoundException(string channelId) :  base($"Channel {channelId} not found")
        {
        }
    }
}
