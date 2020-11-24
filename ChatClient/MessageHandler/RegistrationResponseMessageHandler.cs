using ChatProtocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatClient.MessageHandler
{
    public class RegistrationResponseMessageHandler : IMessageHandler
    {
        public void Execute(TcpClient client, IMessage message)
        {
            var registrationResponseMessage = message as RegistrationResponseMessage;

            string userJson = JsonSerializer.Serialize(registrationResponseMessage.Users);
            File.WriteAllText("users.json", userJson);
        }
    }
}
