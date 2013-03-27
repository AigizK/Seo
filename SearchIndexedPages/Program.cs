using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using Platform;
using Platform.StreamClients;
using Platform.StreamStorage;
using SearchIndexedPages.Google;

namespace SearchIndexedPages
{
    class Program
    {
        static void Main(string[] args)
        {
            var store = PlatformClient.ConnectToEventStore(Settings.StorePath, Settings.StoreId, Settings.StoreConnection);
            var next = !store.ReadAllEvents().Any() ? new EventStoreOffset(0) : store.ReadAllEvents().Last().Next;

            while (true)
            {
                foreach (var eventsWithMetaData in store.ReadAllEvents(next))
                {
                    next = eventsWithMetaData.Next;
                    if (eventsWithMetaData.StreamId != "EngineSearchResult")
                        continue;

                    var keywordResult = KeywordResult.TryGetFromBinary(eventsWithMetaData.EventData);

                    for (int i = 0; i < keywordResult.EngineResults.Count; i++)
                    {
                        EngineResult engineResult = keywordResult.EngineResults[i];
                        new IndexedPages().FindSiteIndexedPages(engineResult.Url, keywordResult.Engine);
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
