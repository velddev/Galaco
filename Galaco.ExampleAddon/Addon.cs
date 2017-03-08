using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IA.SDK;
using IA.SDK.Interfaces;

namespace Galaco.ExampleAddon
{
    public class Addon : IAddon
    {
        public async Task<IAddonInstance> Create(IAddonInstance i)
        {
            i.Modules.Add(
                new Module(x =>
                {
                    x.Name = "HelloWorld";
                    x.Events = new List<IA.SDK.Events.ICommandEvent>()
                    {
                        new CommandEvent(command =>
                        {
                            command.Name = "hello";
                            command.ProcessCommand = async (e, args) =>
                            {
                                IDiscordEmbed embed = e.CreateEmbed();
                                embed.Description = "Hello, Galaco here! <3";
                                embed.Color = new Color(1, 0, 0);
                                embed.ImageUrl = "http://vignette1.wikia.nocookie.net/vocaloid/images/f/f8/Galaco_300x172.png/revision/latest?cb=20151123204009";
                                await e.Channel.SendMessage(embed);
                            };
                        })
                    };
                })
            );
            return i; 
        }
    }
}
