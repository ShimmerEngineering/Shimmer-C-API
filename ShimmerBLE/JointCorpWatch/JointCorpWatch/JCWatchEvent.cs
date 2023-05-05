using System;
using System.Collections.Generic;
using System.Text;

namespace JointCorpWatch
{
    public  class JCWatchEvent
    {
        public int Identifier;
        public string Message;
        public bool End;
        public string DataType;
        public Dictionary<string, string> Data;
    }
}
