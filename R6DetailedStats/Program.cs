using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;

namespace R6DetailedStats
{
    class Program
    {
        static private DiscordSocketClient _client;
        static private CommandService _commands;
        static private IServiceProvider _services;

        private static bool isRunning = false;

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string token = "NzIzMzU4NzE3OTI3NjIwNjQw.Xuz-fQ.nM2J1SRSpZzVeTXObTYBLEVmyLc";

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);

        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot)
                return;

            Console.WriteLine(message.ToString());

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
            else
            {
                //await context.Channel.SendMessageAsync(message.ToString());

                checkFullStats(message.ToString(), context, "Couldn't find user: " + message.ToString());
            
            }
        }

        public static async void checkFullStats(string username, SocketCommandContext context, string errorMssg)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync("https://r6.tracker.network/profile/pc/" + username);
                HtmlDocument page1 = new HtmlDocument();
                page1.LoadHtml(html);

                html = await httpClient.GetStringAsync("https://r6.tracker.network/profile/pc/" + username + "/operators?seasonal=1");
                HtmlDocument page2 = new HtmlDocument();
                page2.LoadHtml(html);

                string KD = page1.DocumentNode.SelectSingleNode("//*[@id='profile']/div[3]/div[2]/div[1]/div[2]/div/span[2]").InnerText;
                string HighestMMR = page1.DocumentNode.SelectSingleNode("//*[@id='profile']/div[3]/div[2]/div[3]/div[2]/div[1]/div[2]/div[2]/text()").InnerText;
                string CurrentMMR = page1.DocumentNode.SelectSingleNode("//*[@id='profile']/div[3]/div[2]/div[1]/div[1]/div/div[2]/div[1]/div[2]").InnerText;
                string attack = "not found";
                string defend = "not found";

                if (page2.DocumentNode.SelectSingleNode("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span") != null)
                    attack = page2.DocumentNode.SelectSingleNode("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span").InnerText;
                if (page2.DocumentNode.SelectSingleNode("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span") != null)
                    defend = page2.DocumentNode.SelectSingleNode("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span").InnerText;

                EmbedBuilder embed = new EmbedBuilder();

                embed.AddField("Seasonal KD",
                    KD)
                    .WithFooter(footer => footer.Text = "R6 stats powered by Morgan Bot")
                    .WithColor(Color.Red)
                    .WithTitle("R6 Stats: " + username)
                    .WithDescription("SEASONAL STATS FOR " + username.ToUpper())
                    .WithUrl("https://r6.tracker.network/profile/pc/" + username)
                    .WithCurrentTimestamp()
                    .Build();
                embed.AddField("Top Attackers: ", attack);
                embed.AddField("Top Defenders: ", defend);
                embed.AddField("Highest MMR: ", HighestMMR);
                embed.AddField("Current MMR: ", CurrentMMR);

                await context.Channel.SendMessageAsync(null, false, embed.Build(), null);
            }
            catch(Exception ex)
            {
                await context.Channel.SendMessageAsync(errorMssg);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
