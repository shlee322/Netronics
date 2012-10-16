using System;
using Netronics.DB;

namespace MobileServer.DB
{
    public class Comment : Model
    {
        [CharField(512)] public string Content;
        [DateTimeField] public DateTime Date;
        [ForeignKey] public Doc Doc;
    }
}
