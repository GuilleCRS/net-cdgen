using System;
using System.IO;

namespace net_dcdgen
{
    public class DocPrinter
    {
        public DocPrinter(string path)
        {
            var lines = File.ReadLines(path);
            foreach (var line in lines)
                if (line.Contains("dngen"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(line);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine(line);
                }
        }
    }
}