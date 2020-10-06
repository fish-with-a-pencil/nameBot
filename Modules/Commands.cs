using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using nameBot.Services;

namespace nameBot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
            => ReplyAsync("pong!");

            [Command("fname")]
               public async Task fname(){
                await ReplyAsync(randFName());
               }

     private string randFName() {
          var rand = new Random();
          string[] vowels = File.ReadAllLines("vowels.txt");
          string[] consonants = File.ReadAllLines("consonants.txt");
          string[] blend = File.ReadAllLines("blendable.txt");
          string[] endings = File.ReadAllLines("endings.txt");
          string name = "", syllable="";
          int syllables = rand.Next(1, 5);
          for (int i=0; i >= syllables; i++) {
            syllable = "";
            if(i == 0 && (rand.Next(0,2) == 0)) {
              name += vowels[rand.Next(0, vowels.Length)];
            }
            if(i == syllables && (rand.Next(0,2) == 0))name += endings[rand.Next(0, endings.Length)];
            else {
              syllable += consonants[rand.Next(0, consonants.Length)];
              syllable += vowels[rand.Next(0, vowels.Length)];
              name += syllable;
              break;
              }
              //middle consonants
              if(rand.Next(0,2) == 0) {
                syllable += blend[rand.Next(0, blend.Length)];
                //add blending consonant later, for now just use l
                syllable += 'l';
              }
              else {
                syllable += consonants[rand.Next(0, consonants.Length)];
              }
              syllable += vowels[rand.Next(0, vowels.Length)];
              name += syllable;
          }

          return name;
     }

    }
}
