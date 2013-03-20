namespace Contract
{
    public class UrlUtility
    {
        public static string GetSiteUrlByPageUrl(string pageUrl)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
                return "";

            bool containsHttpWww = pageUrl.ToLower().StartsWith("http://www.");
            bool containsHttp = pageUrl.ToLower().StartsWith("http://");
            bool containsWww = pageUrl.ToLower().StartsWith("www.");

            string tmpUrl = pageUrl.ToLower().Replace("http://", "").Trim();
            if (tmpUrl.StartsWith("www."))
                tmpUrl = tmpUrl.Substring(4);

            if (tmpUrl.Contains("/"))
                tmpUrl = tmpUrl.Split('/')[0];

            if (containsHttpWww)
                tmpUrl = "http://www." + tmpUrl;
            else if (containsHttp)
                tmpUrl = "http://" + tmpUrl;
            else if (containsWww)
                tmpUrl = "www." + tmpUrl;

            return tmpUrl;
        }

        public static string StandartSiteUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                    return "";

            string tmpUrl = url.ToLower().Replace("http://", "").Trim();
            
            if (tmpUrl.Contains("/"))
                tmpUrl = tmpUrl.Split('/')[0];

            return tmpUrl;
        } 
    }
}