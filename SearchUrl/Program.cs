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
        static void Main(string[] args)
        {
            Console.Write("Введите ключевое слово: ");
            var keyword = Console.ReadLine();

            new KeywordSerach().FindSitePositionByKeyword(keyword, Engine.Google);
        }
    }


}
