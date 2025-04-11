using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadtreeCompression
{
    internal static class Loading
    {
        private static Thread _thread;
        private static bool _running;
        private static readonly int _intervalMs = 500;

        public static void Start()
        {
            if (_running) return;
            Console.Write("\n=============================\n\n");
            _running = true;
            _thread = new Thread(Animate);
            _thread.Start();
        }

        public static void Stop()
        {
            _running = false;
            _thread?.Join();

            // Clear the line after stopping
            Console.Write("\r" + "             DONE                ");
            Console.Write("\n\n=============================\n\n");
        }

        private static void Animate()
        {
            int dotCount = 0;
            while (_running)
            {
                string dots = new string('.', dotCount + 1);
                Console.Write($"\rProcessing{dots}   "); // Extra spaces to overwrite previous
                dotCount = (dotCount + 1) % 3;

                Thread.Sleep(_intervalMs);
            }
        }
    }
}
