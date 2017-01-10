using System;
using ParseLnk;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("No file specified");
                return;
            }

            var parseLnk = new Parser(args[0]);
            parseLnk.Parse();
        }
    }
}
