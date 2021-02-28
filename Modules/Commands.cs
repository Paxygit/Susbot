using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.IO;
using Discord;
//using Microsoft.Extensions.Configuration;

namespace Susbot.Modules
{

    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            var summonMessage = Context.Message;
            var summonMessageTime = summonMessage.Timestamp;
            await ReplyAsync("Pong!" + "`` in " + DateTime.Now.Subtract(summonMessageTime.DateTime.ToLocalTime()).ToString() + " ms!``");
        } //Simple ping


        [Command("who")]
        public async Task Who()
        {
            var user = File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\CurrentSus.txt");
            if (user == "")
                await ReplyAsync("No sus...");
            await ReplyAsync( user + " is sus...");
        } //Prints current sus from sustxt

        
    }
}
