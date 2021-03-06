﻿using ChatClient.MessageHandler;
using ChatProtocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;

namespace ChatClient
{
    class Program
    {
        static string serverIpAddress = "127.0.0.1";
        static int serverPort = 13000;

        static TcpClient client;
        public static List<User> users;
        public static string SessionId;
        public static bool IsConnected = false;
        public static bool IsConnecting = false;
        public static bool IsApplicationExecuting = true;
        public static bool isNewUser = false;

        static Thread receiveDataThread;

        static void Connect(string username, string password)
        {
            IsConnecting = true;
            client = new TcpClient(serverIpAddress, serverPort);

            ConnectMessage connectMessage = new ConnectMessage
            {
                ServerPassword = "test123",
                Username = username,
                Password = password
            };

            StartReceiveDataThread();
            SendMessage(JsonSerializer.Serialize(connectMessage));
        }
        static void Register(string username, string password)
        {
            IsConnecting = true;
            client = new TcpClient(serverIpAddress, serverPort);

            RegistrationMessage registrationMessage = new RegistrationMessage
            {
                ServerPassword = "test123",
                Username = username,
                Password = password
            };

            StartReceiveDataThread();
            SendMessage(JsonSerializer.Serialize(registrationMessage));
        }

        static void Disconnect()
        {
            DisconnectMessage disconnectMessage = new DisconnectMessage()
            {
                SessionId = SessionId
            };
            SendMessage(JsonSerializer.Serialize(disconnectMessage));

            StopReceiveDataThread();

            IsConnecting = false;
            IsConnected = false;
            SessionId = string.Empty;
            client.Close();
            client = null;
        }

        public static void StopReceiveDataThread()
        {
            receiveDataThread.Interrupt();
        }

        public static void StartReceiveDataThread()
        {
            ThreadStart threadStart = new ThreadStart(ReceiveData);
            receiveDataThread = new Thread(threadStart);
            receiveDataThread.Start();
        }

        static void SendMessage(string messageJson)
        {
            // Verschlüsselung: messageJson verschlüsseln
            byte[] data = System.Text.Encoding.UTF8.GetBytes(messageJson);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }

        static void ReceiveData()
        {
            while (client != null)
            {
                try
                {
                    lock (client)
                    {
                        byte[] data = new byte[1024];
                        int bytes = client.GetStream().Read(data, 0, data.Length);
                        string responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                        GenericMessage genericMessage = JsonSerializer.Deserialize<GenericMessage>(responseData);
                        IMessage message = MessageFactory.GetMessage(genericMessage.MessageId, responseData);
                        IMessageHandler messageHandler = MessageHandlerFactory.GetMessageHandler(genericMessage.MessageId);
                        messageHandler.Execute(client, message);
                    }
                }
                catch (System.IO.IOException)
                { }
                catch (System.ObjectDisposedException)
                { }
            }
        }

        static void SendChatMessage(string messageContent)
        {
            try
            {
                // Prepare chat message
                ChatMessage chatMessage = new ChatMessage
                {
                    Content = messageContent,
                    SessionId = SessionId
                };

                // Send message
                SendMessage(JsonSerializer.Serialize(chatMessage));
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        static void Main()
        {
            while (IsApplicationExecuting)
            {
                Console.Clear();

                Console.WriteLine("Are you a new user? For registration type in \"y\".");

                if (Console.ReadLine().Equals("y"))
                {
                    isNewUser = true;
                }

                Console.WriteLine("Username: ");
                string username = Console.ReadLine();

                Console.WriteLine("Password: ");
                string password = Console.ReadLine();

                if (!isNewUser)
                {
                    Console.WriteLine("Connecting to server.");
                    Connect(username, password);
                }
                else
                {
                    Console.WriteLine("Connecting to server.");
                    Register(username, password);
                }

                while (IsConnecting)
                {

                }

                while (IsConnected)
                {
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "/disconnect":
                            Disconnect();
                            break;
                        case "/users":
                            string usersOnServer = File.ReadAllText("users.json");
                            users = JsonSerializer.Deserialize<List<User>>(usersOnServer);
                            foreach (var user in users)
                            {
                                Console.WriteLine($"Name: {user.Username} \tID: {user.Id}");
                            }
                            break;
                        case "/exit":
                            Disconnect();
                            IsApplicationExecuting = false;
                            break;
                        default:
                            SendChatMessage(input);
                            break;
                    }
                }
            }
        }
    }
}
