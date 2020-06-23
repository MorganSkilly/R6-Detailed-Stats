using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace R6DetailedStats
{
    [Group("!m")]
    public class admincommands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task help()
        {
            if (Context.User.ToString() == "MorganSkilly#0001")
            {
                string serverList = "";

                foreach (SocketGuild guild in Context.Client.Guilds)
                    serverList = serverList + guild.Name + "\n";

                EmbedBuilder embed = new EmbedBuilder();

                embed.AddField("Joined Servers", serverList)
                    .WithFooter(footer => footer.Text = "Powered by Morgan Bot")
                    .WithColor(Color.Blue)
                    .WithTitle("Morgan Bot Server List")
                    .WithUrl("https://morgan.games/discord-bot/")
                    .WithCurrentTimestamp()
                    .Build();

                await ReplyAsync(null, false, embed.Build(), null);
            }
        }
    }

    [Group("!r6")]
    public class commands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task help()
        {
            string commandList = "";

            commandList = commandList + "!r6 help" + "\n";
            commandList = commandList + "!r6 main" + "\n";

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("Commands", commandList)
                .WithFooter(footer => footer.Text = "Powered by Morgan Bot")
                .WithColor(Color.Blue)
                .WithTitle("Morgan Bot Help!")
                .WithUrl("https://morgan.games/discord-bot/")
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(null, false, embed.Build(), null);
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
            string attackSeason1 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[4]");
            string attackSeason2 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[4]");
            string defenseSeason1 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[4]");
            string defenseSeason2 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[4]");


            if (await parser.UpdateDocAsync("https://r6.tracker.network/profile/" + platform + "/" + username + "/operators"))
                Console.WriteLine("HTML doc updated.");
            else
                Console.WriteLine("Couldn't update HTML doc!");

            string attackOverall1 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[4]");
            string attackOverall2 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[4]");
            string defenseOverall1 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[4]");
            string defenseOverall2 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[1]/span") + "  :  " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[4]");

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("Top Attackers (Seasonal)",
                "1: " + attackSeason1 + "\n2: " + attackSeason2, true)
                .WithFooter(footer => footer.Text = "R6 stats powered by Morgan Bot")
                .WithColor(Color.Purple)
                .WithTitle("Top Operators: " + username)
                .WithUrl("https://morgan.games/discord-bot/")
                .WithCurrentTimestamp()
                .WithThumbnailUrl(profilePic)
                .Build();

            embed.AddField("Top Attackers (Overall)",
                "1: " + attackOverall1 + "\n2: " + attackOverall2, true);

            embed.AddField("\u200b", "\u200b", true);

            embed.AddField("Top Defenders (Seasonal)",
                "1: " + defenseSeason1 + "\n2: " + defenseSeason2, true);

            embed.AddField("Top Defenders (Overall)",
                "1: " + defenseOverall1 + "\n2: " + defenseOverall2, true);

            embed.AddField("\u200b", "\u200b", true);

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
