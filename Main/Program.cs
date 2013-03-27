using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Contract;
using Platform;
using Platform.StreamClients;
using SearchIndexedPages;
using SearchUrl;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var keywords = File.ReadAllLines("d:\\keywords.txt");

            var store = PlatformClient.ConnectToEventStore(Settings.StorePath, Settings.StoreId, Settings.StoreConnection);
            //var next = !store.ReadAllEvents().Any() ? new EventStoreOffset(0) : store.ReadAllEvents().Last().Next;
            var next = new EventStoreOffset(0);

            //Console.WriteLine("находим позиции сайтов");

            ////находим позиции сайтов
            //for (int i = 0; i < keywords.Length; i++)
            //{
            //    try
            //    {
            //        Console.WriteLine(keywords[i]);
            //        new KeywordSerach().FindSitePositionByKeyword(keywords[i], Engine.Google);
            //        Thread.Sleep(5 * 1000);
            //    }
            //    catch (InvalidDataException e)
            //    {
            //        MessageBox.Show("Google забанил");
            //        Thread.Sleep(30 * 1000);
            //        i--;
            //    }
            //}

            Console.WriteLine("находим просканированные страницы сайта");
            //находим проскнированные страницы сайтов из топ 100
            foreach (var eventsWithMetaData in store.ReadAllEvents(next))
            {
                if (eventsWithMetaData.StreamId != "EngineSearchResult")
                    continue;

                var keywordResult = KeywordResult.TryGetFromBinary(eventsWithMetaData.EventData);
                Console.WriteLine("Ищем страницы сайтов для слова " + keywordResult.Keyword);

                for (int i = 0; i < keywordResult.EngineResults.Count; i++)
                {
                    EngineResult engineResult = keywordResult.EngineResults[i];
                    Console.WriteLine("Слово: {0}\tСайт:{1}", keywordResult.Keyword, engineResult.Url);
                    new IndexedPages().FindSiteIndexedPages(engineResult.Url, keywordResult.Engine);
                }
            }
        }
    }
}
