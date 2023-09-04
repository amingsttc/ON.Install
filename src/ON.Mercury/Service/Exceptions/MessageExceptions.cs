using System;

namespace ON.Mercury.Service.Exceptions
{
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException(string messageId) : base($"Message {messageId} Not Found") {}
    }

    public class MessageNotPinnedException : Exception
    {
        public MessageNotPinnedException(string messageId) : base ($"Message {messageId} not pinned") {}
    }
}
