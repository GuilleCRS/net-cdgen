using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.RegularExpressions;
using static net_cdgen.Resources.HelpMethods_;

namespace net_cdgen.ModelHandler
{
    public class CreateModel
    {
          private string[] args;

        public CreateModel(string[] args)
        {
            this.args = args;
            if (args.Length >= 7)
            {
                if (args[1] == "-m" || args[1] == "--model")
                {
                    if (args[3] == "-nm" || args[3] == "--namespace")
                    {
                        if (args[5] == "-a" || args[3] == "--attributes")
                        {
                        
                            Start();
                        }
                        else
                        {
                            PrintError($"Error: Unexpected {args[5]} argument, expecting [-a | --attributes].");
                        }
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
                PrintError("Error: Expected at least 7 arguments, received " + this.args.Length);
            }
        }

        private void Start()
        {
            string model = args[2];
            string nmspace = args[4];
            string modelName = model + ".cs";
            string directoryName = @"Models";
            bool directoryExists = Directory.Exists(directoryName);
            bool modelExists = File.Exists(@"Models/" + modelName);

            try
            {
                if (directoryExists)
                {
                    Console.WriteLine($"Creating {model} model!");
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
                string modelPath = Path.Combine(directoryName, modelName);
                
                if (modelExists)
                {
                    PrintMessage($"Model {model}.cs was found, would you like to ovwerwrite it? Y/N");
                    var answer = Console.ReadLine()?.ToLower();
                    if (!answer.Contains("y"))
                    {
                        PrintMessage("Aborting model creation.");
                        return;
                    }
                }


                FileStream fs = File.Create(modelPath);
                fs.Close();
                Console.WriteLine("eh");
                var templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                    @"ModelHandler/Templates/SimpleModelTemplate.txt");
                string text = File.ReadAllText(templatePath);
                text = text.Replace("@NAMESPACE", $"{nmspace}");
                text = text.Replace("@MODEL", model);
                text = text.Replace("@PARAMS", GenerateParams(args));
                File.WriteAllText(modelPath, text);
                PrintMessage($"Model: {model}.cs creation completed!"  );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private string GenerateParams(string[] args)
        {
            string _params="";
            string argument = "";
            for (int i = 6; i < args.Length; i++)
            {
                argument += args[i];
            }

            argument = argument.Substring(argument.IndexOf("[", StringComparison.Ordinal), argument.IndexOf("]", StringComparison.Ordinal));
            argument = argument.Replace("[", "");
            argument = argument.Replace("]", "");
            if (!argument.Contains(":"))
            {
                PrintError("Unknown parameter format! Expected: [type:name,...]");
                Environment.Exit(-1);
            }

            var parameters = argument.Split(',');
            List<string> types = new List<string>();List<string> names = new List<string>();
            foreach (var str in parameters)
            {
                var t_n = str.Split(':');
                if (!(t_n.Length == 2))
                {
                    PrintError("Unknown parameter format! Expected: [type:name,...]");
                    Environment.Exit(-1);
                }

                var match1 = Regex.Match(t_n[0], @"^\w+$").Success;
                var match2 = Regex.Match(t_n[1], @"^\w+$").Success;
                if (match1 && match2)
                {
                    types.Add(t_n[0]);
                    names.Add(t_n[1]);
                }
                else
                {
                    PrintError("Unknown type/name format! Only A-Z, a-z, _ accepted\n" +
                               "Error=> "+((match1) ? "" : $"Type: {t_n[0]}"+"  ") + ((match2) ? " " : $"Name: {t_n[1]}"));
                    Environment.Exit(-1);
                }

            }
            for (int i = 0; i < types.Count; i++)
            {
                _params += $"\t\tpublic {types[i]} {names[i]}" + " { get; set; }\n";
            }
            return _params;
        }

    }
}