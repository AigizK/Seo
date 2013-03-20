using System;
using System.Collections.Generic;
using System.IO;

namespace Contract
{
    public enum Engine
    {
        None=-1,
        Google = 0,
        Yandex = 1
    }

    public class KeywordResult
    {
        public string Keyword { get; set; }
        public DateTime Date { get; set; }
        public Engine Engine { get; set; }
        public string Version { get; set; }
        public IList<EngineResult> EngineResults { get; set; }

        public byte[] ToBinary()
        {
            using (var mem = new MemoryStream())
            using (var bin = new BinaryWriter(mem))
            {
                bin.Write(Keyword);
                bin.Write(Date.ToBinary());
                bin.Write((int)Engine);
                bin.Write(Version);
                bin.Write(EngineResults.Count);
                foreach (var engineResult in EngineResults)
                {
                    bin.Write(engineResult.ToBinary());
                }
                return mem.ToArray();
            }
        }

        public static KeywordResult TryGetFromBinary(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            using (var bin = new BinaryReader(mem))
            {
                var result = new KeywordResult
                {
                    Keyword = bin.ReadString(),
                    Date = DateTime.FromBinary(bin.ReadInt64()),
                    Engine = (Engine)bin.ReadInt32(),
                    Version = bin.ReadString(),
                    EngineResults = new List<EngineResult>()
                };

                var count = bin.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    EngineResult engineResult = new EngineResult
                        {
                            Title = bin.ReadString(),
                            Url = bin.ReadString(),
                            ItemHtml = bin.ReadString()
                        };
                    result.EngineResults.Add(engineResult);
                }

                return result;
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

        public static EngineResult TryGetFromBinary(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            using (var bin = new BinaryReader(mem))
            {
                return new EngineResult
                {
                    Title = bin.ReadString(),
                    Url = bin.ReadString(),
                    ItemHtml = bin.ReadString()
                };
            }
        }
    }

    public class IndexedPage
    {
        public EngineResult EngineResult { get; set; }
        public Engine Engine { get; set; }
        public DateTime Date { get; set; }

        public byte[] ToBinary()
        {
            using (var mem = new MemoryStream())
            using (var bin = new BinaryWriter(mem))
            {
                bin.Write(EngineResult.ToBinary());
                bin.Write((int)Engine);
                bin.Write(Date.ToBinary());
                return mem.ToArray();
            }
        }

        public static IndexedPage TryGetFromBinary(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            using (var bin = new BinaryReader(mem))
            {
                return new IndexedPage
                {
                    EngineResult = new EngineResult
                    {
                        Title = bin.ReadString(),
                        Url = bin.ReadString(),
                        ItemHtml = bin.ReadString()
                    },
                    Engine = (Engine)bin.ReadInt32(),
                    Date = DateTime.FromBinary(bin.ReadInt64())
                };
            }
        }
    }
}