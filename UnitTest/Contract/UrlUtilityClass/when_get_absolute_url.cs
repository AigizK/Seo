using Contract;
using NUnit.Framework;

namespace UnitTest.Contract.UrlUtilityClass
{
    /// <summary>
    /// todo http://www.ietf.org/rfc/rfc2396.txt
    /// </summary>
    public class when_get_absolute_url
    {
        [Test]
        public void url_with_http()
        {
            var url = "http://example.com/index.html";

            Assert.AreEqual(url, UrlUtility.GetAbsoluteUrlForResource(null, url));
        }

        [Test]
        public void url_with_https()
        {
            var url = "https://example.com/index.html";

            Assert.AreEqual(url, UrlUtility.GetAbsoluteUrlForResource(null, url));
        }

        [Test]
        public void url_with_ftp()
        {
            var url = "ftp://example.com/index.html";

            Assert.AreEqual(url, UrlUtility.GetAbsoluteUrlForResource(null, url));
        }

        [Test]
        public void root_relative_url()
        {
            var pageUrl = "http://example.com/section/index.html";
            var url = "/images/1.gif";

            Assert.AreEqual("http://example.com/images/1.gif", UrlUtility.GetAbsoluteUrlForResource(pageUrl, url));
        }

        [Test]
        public void current_folder_relative_url()
        {
            var pageUrl = "http://example.com/section/index.html";
            var url = "images/1.gif";

            Assert.AreEqual("http://example.com/section/images/1.gif", UrlUtility.GetAbsoluteUrlForResource(pageUrl, url));
        }

        [Test]
        public void relative_url_with_point()
        {
            var pageUrl = "http://example.com/section/index.html";
            var url = "./images/1.gif";

            Assert.AreEqual("http://example.com/section/images/1.gif", UrlUtility.GetAbsoluteUrlForResource(pageUrl, url));
        }

        [Test]
        public void relative_parent_folder_url()
        {
            var pageUrl = "http://example.com/section/index.html";
            var url = "../images/1.gif";

            Assert.AreEqual("http://example.com/images/1.gif", UrlUtility.GetAbsoluteUrlForResource(pageUrl, url));
        }

        [Test]
        public void relative_more_parent_folder_url()
        {
            var pageUrl = "http://example.com/section1/section2/index.html";
            var url = "../../images/1.gif";

            Assert.AreEqual("http://example.com/images/1.gif", UrlUtility.GetAbsoluteUrlForResource(pageUrl, url));
        }

        [Test]
        public void bad_relative_more_parent_folder_url()
        {
            var pageUrl = "http://example.com/section/index.html";
            var url = "../../../../images/1.gif";

            Assert.AreEqual("http://example.com/images/1.gif", UrlUtility.GetAbsoluteUrlForResource(pageUrl, url));
        }
    }
}