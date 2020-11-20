using ChatProtocol;
using System;
using System.Net.Sockets;

namespace ChatClient.MessageHandler
{
    public class UserCountMessageHandler : IMessageHandler
    {
        public void Execute(TcpClient client, IMessage message)
        {
            UserCountMessage userCountMessage = message as UserCountMessage;
            Console.WriteLine($"{userCountMessage.UserName} is now online." +
                $" Users (online / sum): {userCountMessage.UserOnlineCount} / " +
                $"{userCountMessage.UserCount}");
        }
    }
}
