using ChatProtocol;
using System;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.MessageHandler
{
    public class UserCountMessageHandler : IMessageHandler
    {
        public void Execute(TcpClient client, IMessage message)
        {
            UserCountMessage userCountMessage = message as UserCountMessage;
            StringBuilder sb = new StringBuilder();
            foreach (string username in userCountMessage.UserNames)
            {
                if (username != null)
                {
                sb.Append($"\"{username}\" ");
                }
            }
            string usernames = sb.ToString();
            Console.WriteLine($"Online users: {usernames} ");
            Console.WriteLine($"Total (online / sum): {userCountMessage.UserOnlineCount} / " +
            $"{userCountMessage.UserCount}");
        }
    }
}
