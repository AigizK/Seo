using System;
using System.Collections.Generic;
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
                    if (keywordResult.Engine != Engine.Google)
                        continue;

                    foreach (EngineResult engineResult in keywordResult.EngineResults)
                    {
                        var site = UrlUtility.StandartSiteUrl(engineResult.Url);
                        var indexedPages = new List<EngineResult>();

                        switch (keywordResult.Engine)
                        {
                            case Engine.Google:
                                indexedPages = new GoogleIndexedPages().GetPages(site);
                                break;
                            case Engine.None:
                            case Engine.Yandex:
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        foreach (EngineResult indexedPage in indexedPages)
                        {
                            store.WriteEvent("IndexedPage",
                                new IndexedPage
                                    {
                                        Date = DateTime.UtcNow,
                                        Engine = keywordResult.Engine,
                                        EngineResult = indexedPage
                                    }.ToBinary());
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
