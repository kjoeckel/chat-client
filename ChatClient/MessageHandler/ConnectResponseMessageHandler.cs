using ChatProtocol;
using System;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.MessageHandler
{
    public class ConnectResponseMessageHandler : IMessageHandler
    {
        public void Execute(TcpClient client, IMessage message)
        {
            ConnectResponseMessage connectResponseMessage = message as ConnectResponseMessage;
            if (connectResponseMessage.Success)
            {
                Program.IsConnected = true;
                Program.SessionId = connectResponseMessage.SessionId;
                DateTime timeStamp = DateTime.Now;

                Console.WriteLine($"{timeStamp} Connected! Session Id: {Program.SessionId}");
                Console.WriteLine("Type in your message. " +
                                "for other options type \"/disconnect\" or \"/exit\" .");
            }
            else
            {
                Console.WriteLine("Connection failed!");
            }

            Program.IsConnecting = false;
        }
    }
}
