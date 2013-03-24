using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Ude;

namespace Contract
{
    public class HtmlUtility
    {
        public string GetHtmlByUrl(string url, Encoding encoding)
        {
            try
            {
                WebClient client = new WebClient();
                var data = client.DownloadData(url);
                return encoding.GetString(data);
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        /// <summary>
        /// по адресу страницы получает его HTML код в нормальной кодировке
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public string GetHtmlByUrl(string url, params Encoding[] defaultEncoding)
        {
            /*
             Для определения правильный кодировки используем библиотеку http://code.google.com/p/ude/  windows-1251
             */
            try
            {
                WebClient client = new WebClient();
                var data = client.DownloadData(url);

                Encoding enc = defaultEncoding[0];

                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    ICharsetDetector cdet = new CharsetDetector();

                    cdet.Feed(stream);
                    cdet.DataEnd();

                    if (cdet.Charset != null)
                    {
                        enc = Encoding.GetEncoding(cdet.Charset);
                    }
                }
                //
                var html = enc.GetString(data);
                if (html.Contains("���"))
                {
                    foreach (var encoding in defaultEncoding)
                    {
                        html = encoding.GetString(data);
                        if (!html.Contains("���"))
                            break;
                    }
                }

                return html;
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// по адресу страницы получает его HTML код в нормальной кодировке
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetHtmlByUrl(string url)
        {
            return GetHtmlByUrl(url, Encoding.UTF8, Encoding.GetEncoding(1251));
        }

        public string GetHtmlByStream(byte[] data, Stream stream)
        {
            /*
             Для определения правильный кодировки используем библиотеку http://code.google.com/p/ude/
             */
            try
            {
                return GetEncoding(data, stream).GetString(data);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public Encoding GetEncoding(byte[] data, Stream stream)
        {
            Encoding enc = Encoding.UTF8;

            stream.Seek(0, SeekOrigin.Begin);
            ICharsetDetector cdet = new CharsetDetector();

            cdet.Feed(stream);
            cdet.DataEnd();

            if (cdet.Charset != null)
            {
                enc = Encoding.GetEncoding(cdet.Charset);
            }

            return enc;
        }

        public string GetPostResult(string url, Dictionary<string, string> postParams)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            WebClient client = new WebClient();
            NameValueCollection postData = new NameValueCollection();
            foreach (KeyValuePair<string, string> pair in postParams)
            {
                postData.Add(pair.Key, pair.Value);
            }

            byte[] responseBytes = client.UploadValues(url, postData);
            return Encoding.ASCII.GetString(responseBytes);
        }
    }
}