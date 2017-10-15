using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asyncs
{
    public class TAPFib
    {
        public static int Fibonacci(int k)
        {
            if (k == 1) return 1;
            if (k == 0) return 0;
            return Fibonacci(k - 1) + Fibonacci(k - 2);
        }

        public Task<int> FibonacciAsync(int k)
        {
            return Task.Run(() => { return Fibonacci(k); });
        }
    }
}
