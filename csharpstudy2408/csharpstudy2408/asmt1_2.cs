using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy2408
{
    internal class Asmt1_2
    {
        public static void PaperForm()
        {
            var paper = "Deeper neural networks are more difficult to train. We present a residual learning framework to ease the training of networks that are substantially deeper than those used previously.[ some_paper_a, some_paper_b ] We explicitly reformulate the layers as learning residual functions with reference to the layer inputs, instead of learning unreferenced functions.[ some_book_a, some_paper_a ] We provide comprehensive empirical evidence showing that these residual networks are easier to optimize, and can gain accuracy from considerably increased depth. [ some_book_b ]";
            var correctedPaper = CorrectPaperRefNum(paper);
            Console.WriteLine(correctedPaper);
        }

        static List<string> paperParts; 
        static List<int> refIndexes;
        static Dictionary<string, int> refNumDict;

        static string CorrectPaperRefNum(string paper)
        {
            paperParts = new List<string>();
            refIndexes = new List<int>();
            refNumDict = new Dictionary<string, int>();

            ParsePaper(paper);
            ParseRefNumDict();

            return $"{CorrectPaper()}\n{GetRefNumListStr()}";
        }

        static void ParsePaper(string paper)
        {
            var currentPart = new StringBuilder();
            foreach (var c in paper)
            {
                if (c == '[')
                {
                    paperParts.Add(currentPart.ToString());
                    currentPart.Clear();
                }
                else if (c == ']')
                {
                    refIndexes.Add(paperParts.Count());
                    paperParts.Add(currentPart.ToString());
                    currentPart.Clear();
                }
                else
                {
                    currentPart.Append(c);
                }
            }
            if (currentPart.Length > 0)
            {
                paperParts.Add(currentPart.ToString());
            }
        }

        static void ParseRefNumDict()
        {
            foreach (var idx in refIndexes)
            {
                var names = paperParts[idx].Split(',').Select((n) => n.Trim());
                foreach (var name in names)
                {
                    if (refNumDict.ContainsKey(name))
                    {
                        continue;
                    }
                    refNumDict.Add(name, refNumDict.Count() + 1);
                }
            }
        }

        static string CorrectPaper()
        {
            foreach (var idx in refIndexes)
            {
                var nameNums = paperParts[idx].Split(',')
                    .Select(n => n.Trim())
                    .Select(n => refNumDict[n].ToString());
                paperParts[idx] = $"[ {String.Join(", ", nameNums)} ]";
            }
            return String.Join("", paperParts);
        }

        static string GetRefNumListStr()
        {
            var refNumList = refNumDict.ToList();
            refNumList.Sort((a, b) => a.Value > b.Value ? 1 : 0);
            var result = new StringBuilder();
            for (int i = 0; i < refNumList.Count(); i++)
            {
                var refNum = refNumList[i];
                result.Append("[" + refNum.Value.ToString() + "] " + refNum.Key + "\n");
            }
            return result.ToString();
        }
    }
}
