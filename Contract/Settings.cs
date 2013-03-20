namespace Contract
{
    public static class Settings
    {
        public static string StorePath
        {
            get { return @"c:\lokaddata\itf-store"; }
        }

        public static string StoreConnection
        {
            get { return "http://localhost:16555"; }
        }

        public static string StoreId
        {
            get { return "seo"; }
        }
    }
}