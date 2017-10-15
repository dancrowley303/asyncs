using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asyncs
{
    public class APMFib
    {
        private Func<int, int> f = Fibonacci;

        public static int Fibonacci(int k)
        {
            if (k == 1) return 1;
            if (k == 0) return 0;
            return Fibonacci(k - 1) + Fibonacci(k - 2);
        }

        public IAsyncResult BeginFibonacci(int k, AsyncCallback callback, Object state)
        {
            return f.BeginInvoke(k, callback, state);
        }

        public int EndFibonacci(IAsyncResult result)
        {
            return f.EndInvoke(result);
        }
    }
}
