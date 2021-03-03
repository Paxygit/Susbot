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
using Susbot.Modules;
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
            foreach (string strang in terms) 
            {
                if (message.Content.ToLower().Contains(strang))
                {
                    var role = context.Guild.Roles.FirstOrDefault(x => x.Name == "Sus");

                    if (File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\" + context.Guild.Id + @"_SusData.json") != "")
                    {
                        SusData oldSusData = JsonConvert.DeserializeObject<SusData>(File.ReadAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\" + context.Guild.Id + @"_SusData.json"));
                        IGuildUser prevUser = guild.GetUser(oldSusData.id);
                        await prevUser.RemoveRoleAsync(role);
                    }



                    var summonMessage = message;
                    var user = message.Author.Username;
                    var channel = discClient.GetChannel(message.Channel.Id) as IMessageChannel;
                    await channel.SendMessageAsync("***Emergency Meeting!*** " + message.Author.Username + " is now sus!");

                    //REFORMAT TO JSON
                    SusData susData = new SusData {username = user, id = message.Author.Id };

                    string susDataText = JsonConvert.SerializeObject(susData);

                    File.WriteAllText(@"C:\Users\theon\source\repos\Susbot'\Susbot'\Modules\" + context.Guild.Id + @"_SusData.json", susDataText);

                    await (message.Author as IGuildUser).AddRoleAsync(role);
                    return;
                }
            } //Loops for marked messages to pass sus role and return
            return;
        }//Handle
    }
}
