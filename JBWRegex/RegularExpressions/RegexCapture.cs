//------------------------------------------------------------------------------
// <copyright file="RegexCapture.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

// Capture is just a location/length pair that indicates the
// location of a regular expression match. A single regexp
// search may return multiple Capture within each capturing
// RegexGroup.

using System;
using System.Collections.Generic;
using System.Linq;
namespace JBWRegex.RegularExpressions
{

    /// <devdoc>
    ///    <para> 
    ///       Represents the results from a single subexpression capture. The object represents
    ///       one substring for a single successful capture.</para>
    /// </devdoc>
#if !SILVERLIGHT
    [ Serializable() ] 
#endif
    public class Capture {
        internal String _text;
        internal List<Word> _wordlist;
        internal int _index;
        internal int _length;

        internal Capture(String text, int i, int l) {
            _text = text;
            _index = i;
            _length = l;
        }

        internal Capture(List<Word> wordlist, int i, int l)
        {
            _wordlist = wordlist;
            _index = i;
            _length = l;
        }

        /*
         * The index of the beginning of the matched capture
         */
        /// <devdoc>
        ///    <para>Returns the position in the original string where the first character of
        ///       captured substring was found.</para>
        /// </devdoc>
        public int Index {
            get {
                return _index;
            }
        }

        /*
         * The length of the matched capture
         */
        /// <devdoc>
        ///    <para>
        ///       Returns the length of the captured substring.
        ///    </para>
        /// </devdoc>
        public int Length {
            get {
                return _length;
            }
        }

        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public string Value {
            get {
                return _text.Substring(_index, _length);
            }
        }

        public List<Word> WordList
        {
            get
            {
                return _wordlist.GetRange(_index, _length);
            }
        }

        public string WordSeq
        {
            get
            {
                return String.Join("", _wordlist.GetRange(_index, _length).Select(p => p.word));
            }
        }

        public string WordSeqWithPunc
        {
            get
            {
                string result = "";
                foreach (var w in _wordlist.GetRange(_index, _length))
                {
                    result += w.prepunc + w.word + w.postpunc;
                }
                return result;
            }
        }

        public string PosSeq
        {
            get
            {
                return String.Join("", _wordlist.GetRange(_index, _length).Select(p => p.pos));
            }
        }

        /*
         * The capture as a string
         */
        /// <devdoc>
        ///    <para>
        ///       Returns 
        ///          the substring that was matched.
        ///       </para>
        ///    </devdoc>
        override public String ToString() {
            return Value;
        }

        /*
         * The original string
         */
        internal String GetOriginalString() {
            return _text;
        }

        /*
         * The substring to the left of the capture
         */
        internal String GetLeftSubstring() {
            return _text.Substring(0, _index);
        }

        /*
         * The substring to the right of the capture
         */
        internal String GetRightSubstring() {
            return _text.Substring(_index + _length, _text.Length - _index - _length);
        }

#if DBG
        internal virtual String Description() {
            StringBuilder Sb = new StringBuilder();

            Sb.Append("(I = ");
            Sb.Append(_index);
            Sb.Append(", L = ");
            Sb.Append(_length);
            Sb.Append("): ");
            Sb.Append(_text, _index, _length);

            return Sb.ToString();
        }
#endif
    }



}
