﻿using System;

namespace SimpleBus
{
    public interface IMessageHandler
    {

    }

    public interface IMessageHandler<T> : IMessageHandler where T : IMessage
    {
        void Handle(IReceivedMessage<T> message);
    }
}
