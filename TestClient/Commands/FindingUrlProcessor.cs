using System;
using System.Linq;
using System.Threading;
using Contract;
using Platform.StreamClients;
using Platform.StreamStorage;

namespace TestClient.Commands
{
    public class FindingUrlProcessor : ICommandProcessor
    {
        public string Key { get { return "F-URL"; } }
        public string Usage { get { return "F-URL keyword [T(itle)|U(rl)|H(tml)]"; } }

        public bool Execute(CommandProcessorContext context, CancellationToken token, string[] args)
        {
            var events = context.Client.EventStores.ReadAllEvents(new EventStoreOffset(0));

            foreach (RetrievedEventsWithMetaData eventData in events)
            {
                if (eventData.StreamId != "EngineSearchResult")
                    continue;

                var result = KeywordResult.TryGetFromBinary(eventData.EventData);

                if (!args[0].Equals(result.Keyword, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                foreach (EngineResult engineResult in result.EngineResults)
                {
                    if (args.Length == 1)
                        context.Log.Info("{0} - {1}", engineResult.Url, engineResult.Title);
                    else
                    {
                        string format = "";
                        if (args[1].IndexOf("U", StringComparison.CurrentCultureIgnoreCase) != -1)
                            format += "{0} ";
                        if (args[1].IndexOf("T", StringComparison.CurrentCultureIgnoreCase) != -1)
                            format += "{1}";
                        if (args[1].IndexOf("H", StringComparison.CurrentCultureIgnoreCase) != -1)
                            format += "\r\n{2}\r\n\r\n";

                        context.Log.Info(format, engineResult.Url, engineResult.Title, engineResult.ItemHtml);
                    }
                }
            }

            return false;
        }
    }
}