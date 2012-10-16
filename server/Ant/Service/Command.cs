using System;

namespace Service.Service
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Command : Attribute
    {
    }
}
