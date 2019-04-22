using System;
using System.Threading;
using System.Collections.Concurrent;

namespace One.Systems
{
    public static class ThreadedConsole
    {
        private static Thread _workThread;
        private static AutoResetEvent _block= new AutoResetEvent(false);
        private static ConcurrentQueue<string> _writeQueue = new ConcurrentQueue<string>();
        static ThreadedConsole()
        {
            _workThread = new Thread(WorkLoop);
            _workThread.Start();
        }
        public static void WriteLine(string text)
        {
            _writeQueue.Enqueue(text);
            _block.Set();
        }
        private static void WorkLoop()
        {
            while (true)
            {
                _block.WaitOne();

                while (!_writeQueue.IsEmpty)
                {
                    if (_writeQueue.TryDequeue(out var obj))
                        Console.WriteLine(obj);
                }
            }
        }
    }
}