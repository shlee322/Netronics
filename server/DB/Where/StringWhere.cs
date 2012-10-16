namespace Netronics.DB.Where
{
    class StringWhere : Where
    {
        private string _sql;
        public StringWhere(string sql)
        {
            _sql = sql;
        }
    }
}
