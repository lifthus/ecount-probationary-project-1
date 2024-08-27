using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy2408
{
    public class Asmt1_1
    {
        public static void Raffle()
        {
            var n = ReadIntFromConsole();
            var rafDict = ReadRafflesFromConsole(n);
            var validRafList = ExtractSortedValidRaffleList(rafDict);
            Console.WriteLine(String.Join(" ", validRafList));
        }

        static int ReadIntFromConsole()
        {
            return Int32.Parse(Console.ReadLine());
        }

        static Dictionary<int, int> ReadRafflesFromConsole(int n)
        {
            Dictionary<int, int> rafDict = new Dictionary<int, int>();
            for (int i = 0; i < n; i++)
            {
                ReadRaffleFromConsoleAndSetTo(rafDict);
            }
            return rafDict;
        }

        static void ReadRaffleFromConsoleAndSetTo(Dictionary<int, int> rafDict)
        {
            var rf = ReadIntFromConsole();
            if (!rafDict.ContainsKey(rf))
            {
                rafDict.Add(rf, 1);
            } else
            {
                rafDict[rf]++;
            }
        }

        static List<int> ExtractSortedValidRaffleList(Dictionary<int, int> rafDict)
        {
            var validRaf = rafDict.Keys
                .Where((n) => rafDict[n] == 1)
                .ToList();
            validRaf.Sort((a, b) => a < b ? 0 : 1);
            return validRaf;
        }
    }
}
