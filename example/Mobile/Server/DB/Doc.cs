using System;
using Netronics.DB;

namespace MobileServer.DB
{
    public class Doc : Model
    {
        [CharField]
        public string Title;
        [CharField(512)]
        public string Content;
        [DateTimeField]
        public DateTime Date;
    }
}
