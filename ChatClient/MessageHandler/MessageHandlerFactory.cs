namespace ChatClient.MessageHandler
{
    public static class MessageHandlerFactory
    {
        public static IMessageHandler GetMessageHandler(int messageId)
        {
            switch (messageId)
            {
                case 1:
                    return new ChatMessageHandler();
                case 3:
                    return new DisconnectMessageHandler();
                case 4:
                    return new ConnectResponseMessageHandler();
            }

            return null;
        }
    }
}
