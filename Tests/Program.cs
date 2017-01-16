using System;
using System.IO;
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

            StreamReader streamReader;

            try
            {
                streamReader = new StreamReader(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to open StreamReader: {e.Message}");
                return;
            }

            try
            {
                var parseLnk = new Parser(streamReader.BaseStream);
                parseLnk.Parse();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to parse file: {e.Message}");
                return;
            }
            

        }
    }
}
