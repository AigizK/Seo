using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Contract;
using Platform;
using Platform.StreamClients;
using SearchIndexedPages.Google;

namespace SearchIndexedPages
{
    public class IndexedPages
    {
        private IRawEventStoreClient _store;

        public IndexedPages()
        {
            _store = PlatformClient.ConnectToEventStore(Settings.StorePath, Settings.StoreId, Settings.StoreConnection);
        }

        public void FindSiteIndexedPages(string url, Engine engine)
        {
            var site = UrlUtility.StandartSiteUrl(url);

            List<EngineResult> indexedPages;

            while (true)
            {
                try
                {
                    switch (engine)
                    {
                        case Engine.Google:
                            indexedPages = new GoogleIndexedPages().GetPages(site);
                            break;
                        case Engine.Yandex:
                            throw new InvalidEnumArgumentException("не реализован");
                        case Engine.None:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                catch (InvalidDataException e)
                {
                    MessageBox.Show(engine + " забанил");
                    Thread.Sleep(30 * 1000);
                }
            }
            

            foreach (EngineResult indexedPage in indexedPages)
            {
                _store.WriteEvent("IndexedPage",
                                 new IndexedPage
                                 {
                                     Date = DateTime.UtcNow,
                                     Engine = engine,
                                     EngineResult = indexedPage
                                 }.ToBinary());
            }
        }
    }
}