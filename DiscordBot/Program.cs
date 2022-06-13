using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace DiscordBot4Real
{
    class Program
    {
        Random rand = new Random();

        //Bot's token
        private string token = ConfigurationManager.AppSettings["apiToken"];

        private DiscordSocketClient _client;
        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.Ready += Ready;
            _client.MessageReceived += MessageReceivedAsync;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task Ready()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {

            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            //List of bad words
            var bwords = new List<string> { "imbecyl", "szajbus", "cymbał", "gamoń", "matoł", "głąb", "przygłup", "bęcwał", "dureń", "bałwan", "kretyn", "tuman", "frajer", "osioł", "baran", "świrus", "głup", "cap", "świr", "czub", "kiep", "muł", "półgłówek", "głupol", "palant", "cep", "pacan" };
            bool isToxic = false;

            //Checking if sent msg contains badword from list
            foreach (var w in bwords)
            {
                if (message.Content.Contains(w, StringComparison.OrdinalIgnoreCase)) isToxic = true;
            }


            //Emote +1 when bad word is used with pacholik
            var plusEmoji = new Emoji("➕"); //+
            var oneEmoji = new Emoji("1️⃣"); //1

            if ((message.Content.Contains("pacholik", StringComparison.OrdinalIgnoreCase) || message.Content.Contains("jacob", StringComparison.OrdinalIgnoreCase) || message.Content.Contains("wat", StringComparison.OrdinalIgnoreCase)) && isToxic)
            {
                await message.AddReactionAsync(plusEmoji);
                await message.AddReactionAsync(oneEmoji);

            }


            // Deleting msges containing bad word and my name
            if ((message.Content.Contains("zaton", StringComparison.OrdinalIgnoreCase) || message.Content.Contains("zatoń", StringComparison.OrdinalIgnoreCase) || message.Content.Contains("kariwan", StringComparison.OrdinalIgnoreCase)) && isToxic)
                await message.Channel.DeleteMessageAsync(message);


            //Testing direct msg
            if (message.Content.Equals("!help", StringComparison.OrdinalIgnoreCase))
                await message.Author.SendMessageAsync("```fix\n I'm helping beep boop or something I dunno``` ```help please```");


            //Testing sending file
            if (message.Content.Contains("bloczek", StringComparison.OrdinalIgnoreCase))
                await message.Channel.SendFileAsync("/home/testBot/bloczekfinalgif.gif");


            //sending xkcd random image
            if (message.Content.Equals("!xkcd", StringComparison.OrdinalIgnoreCase))
            {
                int r = rand.Next(1, 2500);
                string url = "https://xkcd.com/" + r.ToString();

                await message.Channel.SendMessageAsync(url);
            }


        }

    }
}

