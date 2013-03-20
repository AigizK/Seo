using System;
using System.Collections.Generic;
using System.IO;

namespace Contract
{
    public class KeywordResult
    {
        public string Keyword { get; set; }
        public DateTime Date { get; set; }
        public string Engine { get; set; }
        public string Version { get; set; }
        public IList<EngineResult> EngineResults { get; set; }

        public byte[] ToBinary()
        {
            using (var mem = new MemoryStream())
            using (var bin = new BinaryWriter(mem))
            {
                bin.Write(Keyword);
                bin.Write(Date.ToBinary());
                bin.Write(Engine);
                bin.Write(Version);
                bin.Write(EngineResults.Count);
                foreach (var engineResult in EngineResults)
                {
                    bin.Write(engineResult.ToBinary());
                }
                return mem.ToArray();
            }
        }
    }

    public class EngineResult
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ItemHtml { get; set; }

        public byte[] ToBinary()
        {
            using (var mem = new MemoryStream())
            using (var bin = new BinaryWriter(mem))
            {
                bin.Write(Title);
                bin.Write(Url);
                bin.Write(ItemHtml);
                return mem.ToArray();
            }
        }
    }
}