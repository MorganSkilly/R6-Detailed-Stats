using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace R6DetailedStats
{
    [Group("!r6")]
    public class commands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task help()
        {
            await ReplyAsync("!m commands");
        }

        [Command("main")]
        public async Task main(string username, string platform = "pc")
        {
            HTMLDataParser parser = new HTMLDataParser();

            if (await parser.UpdateDocAsync("https://r6.tracker.network/profile/" + platform + "/" + username + "/operators?seasonal=1"))
                Console.WriteLine("HTML doc updated.");
            else
                Console.WriteLine("Couldn't update HTML doc!");

            string profilePic = parser.ParseImgUrl("//*[@id='profile']/div[1]/div/div[1]/img");
            string attackSeason1 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span");
            string attackSeason2 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[1]/span");
            string defenseSeason1 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span");
            string defenseSeason2 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[1]/span");


            if (await parser.UpdateDocAsync("https://r6.tracker.network/profile/" + platform + "/" + username + "/operators"))
                Console.WriteLine("HTML doc updated.");
            else
                Console.WriteLine("Couldn't update HTML doc!");

            string attackOverall1 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span");
            string attackOverall2 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[1]/span");
            string defenseOverall1 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span");
            string defenseOverall2 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[1]/span");

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("Top Attackers (Seasonal)",
                "1: " + attackSeason1 + "\n2: " + attackSeason2)
                .WithFooter(footer => footer.Text = "R6 stats powered by Morgan Bot")
                .WithColor(Color.Purple)
                .WithTitle("Top Operators: " + username)
                .WithUrl("https://morgan.games/discord-bot/")
                .WithCurrentTimestamp()
                .WithThumbnailUrl(profilePic)
                .Build();

            embed.AddField("Top Defenders (Seasonal)",
                "1: " + defenseSeason1 + "\n2: " + defenseSeason2);

            embed.AddField("Top Attackers (Overall)",
                "1: " + attackOverall1 + "\n2: " + attackOverall2);

            embed.AddField("Top Defenders (Overall)",
                "1: " + defenseOverall1 + "\n2: " + defenseOverall2);

            await ReplyAsync(null, false, embed.Build(), null);
        }

        [Command("embed")]
        public async Task SendRichEmbedAsync()
        {
            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("Field title",
                "Field value. [good website](https://morgan.games)!")
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = "I am a footer.")
                .WithColor(Color.Blue)
                .WithTitle("I overwrote \"Hello world!\"")
                .WithDescription("I am a description.")
                .WithUrl("https://example.com")
                .WithCurrentTimestamp()
                .Build();
            await ReplyAsync(null, false, embed.Build(), null);
        }
    }
}
