using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Contract.GoogleSearch
{
    public class GoogleSearchUtility
    {
        public IList<EngineResult> GetEngineResults(string keyword, int pageIndex = 0)
        {
            var html = GetHtml(keyword, pageIndex);

            if (!html.Contains("id=\"ires\""))
                throw new InvalidDataException("не найден контент с результатами");

            html = html.Substring(html.IndexOf("id=\"ires\""));

            var result = new List<EngineResult>();
            int position = 0;
            int startIndex;
            while ((startIndex = html.Substring(position).IndexOf("<li class=\"g\">")) >= 0)
            {
                const string endTag = "</li>";
                var endIndex = html.Substring(position + startIndex).IndexOf(endTag);
                if (endIndex == -1)
                    break;
                endIndex += endTag.Length;

                var engineResult = new EngineResult
                    {
                        ItemHtml = html.Substring(position + startIndex, endIndex)
                    };

                position += startIndex + endIndex;

                if (engineResult.ItemHtml.IndexOf("Картинки по запросу", StringComparison.CurrentCultureIgnoreCase) != -1 || //картинки по запросу
                    engineResult.ItemHtml.IndexOf("<cite class=\"kv\">") != -1 ||  //видео по запросу
                    engineResult.ItemHtml.IndexOf("float:right") != -1      //из гугл мапс объекты
                    )
                    continue;

                engineResult.Title = GetTitle(engineResult.ItemHtml);
                engineResult.Url = GetUrl(engineResult.ItemHtml);
                result.Add(engineResult);
            }

            return result;
        }

        private string GetUrl(string itemHtml)
        {
            int startPosition = itemHtml.IndexOf("<cite>");
            int endPosition = itemHtml.Substring(Math.Max(startPosition, 0)).IndexOf("</cite>");

            if (startPosition == -1 || endPosition == -1)
                throw new InvalidDataException("не можем найти адрес страницы");

            return Regex.Replace(itemHtml.Substring(startPosition, endPosition), @"<[^>]*>", String.Empty);
        }

        private string GetTitle(string itemHtml)
        {
            int startPosition = itemHtml.IndexOf("<a");
            int endPosition = itemHtml.Substring(Math.Max(startPosition, 0)).IndexOf("</a>");

            if (startPosition == -1 || endPosition == -1)
                throw new InvalidDataException("не можем найти заголовок страницы");

            return Regex.Replace(itemHtml.Substring(startPosition, endPosition), @"<[^>]*>", String.Empty);
        }

        static string GetHtml(string key, int pageIndex)
        {
            key = key.Trim();
            string responseData = "";
            try
            {
                string url = string.Format("https://www.google.ru/search?hl=ru&source=hp&biw=&bih=&q={0}&btnG=%D0%9F%D0%BE%D0%B8%D1%81%D0%BA+%D0%B2+Google&gbv=1&start={1}", key, pageIndex * 100);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Host = "www.google.ru";
                request.Referer = "https://www.google.ru/?hl=ru";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0";

                request.Timeout = 60000;
                request.Method = "GET";

                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Uri("http://www.google.ru"), new Cookie("PREF", "ID=9dbfa5884be76668:U=d92829bd9034e984:FF=0:LD=ru:NR=100:NW=1:TM=1362901947:LM=1362978808:GBV=1:SG=2:S=Gf4fp9hvTWVS4AKZ"));
                //request.CookieContainer.Add(new Uri("http://www.google.ru"), new Cookie("NID", "67=KEvpMo2lMnW8gjWe9nMHImKD5gXixGhtHnqBIa63i2Uu6PI5SQL7lXRFdL-pME3bjOY8RHM2X7klBWEVWX_NxWQpaJn28gZJUpSlDKGJ_Yikrur78ou5JcBtpLYfvaDt"));

                using (var hwresponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var responseStream = hwresponse.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var myStreamReader = new StreamReader(responseStream))
                                responseData = myStreamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                responseData = "An error occurred: " + e.Message;
            }
            return responseData;
        }
    }
}