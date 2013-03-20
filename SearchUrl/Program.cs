using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Platform;
using Platform.StreamClients;
using SearchUrl.Google;

namespace SearchUrl
{
    class Program
    {
        private static IRawEventStoreClient _store;

        static void Main(string[] args)
        {
            _store = PlatformClient.ConnectToEventStore(Settings.StorePath, Settings.StoreId, Settings.StoreConnection);

            Console.Write("Введите ключевое слово: ");
            var keyword = Console.ReadLine();

            IKeywordSearch keywordSearch = new GoogleKeywordSearch();
            var result = keywordSearch.Search(keyword);

            _store.WriteEvent("EngineSearchResult", result.ToBinary());
        }
    }


}
