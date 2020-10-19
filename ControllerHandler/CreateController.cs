using System;
using System.IO;
using System.Reflection;

namespace net_cdgen.ControllerHandler
{
    public class CreateController
    {
        private string[] args;
        public CreateController(string[] args)
        {
            this.args = args;
            if (args[1] == "-m" || args[1] == "--model")
            {
                string model = args[2];
                Console.WriteLine($"Creating {model} controller!");
                if (args[3] == "-nm" || args[3] == "--namespace")
                {
                    string nmspace = args[4];
                    Console.WriteLine(nmspace);
                    string controllersDirPath = $"./Controllers";
                    string controllerPath = $"{controllersDirPath}/{model}Controller.cs";
                    try
                    {
                        if (Directory.Exists(controllersDirPath))
                        {
                            Console.WriteLine(controllersDirPath);
                        }
                        else
                        {
                            DirectoryInfo di = Directory.CreateDirectory(controllersDirPath);
                            Console.WriteLine("The directory was created successfully at {0}.",
                                Directory.GetCreationTime(controllersDirPath));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    finally
                    {
                        try
                        {
                            FileStream fs = File.Create(controllerPath);
                            fs.Close();
                            var currDir = Directory.GetCurrentDirectory();
                            FileInfo f = new FileInfo("BaseControllerTemplate.txt");
                            var templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,@"ControllerHandler/Templates/ControllerBaseTemplate.txt");
                            Console.WriteLine(templatePath);
                            string text = File.ReadAllText(templatePath);
                            text = text.Replace("@NAMESPACE", $"{nmspace}");
                            text = text.Replace("@MODEL", model);
                            text = text.Replace("@CONTROLLER", $"{model}Controller");
                            text = text.Replace("@MTL", $"{model.ToLower()}");
                            File.WriteAllText(controllerPath,text);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }
        }
    }
}