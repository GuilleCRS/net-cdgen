using System;
using System.IO;
using System.Reflection;
using static net_cdgen.Resources.HelpMethods_;

namespace net_cdgen.ControllerHandler
{
    public class CreateController
    {
        private string[] args;

        public CreateController(string[] args)
        {
            this.args = args;
            if (args.Length == 5)
            {
                if (args[1] == "-m" || args[1] == "--model")
                {
                    if (args[3] == "-nm" || args[3] == "--namespace")
                    {
                        Start();
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
                PrintError("Error: 5 Arguments expected received " + this.args.Length);
            }
        }

        private void Start()
        {
            string model = args[2];
            string nmspace = args[4];
            string controllerName = model + "Controller.cs";
            string directoryName = @"Controllers";
            bool directoryExists = Directory.Exists(directoryName);
            bool modelExists = File.Exists(@"Models/" + model+".cs");
            bool fileExists = File.Exists(@"Controllers/" + controllerName);

            try
            {
                if (directoryExists)
                {
                    Console.WriteLine($"Creating {model} controller!");
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(directoryName);
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
                string controllerPath = Path.Combine(directoryName, controllerName);
                
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


                FileStream fs = File.Create(controllerPath);
                fs.Close();
                FileInfo f = new FileInfo("BaseControllerTemplate.txt");
                var templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                    @"ControllerHandler/Templates/ControllerBaseTemplate.txt");
                string text = File.ReadAllText(templatePath);
                text = text.Replace("@NAMESPACE", $"{nmspace}");
                text = text.Replace("@MODEL", model);
                text = text.Replace("@CONTROLLER", $"{model}Controller");
                text = text.Replace("@MTL", $"{model.ToLower()}");
                File.WriteAllText(controllerPath, text);
                PrintMessage($"Controller: {model}Controller.cs creation completed!"  );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}