using System;
using System.IO;
using System.Reflection;
using net_dcdgen;
using static net_cdgen.Resources.HelpMethods_;

namespace net_cdgen.ControllerHandler
{
    public class CreateController
    {
        private readonly string[] args;

        public CreateController(string[] args)
        {
            this.args = args;
            if(args.Length==1)
            {
                new DocPrinter( @"Docs/Controller.txt");
            }
            else if (args.Length == 5)
            {
                if (args[1] == "-m" || args[1] == "--model")
                {
                    if (args[3] == "-nm" || args[3] == "--namespace")
                        Start();
                    else
                        PrintError($"Error: Unexpected {args[3]} argument, expecting [-nm | --namespace].");
                }
                else
                {
                    PrintError($"Error: Unexpected {args[1]} argument, expecting [-m | --model].");
                }
            }
            else
            {
                new DocPrinter( @"Docs/Controller.txt");
                PrintError("\nError: 5 Arguments expected received " + this.args.Length);
            }
        }

        private void Start()
        {
            var model = args[2];
            var nmspace = args[4];
            var controllerName = model + "Controller.cs";
            var directoryName = @"Controllers";
            var directoryExists = Directory.Exists(directoryName);
            var modelExists = File.Exists(@"Models/" + model + ".cs");
            var fileExists = File.Exists(@"Controllers/" + controllerName);

            try
            {
                if (directoryExists)
                {
                    Console.WriteLine($"Creating {model} controller!");
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
                var controllerPath = Path.Combine(directoryName, controllerName);

                if (fileExists)
                {
                    PrintMessage($"Controller {model}Controller.cs was found, would you like to ovwerwrite it? Y/N");
                    var answer = Console.ReadLine()?.ToLower();
                    if (!answer.Contains("y"))
                    {
                        PrintMessage("Aborting controller creation.");
                        return;
                    }
                }

                if (!modelExists)
                {
                    PrintMessage($"Model {model} was not found, would you like to proceed? Y/N");
                    var answer = Console.ReadLine()?.ToLower();
                    if (!answer.Contains("y"))
                    {
                        PrintMessage("Aborting controller creation.");
                        return;
                    }
                }


                var fs = File.Create(controllerPath);
                fs.Close();
                var f = new FileInfo("BaseControllerTemplate.txt");
                var templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                    @"ControllerHandler/Templates/ControllerBaseTemplate.txt");
                var text = File.ReadAllText(templatePath);
                text = text.Replace("@NAMESPACE", $"{nmspace}");
                text = text.Replace("@MODEL", model);
                text = text.Replace("@CONTROLLER", $"{model}Controller");
                text = text.Replace("@MTL", $"{model.ToLower()}");
                File.WriteAllText(controllerPath, text);
                PrintMessage($"Controller: {model}Controller.cs creation completed!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}