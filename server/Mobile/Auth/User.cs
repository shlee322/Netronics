using Netronics.DB;

namespace Netronics.Mobile.Auth
{
    class User : Model
    {
        [CharField(256)] public string Key;
    }
}
