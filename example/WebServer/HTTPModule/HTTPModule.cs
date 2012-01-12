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
            _frontEnd.SetProcessor(() => { return new HTTPHandler(); });
            _frontEnd.Start(8080);
        }
    }
}
