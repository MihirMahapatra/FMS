namespace FMS.Utility
{
    public sealed class Mapping
    {
        private Mapping() { }
        private static Mapping _instance;
        private static readonly object _lock = new object();
        public static Mapping GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Mapping();
                    }
                }
            }
            return _instance;
        }
        public Dictionary<string, string> GetSalesType()
        {
            var salesType = new Dictionary<string, string>
            {
                { "cash", "Cash" },
                { "credit", "Credit" },
            };
            return salesType;
        }
    }
}
