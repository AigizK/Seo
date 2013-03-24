using System;

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

        public static string GetAbsoluteUrlForResource(string pageUrl, string url)
        {
            if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
                    url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase) ||
                    url.StartsWith("ftp://", StringComparison.CurrentCultureIgnoreCase) ||
                    url.StartsWith("//", StringComparison.CurrentCultureIgnoreCase))
                return url;

            if (url.StartsWith("/"))
                return GetSiteUrlByPageUrl(pageUrl) + url;

            if (url.StartsWith("../"))
                return GetAbsoluteUrlForResource(GetUrlParentDirectory(GetUrlParentDirectory(pageUrl)), url.Substring(3));

            if (url.StartsWith("./"))
                return GetAbsoluteUrlForResource(pageUrl, url.Substring(2));

            return GetUrlParentDirectory(pageUrl) + url;
        }

        public static string GetUrlParentDirectory(string pageUrl)
        {
            var siteUrl = GetSiteUrlByPageUrl(pageUrl);
            string tmpPageUrl = pageUrl.Substring(siteUrl.Length);

            var length = tmpPageUrl.Length;
            if (tmpPageUrl.Contains("?"))
                length = tmpPageUrl.IndexOf('?');
            if (tmpPageUrl.Contains("#") && tmpPageUrl.IndexOf('#') < length)
                length = tmpPageUrl.IndexOf('#');

            var pagePath = tmpPageUrl.Substring(0, length).Trim('/');
            var segments = pagePath.Split('/');

            var pageDirectoryUrl = "";
            for (int i = 0; i < segments.Length - 1; i++)
            {
                pageDirectoryUrl += segments[i] + "/";
            }
            return siteUrl + "/" + pageDirectoryUrl.TrimStart('/');
        }
    }
}