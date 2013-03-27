using System.Collections.Generic;
using Contract;
using Contract.GoogleSearch;

namespace SearchIndexedPages.Google
{
    public class GoogleIndexedPages
    {
        public List<EngineResult> GetPages(string site)
        {
            var result = new List<EngineResult>();
            int pageIndex = 0;
            while (true)
            {
                var currentItems = new GoogleSearchUtility().GetEngineResults("site:" + site, pageIndex, 5);
                if (currentItems.Count == 0)
                    break;

                if (ResultContainsLastResult(result, currentItems))
                    break;

                result.AddRange(currentItems);
                pageIndex++;

                if (pageIndex > 10)
                    break;
            }

            return result;
        }

        private static bool ResultContainsLastResult(List<EngineResult> result, IList<EngineResult> currentItems)
        {
            if (result.Count >= currentItems.Count)
            {
                int i;
                for (i = 0; i < currentItems.Count; i++)
                {
                    if (result[result.Count - currentItems.Count + i].Url != currentItems[i].Url)
                        break;
                }

                if (i == currentItems.Count)
                    return true;
            }
            return false;
        }
    }
}