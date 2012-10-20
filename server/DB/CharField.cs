namespace Netronics.DB
{
    public class CharField : Field
    {
        private readonly int _maxLength;
        public CharField(int maxLength=256)
        {
            _maxLength = maxLength;
        }

        public int GetMaxLength()
        {
            return _maxLength;
        }
    }
}
