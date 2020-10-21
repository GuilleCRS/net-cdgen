using System;
using System.IO;
using System.Reflection;

namespace net_dcdgen
{
    public class DocPrinter
    {
        public DocPrinter(string path)
        {
            var mainPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(mainPath, path);
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