using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform;
using Platform.StreamClients;
using SearchUrl.Common;
using SearchUrl.Google;

namespace SearchUrl
{
    class Program
    {
        private static IRawEventStoreClient _store;

        static void Main(string[] args)
        {
            _store = PlatformClient.ConnectToEventStore(StorePath, "seo", StoreConnection);

            Console.Write("Введите ключевое слово: ");
            var keyword = Console.ReadLine();

            IKeywordSearch keywordSearch = new GoogleKeywordSearch();
            var result = keywordSearch.Search(keyword);

            _store.WriteEvent("EngineSearchResult",result.ToBinary());
        }

        public static string StorePath
        {
            get { return @"c:\lokaddata\itf-store"; }
        }

        public static string StoreConnection
        {
            get { return "http://localhost:16555"; }
        }
    }


}
