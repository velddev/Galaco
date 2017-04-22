using IA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaco
{
    class Hotloader
    {
        Bot bot;
        FileSystemWatcher hotloader;

        public Hotloader(Bot bot)
        {
            this.bot = bot;
            hotloader = new FileSystemWatcher(Directory.GetCurrentDirectory() + "/modules", "*.dll");
        }

        public void Run()
        {
            hotloader.EnableRaisingEvents = true;

            hotloader.Created += async (sender, args) => 
            {
                await bot.Addons.LoadSpecific(bot, args.Name.Remove(args.Name.Length - 4));
            };

            hotloader.Changed += async (sender, args) => 
            {
                await bot.Addons.Reload(bot, args.Name.Remove(args.Name.Length - 4));
            };

            hotloader.Deleted += async (sender, args) =>
            {
                await bot.Addons.Unload(bot, args.Name.Remove(args.Name.Length - 4));
            };
        }
    }
}
