using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace asyncs
{
    public class FibonacciCompletedEventArgs : AsyncCompletedEventArgs
    {
        public FibonacciCompletedEventArgs(Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
        {
        }

        public int Result { get; set; }
    }

    public delegate void FibonacciCompletedEventHandler(object sender, FibonacciCompletedEventArgs e);


    public class EAPFib
    {
        public event FibonacciCompletedEventHandler FibonacciCompleted;
        private delegate void WorkerEventHandler(int k, AsyncOperation asyncOp);
        private SendOrPostCallback onCompletedDelegate;
        private HybridDictionary tasks = new HybridDictionary();

        protected virtual void InitializeDelegates()
        {
            onCompletedDelegate = new SendOrPostCallback(WorkCompleted);
        }

        public EAPFib()
        {
            InitializeDelegates();
        }

        private void WorkCompleted(object operationState)
        {
            FibonacciCompletedEventArgs e = operationState as FibonacciCompletedEventArgs;
            OnFibonacciCompleted(e);
        }

        protected void OnFibonacciCompleted(FibonacciCompletedEventArgs e)
        {
            FibonacciCompleted?.Invoke(this, e);
        }

        public int Fibonacci(int k)
        {
            if (k == 1) return 1;
            if (k == 0) return 0;
            return Fibonacci(k - 1) + Fibonacci(k - 2);
        }

        public void FibonacciAsync(int k, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(userState);
            lock(tasks.SyncRoot)
            {
                if (tasks.Contains(userState))
                {
                    throw new ArgumentException("User state parameters must be unique", "userState");
                }
                tasks[userState] = asyncOp;
            }

            WorkerEventHandler worker = new WorkerEventHandler(FibonacciWorker);
            worker.BeginInvoke(k, asyncOp, null, null);
        }

        private void FibonacciWorker(int k, AsyncOperation asyncOp)
        {
            int result = Fibonacci(k);
            lock (tasks.SyncRoot)
            {
                tasks.Remove(asyncOp.UserSuppliedState);
            }
            FibonacciCompletedEventArgs e = new FibonacciCompletedEventArgs(null, false, asyncOp.UserSuppliedState);
            e.Result = result;
            asyncOp.PostOperationCompleted(onCompletedDelegate, e);
        }

    }
}
