using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MonoGameClusterFuck.Systems
{
    public static class ThreadedConsole
    {
        private static readonly AutoResetEvent Block= new AutoResetEvent(false);
        private static readonly ConcurrentQueue<string> WriteQueue = new ConcurrentQueue<string>();
        static ThreadedConsole()
        {
            var workThread = new Thread(WorkLoop)
            {
                IsBackground = true
            };
            workThread.Start();
        }
        public static void WriteLine(string text)
        {
            WriteQueue.Enqueue(text);
            Block.Set();
        }
        private static void WorkLoop()
        {
            while (true)
            {
                Block.WaitOne();

                while (!WriteQueue.IsEmpty)
                {
                    if (WriteQueue.TryDequeue(out var obj))
                        Console.WriteLine(obj);
                }
            }
        }
    }
}