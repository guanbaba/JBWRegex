using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBWRegex.RegularExpressions
{
    public class Word
    {
        public string word;
        public string pos;
        public static bool operator == (Word w1, Word w2)
        {
            return w1.word == w2.word && w1.pos == w2.pos;
        }
        public static bool operator !=(Word w1, Word w2)
        {
            return w1.word != w2.word || w1.pos != w2.pos;
        }
    }
    
}
