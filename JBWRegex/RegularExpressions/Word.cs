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
        public string candidatepos;
        public string subcat;
        public string prepunc="";
        public string postpunc="";
        public Word()
        {
        }
        public Word(Word w)
        {
            word = w.word;
            pos = w.pos;
            candidatepos = w.candidatepos;
            subcat = w.subcat;
            prepunc = w.prepunc;
            postpunc = w.postpunc;
        }
        public bool IsEqual (Word w)
        {
            if (w == null)
                return false;
            return word == w.word && pos == w.pos && subcat == w.subcat;
        }
        public bool NotEqual(Word w)
        {
            if (w == null)
                return true;
            return word != w.word || pos != w.pos || subcat != w.subcat;
        }
        public bool hasPos(string p)
        {
            return candidatepos.Contains(p);
        }
    }  
}
