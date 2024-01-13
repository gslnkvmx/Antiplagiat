using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiplagiatLib
{
    public class HashArray : HashFunction
    {
        public List<int> h = new();
        public string S {  get; set; }

        public HashArray(string s)
        {
            h.Add(0);
            for (int i = 1; i <= s.Length; ++i)
            {
                h.Add(Add(Mul(h[i - 1], P), (int)s[i - 1] - 'a' + 1));
                // Console.WriteLine($"{mul(h[i - 1], P)} + {(int)s[i - 1] - 'a' + 1}");
                // Console.WriteLine($"h{i} = {h[i]}");
            }
            this.S = s;
        }

        public int this[int index]
        {
            get => h[index];
            set => h[index] = value;
        }

        public int Length() { return h.Count; }

        public int GetSubhash(int l, int r)
        {
            return Add(h[r], -Mul(h[l - 1], p[r - l + 1]));
        }
        
        public void Print()
        {
            for (int i = 0; i < h.Count; ++i)
            {
                Console.WriteLine($"h[{i}] = {h[i]}");
            }
        }

        public List<int> Positions(HashArray TextHashArray)
        {
            List<int> pos = new();

            if (TextHashArray is null) return pos;

            if (h[this.Length() - 1] == TextHashArray[this.Length() - 1])
            {
                pos.Add(0);
            }

            for (int i = 0; i <= TextHashArray.Length() - this.Length(); ++i)
            {
                // Console.WriteLine($"{i}: {get_subhash(h_t, i + 1, i + s.Length - 1)}");
                if (h[this.Length() - 1] == TextHashArray.GetSubhash(i + 1, i + this.Length() - 1))
                {
                    pos.Add(i);
                }
            }
            return pos;
        }
    }
}
