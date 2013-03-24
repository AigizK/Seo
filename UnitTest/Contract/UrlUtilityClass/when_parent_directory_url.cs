using Contract;
using NUnit.Framework;

namespace UnitTest.Contract.UrlUtilityClass
{
    public class when_parent_directory_url
    {
        [Test]
        public void site_url_without_slach()
        {
            string url = "http://example.com";

            Assert.AreEqual(url+"/",UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void site_url_with_slach()
        {
            string url = "http://example.com/";

            Assert.AreEqual(url , UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void root_file_url()
        {
            string url = "http://example.com/index.html";

            Assert.AreEqual("http://example.com/", UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void child_page_url()
        {
            string url = "http://example.com/directory/index.html";

            Assert.AreEqual("http://example.com/directory/", UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void directory_url_witout_slach()
        {
            string url = "http://example.com/directory";

            Assert.AreEqual("http://example.com/", UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void directory_url_wit_slach()
        {
            string url = "http://example.com/directory/";

            Assert.AreEqual("http://example.com/", UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void url_with_query()
        {
            string url = "http://example.com/directory/index.html?x=1/2";

            Assert.AreEqual("http://example.com/directory/", UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void url_with_ajax_query()
        {
            string url = "http://example.com/directory/index.html#x=1/2";

            Assert.AreEqual("http://example.com/directory/", UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void directory_url_with_query()
        {
            string url = "http://example.com/directory/index.html?x=1/2";

            Assert.AreEqual("http://example.com/directory/", UrlUtility.GetUrlParentDirectory(url));
        }

        [Test]
        public void directory_url_with_ajax_query()
        {
            string url = "http://example.com/directory/index.html#x=1/2";

            Assert.AreEqual("http://example.com/directory/", UrlUtility.GetUrlParentDirectory(url));
        }
    }
}