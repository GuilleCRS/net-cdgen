using System;
using System.IO;
using System.Reflection;
using net_dcdgen;
using static net_cdgen.Resources.HelpMethods_;

namespace net_cdgen.ServiceHandler
{
    public class CreateService
    {
        private readonly string[] args;

        public CreateService(string[] args)
        {
            this.args = args;
            if(args.Length==1)
            {
                new DocPrinter( @"Docs/Service.txt");
            }
            else if (args.Length == 6)
            {
                if (args[1] == "-m" || args[1] == "--model")
                {
                    if (args[3] == "-nm" || args[3] == "--namespace")
                    {
                        if (args[5] == "-mongo" || args[5] == "-sql")
                            Start(args[5]);
                        else
                            PrintError($"Error: Unexpected {args[5]} argument, expecting [--mongo | --sql].");
                    }
                    else
                    {
                        PrintError($"Error: Unexpected {args[3]} argument, expecting [-nm | --namespace].");
                    }
                }
                else
                {
                    PrintError($"Error: Unexpected {args[1]} argument, expecting [-m | --model].");
                }
            }
            else
            {
                new DocPrinter( @"Docs/Service.txt");
                PrintError("\nError: 5 Arguments expected received " + this.args.Length);
            }
        }

        private void Start(string type)
        {
            var model = args[2];
            var nmspace = args[4];
            var serviceName = model + "Service.cs";
            var directoryName = @"Services";
            var directoryExists = Directory.Exists(directoryName);
            var modelExists = File.Exists(@"Models/" + model + ".cs");
            var fileExists = File.Exists(@"Services/" + serviceName);

            try
            {
                if (directoryExists)
                {
                    Console.WriteLine($"Creating {model} service!");
                }
                else
                {
                    var di = Directory.CreateDirectory(directoryName);
                    PrintMessage(
                        $"The directory {directoryName} was created successfully at {Directory.GetCreationTime(directoryName)}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                var controllerPath = Path.Combine(directoryName, serviceName);

                if (fileExists)
                {
                    PrintMessage($"Service {model}Service.cs was found, would you like to ovwerwrite it? Y/N");
                    var answer = Console.ReadLine()?.ToLower();
                    if (!answer.Contains("y"))
                    {
                        PrintMessage("Aborting service creation.");
                        return;
                    }
                }

                if (!modelExists)
                {
                    PrintMessage($"Model {model} was not found, would you like to proceed? Y/N");
                    var answer = Console.ReadLine()?.ToLower();
                    if (!answer.Contains("y"))
                    {
                        PrintMessage("Aborting service creation.");
                        return;
                    }
                }


                var fs = File.Create(controllerPath);
                fs.Close();
                var templatePath = "";
                if (type == "--mongo")
                    templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                        @"ServiceHandler/Templates/MongoServiceTemplate.txt");
                else if (type == "--sql")
                    templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                        @"ServiceHandler/Templates/SqlServiceTemplate.txt");
                var text = File.ReadAllText(templatePath);
                text = text.Replace("@NAMESPACE", $"{nmspace}");
                text = text.Replace("@MODEL", model);
                //text = text.Replace("@CONTROLLER", $"{model}Controller");
                text = text.Replace("@MTL", $"{model.ToLower()}");
                File.WriteAllText(controllerPath, text);
                PrintMessage($"Service: {model}Service.cs creation completed!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}