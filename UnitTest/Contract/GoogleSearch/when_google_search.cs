using System;
using System.Collections.Generic;
using Contract;
using Contract.GoogleSearch;
using NUnit.Framework;

namespace UnitTest.Contract.GoogleSearch
{
    public class when_google_search
    {
        [Test]
        public void validate_keyword()
        {
            var search = new GoogleSearchUtility().GetEngineResults("жалюзи");

            Assert.IsTrue(search.Count > 80);
            Assert.IsTrue(search.Count <= 100);
        }

        [Test]
        public void bad_keyword()
        {
            var search = new GoogleSearchUtility().GetEngineResults("---------------------------------");

            Assert.AreEqual(0, search.Count);
        }

        [Test]
        public void next_page()
        {
            var search = new GoogleSearchUtility().GetEngineResults("жалюзи", 1);

            Assert.IsTrue(search.Count > 80);
            Assert.IsTrue(search.Count <= 100);
        }

        [Test]
        public void page_index_default_value()
        {
            var pageDefault = new GoogleSearchUtility().GetEngineResults("жалюзи");
            var page0 = new GoogleSearchUtility().GetEngineResults("жалюзи", 0);

            Assert.AreEqual(pageDefault.Count, page0.Count);
            for (int i = 0; i < pageDefault.Count; i++)
                Assert.AreEqual(pageDefault[i].Url, page0[i].Url);
        }

        [Test]
        public void compare_page_zero_and_page_one()
        {
            var page0 = new GoogleSearchUtility().GetEngineResults("жалюзи");
            var page1 = new GoogleSearchUtility().GetEngineResults("жалюзи", 1);

            var min = Math.Min(page0.Count, page1.Count);
            var equalCount = 0;

            for (int i = 0; i < min; i++)
            {
                if (page0[0].Url.Equals(page1[i].Url))
                    equalCount++;
            }

            Assert.IsTrue(equalCount * 100.0 / min < 3);
        }

        [Test]
        public void big_page_index()
        {
            var search = new GoogleSearchUtility().GetEngineResults("жалюзи", 1000);

            Assert.IsTrue(search.Count == 0);
        }
    }
}