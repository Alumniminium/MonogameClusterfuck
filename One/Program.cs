using System;
using One.Systems;

namespace MonoGameClusterFuck
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            ThreadedConsole.WriteLine("Starting the engine...");
            using (var game = new Engine())
                game.Run();
            ThreadedConsole.WriteLine("Engine shut down, exiting...");
        }
    }
}
