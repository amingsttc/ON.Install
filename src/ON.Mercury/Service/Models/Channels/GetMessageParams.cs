using System;

namespace ON.Mercury.Service.Models.Channels
{
    [Flags]
    public enum MessageSenderParams
    {
        SenderId = 0,
        Full = 1,
    }
    public class GetMessageParams
    {
        public MessageSenderParams? MessageSender { get; set; }
    }
}
