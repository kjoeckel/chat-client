using ChatProtocol;
using System;
using System.Net.Sockets;

namespace ChatClient.MessageHandler
{
    public class DisconnectMessageHandler : IMessageHandler
    {
        public void Execute(TcpClient client, IMessage message)
        {
            DisconnectMessage disconnectMessage = message as DisconnectMessage;
            Program.IsConnected = false;
            Program.SessionId = null;
        Console.WriteLine($"Disconnecting");
        }
    }
}
