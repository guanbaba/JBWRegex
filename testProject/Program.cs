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
            wordList.Add(new Word() { word = "这", pos = "r" });
            wordList.Add(new Word() { word = "本", pos = "q" });
            wordList.Add(new Word() { word = "有趣", pos = "a" });
            wordList.Add(new Word() { word = "有意思", pos = "a" });
            wordList.Add(new Word() { word = "的", pos = "u" });
            wordList.Add(new Word() { word = "书", pos = "n" });
            Regex re = new Regex(@"^(?:rq([#有趣|有意思])+的n)$");
            Match match = re.Match(wordList);
            Console.WriteLine(match.Success);
            if (match.Success)
            {
                Console.WriteLine(match.Words);
                Console.WriteLine(match.Poss);
                Console.WriteLine(match.Index);
                foreach (Group group in match.Groups)
                {
                    Console.WriteLine(group.Name+":");
                    foreach (Capture cap in group.Captures)
                    {
                        Console.WriteLine("索引："+cap.Index+" "+"长度："+cap.Length+" "+"词序列："+cap.Words);
                    }
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }
    }
}
