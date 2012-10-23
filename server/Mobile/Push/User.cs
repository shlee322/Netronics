using Netronics.DB;

namespace Netronics.Mobile.Push
{
    public class User : Model
    {
        [Int64Field] public long UserId;
        [CharField] public string Type;
        [CharField] public string Key;
    }
}
