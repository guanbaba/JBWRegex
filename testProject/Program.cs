using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBWRegex.RegularExpressions;

namespace testProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Word> wordList = new List<Word>();
            wordList.Add(new Word() { word = "这", pos = "e",candidatepos="erabc"});
            wordList.Add(new Word() { word = "本", pos = "q", candidatepos = "q" });
            wordList.Add(new Word() { word = "有趣", pos = "a", candidatepos = "a" });
            wordList.Add(new Word() { word = "有意思", pos = "a", candidatepos = "a" });
            wordList.Add(new Word() { word = "的", pos = "u", candidatepos = "u" });
            wordList.Add(new Word() { word = "书", pos = "n", candidatepos = "n" });
            Regex re = new Regex(@"^(?:([rq])+([#有趣|有意思|好看])+(的)([nrt]))$");
            Match match = re.Match(wordList);
            Console.WriteLine(match.Success);
            if (match.Success)
            {
                Console.WriteLine(match.WordList.Count);
                Console.WriteLine(match.WordSeq);
                Console.WriteLine(match.PosSeq);
                Console.WriteLine(match.Index);
                foreach (Group group in match.Groups)
                {
                    Console.WriteLine(group.Name + ":");
                    foreach (Capture cap in group.Captures)
                    {
                        Console.WriteLine("索引：" + cap.Index + " " + "长度：" + cap.Length + " " + "词序列：" + cap.WordSeq);
                    }
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }
    }
}
