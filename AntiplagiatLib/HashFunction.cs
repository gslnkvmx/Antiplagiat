using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiplagiatLib
{
    public abstract class HashFunction
    {
        const int MOD = 1_000_000_007;
        const int N = 1_000_000;
        internal const int P = 131;
        internal List<int> p = new();
        public HashFunction() 
        {
            Init();
        }
        // test pull request from sublime text
        internal static int Mul(long a, int b)
        {
            return (int)(a * b % MOD);
        
        }

        internal static int Add(long a, int b)
        {
            a += b;
            if (a < 0) a += MOD;
            if (a >= MOD) a -= MOD;

            return (int)a;
        }

        void Init()
        {
            p.Add(1);
            for (int i = 1; i <= N; ++i)
            {
                p.Add(Mul(p[i - 1], P));
                // Console.WriteLine(p[i]);
            }
        }
    }
}
