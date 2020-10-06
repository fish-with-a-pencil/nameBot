using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
            private DiscordSocketClient _client;

            public async Task MainAsync() {
              _client = new DiscordSocketClient();
              _client.Log += Log;
              await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
              await _client.StartAsync();

              await Task.Delay(-1);
            }


          public class CommandHandler {
            private readonly DiscordSocketClient _client;
            private readonly CommandService _commands;

            public CommandHandler(DiscordSocketClient client, CommandService commands) {
              _commands = commands;
              _client = client;
            }

            public async Task InstallCommandsAsync() {
              _client.MessageRecieved += HandleCommandAsync;
              await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
            }

            private async Task HandleCommandAsync(SocketMessage messageParam){
              var message = messageParam as SocketUserMessage;
              if (message == null) return;
              int argpos = 0;
              if (!(message.HasCharPrefix('~', ref argPos) ||
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot) return;

              var context = new SocketCommandContext(_client, message);
              var result = await _commands.ExecuteAsync(
            context: context,
            argPos: argPos,
            services: null);
            }


          }


    }
}
