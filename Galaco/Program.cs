using IA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Galaco
{
    internal class Program
    {
        private static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        private async Task Start()
        {
            ClientInformation clientInfo = LoadKeyFromConfig();

            Bot bot = new Bot(clientInfo);

            await bot.ConnectAsync();
        }

        private ClientInformation LoadKeyFromConfig()
        {
            ClientInformation info = new ClientInformation();

            string file = Directory.GetCurrentDirectory() + "/config/settings.config";

            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);

                while (!sr.EndOfStream)
                {
                    string x = sr.ReadLine();

                    if (x.StartsWith("#")) continue;

                    PropertyInfo propertyInfo = info.GetType().GetProperty(x.Split(':')[0]);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(info, Convert.ChangeType(x.Split(':')[1], propertyInfo.PropertyType), null);
                    }
                    else
                    {
                        Log.Warning($"{x} is not a valid option.");
                    }
                }
                sr.Close();
                return info;
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/config");

                StreamWriter sw = new StreamWriter(file);

                List<string> allProperties = new List<string>();

                List<PropertyInfo> properties = new List<PropertyInfo>();
                properties.AddRange(typeof(ClientInformation).GetProperties());

                //TODO: find a more elegant solution for this
                properties.Remove(properties.Find(x => { return x.Name == "EventLoaderMethod"; }));

                foreach(PropertyInfo p in properties)
                {
                    allProperties.Add(p.Name);
                }

                WriteComments(sw,
                    "GALACO bot settings file v1.0",
                    "==============================",
                    "Here's a quick tutorial for you to understand GALACO's settings in a breeze!",
                    "Firstly, comments HAVE to start with a #. as you can see from this file.",
                    "Secondly, write a property like this: property:value. One on each line",
                    "There are a handful of settings you can change to however you want, I'll put a full list on the bottom!",
                    "-----------------------------",
                    "Created by: Veld#5128",
                    "-----------------------------",
                    "ALL PROPERTIES AVAILABLE",
                    string.Join(", ", allProperties.ToArray()),
                    "-----------------------------"
                    );
                sw.WriteLine("Name:GALACO");
                sw.WriteLine("Version:1.0");
                sw.WriteLine("ShardCount:1");
                sw.Close();

                Console.WriteLine("First run detected!");
                Console.WriteLine($"Created a config file at:\n {file}\n\nPress enter to continue...");
                Console.ReadLine();
            }
            return null;
        }

        void WriteComments(StreamWriter sw, params string[] lines)
        {
            foreach(string s in lines)
            {
                sw.WriteLine("# " + s);
            }
        }
    }
}