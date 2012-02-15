namespace Netronics.Protocol.PacketEncoder.Http
{
    public class Response
    {
        private string _content;

        public void SetContent(string content)
        {
            _content = content;
        }

        public string GetContent()
        {
            return _content;
        }
    }
}