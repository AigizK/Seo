using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Contract;
using Contract.GoogleSearch;

namespace SearchUrl.Google
{
    public class GoogleKeywordSearch : IKeywordSearch
    {
        public KeywordResult Search(string keyword)
        {
            return new KeywordResult
                {
                    Date = DateTime.UtcNow,
                    Engine =Engine.Google,
                    Version = "0.0.1",
                    Keyword = keyword,
                    EngineResults =new GoogleSearchUtility().GetEngineResults(keyword,0, 10)
                };
        }
    }
}