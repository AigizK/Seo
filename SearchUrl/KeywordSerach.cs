using System;
using System.ComponentModel;
using Contract;
using Platform;
using SearchUrl.Google;

namespace SearchUrl
{
    public class KeywordSerach
    {
        public void FindSitePositionByKeyword(string keyword, Engine engine)
        {
            var store = PlatformClient.ConnectToEventStore(Settings.StorePath, Settings.StoreId, Settings.StoreConnection);

            IKeywordSearch keywordSearch = null;

            switch (engine)
            {
                case Engine.Google:
                    keywordSearch = new GoogleKeywordSearch();
                    break;
                case Engine.Yandex:
                    throw new InvalidEnumArgumentException("не реализован");
                case Engine.None:
                default:
                    throw new ArgumentOutOfRangeException("engine");
            }
            
            var result = keywordSearch.Search(keyword);

            store.WriteEvent("EngineSearchResult", result.ToBinary());
        }
    }
}