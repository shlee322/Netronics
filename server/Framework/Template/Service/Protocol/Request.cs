﻿namespace Netronics.Template.Service.Protocol
{
    public class Request
    {
        public bool Result;
        public int Sender;
        public int Receiver;
        public ulong Transaction;
        public object Message;
    }
}
