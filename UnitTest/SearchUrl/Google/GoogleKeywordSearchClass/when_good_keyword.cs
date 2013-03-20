using System;
using Contract;
using NUnit.Framework;
using SearchUrl.Google;

namespace UnitTest.SearchUrl.Google.GoogleKeywordSearchClass
{

    public class when_good_keyword
    {
        [Test]
        public void get_keyword()
        {
            var result = new GoogleKeywordSearch().Search("жалюзи");

            Assert.AreEqual("жалюзи", result.Keyword);
        }

        [Test]
        public void get_engine()
        {
            var result = new GoogleKeywordSearch().Search("жалюзи");

            Assert.AreEqual(Engine.Google, (Engine)result.Engine);
        }

        [Test]
        public void get_date()
        {
            DateTime dt = DateTime.UtcNow;
            var result = new GoogleKeywordSearch().Search("жалюзи");

            Assert.IsTrue((dt - result.Date).TotalSeconds < 5);
        }

        [Test]
        public void get_version()
        {
            var result = new GoogleKeywordSearch().Search("жалюзи");

            Assert.AreEqual("0.0.1", result.Version);
        }

        [Test]
        public void get_items_any()
        {
            var result = new GoogleKeywordSearch().Search("жалюзи горизонтальные");

            Assert.IsTrue(result.EngineResults.Count > 80);
            Assert.IsTrue(result.EngineResults.Count <= 100);
        }
    }
}