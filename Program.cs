using System;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.API;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Webhook;
using Discord.Net;


namespace Discord
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

            private Task Log(LogMessage msg){
              Console.WriteLine(msg.ToString());
              return Task.CompletedTask;
              }


          public class CommandHandler {
            private readonly DiscordSocketClient _client;
            private readonly CommandService _commands;

            public CommandHandler(DiscordSocketClient client, CommandService commands) {
              _commands = commands;
              _client = client;
            }

            private async Task InstallCommandsAsync() {
              await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
              _client.MessageReceived += HandleCommandAsync;
            }

            private async Task HandleCommandAsync(SocketMessage messageParam){
              var message = messageParam as SocketUserMessage;
              if (message == null) return;
              int argPos = 0;
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
