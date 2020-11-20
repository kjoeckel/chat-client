using ChatProtocol;
using System;
using System.Net.Sockets;

namespace ChatClient.MessageHandler
{
    public class ChatMessageHandler : IMessageHandler
    {
        public void Execute(TcpClient client, IMessage message)
        {
            DateTime timeStamp = DateTime.Now;
            ChatMessage chatMessage = message as ChatMessage;
            Console.WriteLine($"{timeStamp} {chatMessage.SenderName} send: {chatMessage.Content}");
        }
    }
}
