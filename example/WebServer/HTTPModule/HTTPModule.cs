using ProxyService;

namespace HTTPModule
{
    public class HTTPModule
    {
        private static FrontEnd _frontEnd;
        public static void Load()
        {
            _frontEnd = new FrontEnd();
            _frontEnd.SetHandshake(() => { return null; });
            _frontEnd.Start(80);
        }
    }
}
