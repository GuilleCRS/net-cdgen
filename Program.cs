using System;
using System.IO;
using System.Reflection;
using net_cdgen.ControllerHandler;
using net_cdgen.ModelHandler;
using net_cdgen.ServiceHandler;
using net_dcdgen;

namespace net_cdgen
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var mainPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (args.Length > 1)
            {
                switch (args[0])
                {
                    case "controller":
                        new CreateController(args);
                        break;
                    case "model":
                        new CreateModel(args);
                        break;
                    case "service":
                        new CreateService(args);
                        break;
                }
            }
            else
            {
                var mainHelp = Path.Combine(mainPath, @"Docs/Help.txt");
                new DocPrinter(mainHelp);
            }
        }
        
    }
}