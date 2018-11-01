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
            string vooPrd = "给|问|告诉|送|叫|教|找|祝|谓|借|帮|交|给予|称|是|付|递|说|留|赋予|赠|回|收|欠|答应|发|通知|寄|带|还|看|提醒|授予|敬|会|打|劝|骂|请教|讲|当|上|夸|建议|要求|谢谢|判|过|分|赔偿|包|塞|供给|起|算|买|去|考|传|给与|喂";
            Regex vpRe2 = new Regex(@"^(?<cc>c+)?(?<adv>[td]|(?:[vad]地?)|p[ntr][fu]?)*([#给|问|告诉][了着过]?)(?:(?<obj>)(?<att>[rm]m?q?|q|[antrv]f?的?)*([ntr])){2}(?<uv>u+)?$");

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
