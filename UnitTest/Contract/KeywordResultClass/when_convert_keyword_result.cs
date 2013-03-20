using System;
using System.Collections.Generic;
using Contract;
using NUnit.Framework;

namespace UnitTest.Contract.KeywordResultClass
{
    public class when_convert_keyword_result
    {
        [Test]
        public void empty_engine_results()
        {
            var keywordResult = new KeywordResult
            {
                Keyword = "keyword",
                Date = DateTime.UtcNow,
                Engine = Engine.None,
                Version = "1",
                EngineResults = new List<EngineResult>()
            };

            var bytes = keywordResult.ToBinary();

            AssertEx.PropertyValuesAreEquals(keywordResult, KeywordResult.TryGetFromBinary(bytes));
        }

        [Test]
        public void exist_engine_results()
        {
            var keywordResult = new KeywordResult
            {
                Keyword = "keyword",
                Date = DateTime.UtcNow,
                Engine = Engine.None,
                Version = "1",
                EngineResults = new List<EngineResult>{
                        new EngineResult{
                        Title="Title",
                        Url = "Url",
                        ItemHtml="<b>Html</b>"
                    }
                }
            };

            var bytes = keywordResult.ToBinary();

            var actual = KeywordResult.TryGetFromBinary(bytes);

            AssertEx.PropertyValuesAreEquals(keywordResult, actual);
        }
    }
}