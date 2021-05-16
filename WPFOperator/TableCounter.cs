using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFOperator
{
    class TableCounter
    {
        public string[] Keys { get; set; }
        public int[] ArrayCount { get; set; }

        public TableCounter(string[] dict)
        {
            Keys = new string[dict.Length];
            ArrayCount = new int[dict.Length];
            for (int i = 0; i < dict.Length; i++)
            {
                Keys[i] = dict[i];
            }
        }

        public int FindKeyIndex(string k)
        {
            for (int i = 1; i < Keys.Length; i++)
            {
                if (Keys[i] == k) return i;
            }
            return -1;
        }
    }
}
