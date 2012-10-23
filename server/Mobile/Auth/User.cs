using Netronics.DB;

namespace Netronics.Mobile.Auth
{
    public class User : Model
    {
        [CharField(256)] public string Key;
    }
}
