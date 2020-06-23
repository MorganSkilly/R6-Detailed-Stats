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

            await _commands.ExecuteAsync(
            context: context,
            argPos: argPos,
            services: null);
        }
    }
}
