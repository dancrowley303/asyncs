using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace asyncs
{
    class Program
    {
        static void Main(string[] args)
        {
            Task[] tasks = new Task[3];
            tasks[0] = Task.Run(() => RunAPM(35));
            tasks[1] = Task.Run(() => RunEAP(33));
            tasks[2] = Task.Run(() => RunTAP(30));
            Task.WaitAll(tasks);
        }

        private static void RunAPM(int k)
        {
            Console.WriteLine("starting APM");
            var apm = new APMFib();
            var asyncResult = apm.BeginFibonacci(k, null, null);
            Console.WriteLine("doing some other work in APM");
            var result = apm.EndFibonacci(asyncResult);
            Console.WriteLine("result from APM was {0}", result);
        }

        private static void RunEAP(int k)
        {
            Console.WriteLine("Starting EAP");
            var eap = new EAPFib();
            var auto = new AutoResetEvent(false);
            eap.FibonacciAsync(k, auto);
            eap.FibonacciCompleted += (s, e) =>
            {
                Console.WriteLine("result from EAP was {0}", e.Result);
                var autoReturned = e.UserState as AutoResetEvent;
                autoReturned.Set();
            };
            Console.WriteLine("Doing some other work in EAP");
            auto.WaitOne();
        }

        private static void RunTAP(int k)
        {
            Console.WriteLine("Starting TAP");
            var tap = new TAPFib();
            var tapTask = tap.FibonacciAsync(k);
            Console.WriteLine("Doing some other work in TAP");
            var result = tapTask.Result;
            Console.WriteLine("result from TAP was {0}", result);
        }
    }
}
