using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contract;

namespace DownloadPageResource
{
    class Program
    {
        static void Main(string[] args)
        {
            string pageUrl = "http://www.profdekor.ru/";
            string savePath = @"D:\tmp\profdecor";
            string html = GetPageHtml(pageUrl);


            html = DownloadImageResources(html, pageUrl, savePath);
            html = DownloadJsResources(html, pageUrl, savePath);
            html = DownloadCssResources(html, pageUrl, savePath);

            SaveHtml(html, savePath);
        }

        private static string DownloadCssResources(string html, string pageUrl, string savePath)
        {
            var m = Regex.Match(html, "<link.+?href=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
            Dictionary<string, string> replacedUrls = new Dictionary<string, string>();
            while ((m = m.NextMatch()).Success)
            {
                var src = m.Groups[1].Value;

                if (src.IndexOf(".css", StringComparison.CurrentCultureIgnoreCase) == -1)
                    continue;

                string imageUrl = UrlUtility.GetAbsoluteUrlForResource(pageUrl, src);
                replacedUrls[src] = imageUrl;
            }

            var imageDirectory = Path.Combine(savePath, "css");
            if (!Directory.Exists(imageDirectory))
                Directory.CreateDirectory(imageDirectory);

            foreach (KeyValuePair<string, string> replacedUrl in replacedUrls)
            {
                string filename = Path.GetFileName(new Uri(replacedUrl.Value).LocalPath);
                int index = 0;
                while (true)
                {
                    var filePath = Path.Combine(imageDirectory, filename);
                    if (!File.Exists(filePath))
                    {
                        html = html.Replace(replacedUrl.Key, "file://" + Path.Combine(imageDirectory, filename));
                        break;
                    }

                    index++;
                    filename = index + "-" + filename;
                }

                try
                {
                    var client = new WebClient();
                    client.DownloadFile(replacedUrl.Value, Path.Combine(imageDirectory, filename));
                }
                catch (Exception)
                { }
            }

            return html;
        }

        private static string DownloadJsResources(string html, string pageUrl, string savePath)
        {
            var m = Regex.Match(html, "<script.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase| RegexOptions.Multiline);
            Dictionary<string, string> replacedUrls = new Dictionary<string, string>();
            while ((m = m.NextMatch()).Success)
            {
                var src = m.Groups[1].Value;

                string imageUrl = UrlUtility.GetAbsoluteUrlForResource(pageUrl, src);
                replacedUrls[src] = imageUrl;
            }

            var imageDirectory = Path.Combine(savePath, "js");
            if (!Directory.Exists(imageDirectory))
                Directory.CreateDirectory(imageDirectory);

            foreach (KeyValuePair<string, string> replacedUrl in replacedUrls)
            {
                string filename = Path.GetFileName(new Uri(replacedUrl.Value).LocalPath);
                int index = 0;
                while (true)
                {
                    var filePath = Path.Combine(imageDirectory, filename);
                    if (!File.Exists(filePath))
                    {
                        html = html.Replace(replacedUrl.Key, "file://" + Path.Combine(imageDirectory, filename));
                        break;
                    }

                    index++;
                    filename = index + "-" + filename;
                }

                try
                {
                    var client = new WebClient();
                    client.DownloadFile(replacedUrl.Value, Path.Combine(imageDirectory, filename));
                }
                catch (Exception)
                { }
            }

            return html;
        }

        private static void SaveHtml(string html, string savePath)
        {
            using (var sw = new StreamWriter(Path.Combine(savePath, "index.html")))
            {
                sw.Write(html);
            }
        }

        private static string DownloadImageResources(string html, string pageUrl, string savePath)
        {
            var m = Regex.Match(html, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
            Dictionary<string, string> replacedUrls = new Dictionary<string, string>();
            while ((m = m.NextMatch()).Success)
            {
                var src = m.Groups[1].Value;

                string imageUrl = UrlUtility.GetAbsoluteUrlForResource(pageUrl, src);
                replacedUrls[src] = imageUrl;
            }

            var imageDirectory = Path.Combine(savePath, "images");
            if (!Directory.Exists(imageDirectory))
                Directory.CreateDirectory(imageDirectory);

            foreach (KeyValuePair<string, string> replacedUrl in replacedUrls)
            {
                string filename = Path.GetFileName(new Uri(replacedUrl.Value).LocalPath);
                int index = 0;
                while (true)
                {
                    var filePath = Path.Combine(imageDirectory, filename);
                    if (!File.Exists(filePath))
                    {
                        html = html.Replace(replacedUrl.Key, "file://" + Path.Combine(imageDirectory, filename));
                        break;
                    }

                    index++;
                    filename = index + "-" + filename;
                }

                try
                {
                    var client = new WebClient();
                    client.DownloadFile(replacedUrl.Value, Path.Combine(imageDirectory, filename));
                }
                catch (Exception)
                { }
            }


            return html;
        }



        private static string GetPageHtml(string url)
        {
            return new HtmlUtility().GetHtmlByUrl(url, Encoding.UTF8, Encoding.GetEncoding(1251));
        }
    }
}
