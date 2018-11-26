using System;
namespace Communicator
{
    public class Message
    {
        public string ID;
        public string UserName { get; set; }
        public string MessageText { get; set; }
        public bool IsSender { get; set; }
        public bool IsNotification { get; set; }

        public Message(string username, string text)
        {
            UserName = username;
            MessageText = text;
        }

    }
}
