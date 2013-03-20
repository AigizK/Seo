using System;
using Contract;
using NUnit.Framework;

namespace UnitTest.Contract.KeywordResultClass
{
    public class when_convert_engine_result
    {
        [Test]
        public void success_convert()
        {
            var engineResult = new EngineResult
                {
                    Title="Title",
                    Url = "Url",
                    ItemHtml="<b>Html</b>"
                };

            var bytes = engineResult.ToBinary();

            AssertEx.PropertyValuesAreEquals(engineResult,EngineResult.TryGetFromBinary(bytes));
        }
    }
}