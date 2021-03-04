using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SusBot.Modules
{
    [Group("help")]
    public class HelpCommands : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task Help()
        {
            var embed = new EmbedBuilder();
            // add data to embedBuilder
            embed.AddField("Readme page",
                "Click [here](https://github.com/Paxygit/Susbot/blob/master/README.md) to see the Susbot Readme page!")
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = "Thats awfully sus...")
                .WithColor(Color.Red)
                .WithCurrentTimestamp();

            //send embed
            await ReplyAsync(embed: embed.Build());
        }

        [Command("who")]
        public async Task helpWho()
        {
            var embed = new EmbedBuilder();
            // add data to embedBuilder
            embed.Title = ".who";
            embed.AddField("Usage",
                "This command is used to tell everybody who is sus! ")
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = "Thats awfully sus...")
                .WithColor(Color.Red)
                .WithCurrentTimestamp();

            //send embed
            await ReplyAsync(embed: embed.Build());
        }
    }
}
