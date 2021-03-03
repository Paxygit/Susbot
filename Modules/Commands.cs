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
            try
            {                                                  
                SusData oldSusData = JsonConvert.DeserializeObject<SusData>(File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\" + Context.Guild.Id + @"_SusData.json"));
                await ReplyAsync(oldSusData.username + " is sus...");
            }
            catch
            {
                await ReplyAsync("No sus..." + "\n\n\n``or something is bugged. If I've called somebody sus before in this server, contact Paxy#1234 and he might fix it.``");
            }
        } //Prints current sus from sustxt

        
    }
}
