using System;
using System.Collections.Generic;

namespace SearchUrl.Common
{
    public interface IKeywordSearch
    {
        KeywordResult Search(string keyword);
    }
}