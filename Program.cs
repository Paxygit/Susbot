using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Susbot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunASync().GetAwaiter().GetResult();


        private string[] terms = { "amogus", "amog us", "among us", "amongus", "vent", "sus", "electrical", "impasta", "imposter", "task" }; //evil terms
        private DiscordSocketClient discClient;
        private CommandService discCommandService;
        private IServiceProvider discService;
        
        public async Task RunASync()
        {
            discClient = new DiscordSocketClient();
            discCommandService = new CommandService();
            discService = new ServiceCollection().AddSingleton(discClient).AddSingleton(discCommandService).BuildServiceProvider();
            Susbot.Modules.Token token = JsonConvert.DeserializeObject<Susbot.Modules.Token>(File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\token.json"));

            discClient.Log += DiscClient_Log;
            await RegisterCommandsAsync();
            await discClient.LoginAsync(TokenType.Bot, token.key);
            await discClient.StartAsync();
           

            await Task.Delay(-1);
        }//Init

        private Task DiscClient_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }//For console log

        public async Task RegisterCommandsAsync()
        {
            discClient.MessageReceived += HandleCommandAsync;
            await discCommandService.AddModulesAsync(Assembly.GetEntryAssembly(), discService);
        }//Pass to message Handler

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(discClient, message);
            var guild = context.Guild;
            await guild.DownloadUsersAsync();
            int argPos = 0;
            
            if (message.Author.IsBot) return; //Ignore bot

            if (message.HasStringPrefix(".", ref argPos))
            {
                var result = await discCommandService.ExecuteAsync(context, argPos, discService);
                if (!result.IsSuccess) Console.WriteLine(result.Error + ": " + result.ErrorReason);
            } //Finds . Commands, init command service
            foreach (string strang in terms) {
                if (message.Content.ToLower().Contains(strang))
                {
                    var role = context.Guild.Roles.FirstOrDefault(x => x.Name == "Sus");

                    if (File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\CurrentSus.txt") != "")
                    {
                        var prevId = Convert.ToUInt64(File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\CurrentSusID.txt"));
                        var prevUsername = File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\CurrentSus.txt");
                        IGuildUser prevUser = guild.GetUser(prevId);
                        await prevUser.RemoveRoleAsync(role);
                    }



                    var summonMessage = message;
                    var user = message.Author.Username;
                    var channel = discClient.GetChannel(message.Channel.Id) as IMessageChannel;
                    await channel.SendMessageAsync("***Emergency Meeting!*** " + message.Author.Username + " is now sus!");

                    File.WriteAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\CurrentSus.txt", message.Author.Username);
                    File.WriteAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\CurrentSusID.txt", message.Author.Id.ToString());
                    await (message.Author as IGuildUser).AddRoleAsync(role);
                }
            } //Loops for marked messages to pass sus role
            return;
        }//Handle
    }
}
