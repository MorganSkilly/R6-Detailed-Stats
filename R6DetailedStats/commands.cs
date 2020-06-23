using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using HtmlAgilityPack;

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

        [Command("test")]
        public async Task test(string username = "abuseanddesfuse", string platform = "pc")
        {
            HTMLDataParser parser = new HTMLDataParser();

            if (await parser.UpdateDocAsync("https://r6.tracker.network/profile/" + platform + "/" + username + "/mmr-history"))
                Console.WriteLine("HTML doc updated.");
            else
                Console.WriteLine("Couldn't update HTML doc!");

            string returnedval = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[1]/td[2]/span[1]");

            await ReplyAsync("value: " + returnedval);
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
            commandList = commandList + "!r6 rank" + "\n";
            commandList = commandList + "!r6 history" + "\n";

            string descriptionlist = "";

            descriptionlist = descriptionlist + "displays commands" + "\n";
            descriptionlist = descriptionlist + "displays player's primary operators" + "\n";
            descriptionlist = descriptionlist + "displays player's previous 3 ranks and MMR" + "\n";
            descriptionlist = descriptionlist + "displays player's last 5 game results" + "\n";

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("Commands", commandList, true)
                .WithFooter(footer => footer.Text = "Powered by Morgan Bot")
                .WithColor(Color.Blue)
                .WithTitle("Morgan Bot Help!")
                .WithUrl("https://morgan.games/discord-bot/")
                .WithCurrentTimestamp()
                .Build();

            embed.AddField("Description", descriptionlist, true);

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
            string attackSeason1 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[4]");
            string attackSeason2 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[4]");
            string defenseSeason1 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[4]");
            string defenseSeason2 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[4]");


            if (await parser.UpdateDocAsync("https://r6.tracker.network/profile/" + platform + "/" + username + "/operators"))
                Console.WriteLine("HTML doc updated.");
            else
                Console.WriteLine("Couldn't update HTML doc!");

            string attackOverall1 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[1]/td[4]");
            string attackOverall2 = parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Attackers']/tbody/tr[2]/td[4]");
            string defenseOverall1 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[1]/td[4]");
            string defenseOverall2 = parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[1]/span") + "\nK/D: " + parser.ParseString("//*[@id='operators-Defenders']/tbody/tr[2]/td[4]");

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("THIS SEASON\nattack",
                attackSeason1 + "\n" + attackSeason2, true)
                .WithFooter(footer => footer.Text = "R6 stats powered by Morgan Bot")
                .WithTitle("Top Operators: " + username)
                .WithUrl("https://morgan.games/discord-bot/")
                .WithCurrentTimestamp()
                .WithThumbnailUrl(profilePic)
                .Build();

            embed.AddField("OVERALL\nattack",
                attackOverall1 + "\n" + attackOverall2, true);

            embed.AddField("\u200b", "\u200b", true);

            embed.AddField("defense",
                defenseSeason1 + "\n" + defenseSeason2, true);

            embed.AddField("defense",
                defenseOverall1 + "\n" + defenseOverall2, true);

            embed.AddField("\u200b", "\u200b", true);

            await ReplyAsync(null, false, embed.Build(), null);
        }

        [Command("rank")]
        public async Task rank(string username, string platform = "pc")
        {
            HTMLDataParser parser = new HTMLDataParser();

            if (await parser.UpdateDocAsync("https://r6.tracker.network/profile/" + platform + "/" + username + "/seasons"))
                Console.WriteLine("HTML doc updated.");
            else
                Console.WriteLine("Couldn't update HTML doc!");

            string profilePic = parser.ParseImgUrl("//*[@id='profile']/div[1]/div/div[1]/img");

            string op1name = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[2]/div[1]/h2/text()");
            string op2name = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[1]/h2/text()");
            string op3name = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[4]/div[1]/h2/text()");

            string currentmmr1 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[2]/div[2]/div/div[2]/div/div[11]/div[2]");
            string maxmmr1 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[2]/div[2]/div/div[2]/div/div[12]/div[2]");
            string endrank1 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[2]/div[2]/div/div[2]/div/div[9]/div[2]");
            string maxrank1 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[2]/div[2]/div/div[2]/div/div[10]/div[2]");

            string currentmmr2 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/div/div[2]/div/div[11]/div[2]");
            string maxmmr2 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/div/div[2]/div/div[12]/div[2]");
            string endrank2 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/div/div[2]/div/div[9]/div[2]");
            string maxrank2 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/div/div[2]/div/div[10]/div[2]");

            string currentmmr3 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[4]/div[2]/div/div[2]/div/div[11]/div[2]");
            string maxmmr3 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[4]/div[2]/div/div[2]/div/div[12]/div[2]");
            string endrank3 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[4]/div[2]/div/div[2]/div/div[9]/div[2]");
            string maxrank3 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[4]/div[2]/div/div[2]/div/div[10]/div[2]");

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("OPERATION:\n" + op1name + "\ncurrent rank",
                endrank1 + "\nMMR: " + currentmmr1, true)
                .WithFooter(footer => footer.Text = "R6 stats powered by Morgan Bot")
                .WithTitle("Rank History: " + username)
                .WithUrl("https://morgan.games/discord-bot/")
                .WithCurrentTimestamp()
                .WithThumbnailUrl(profilePic)
                .Build();

            embed.AddField("OPERATION:\n" + op2name + "\nfinal rank",
                endrank2 + "\nMMR: " + currentmmr2, true);

            embed.AddField("OPERATION:\n" + op3name + "\nfinal rank",
                endrank3 + "\nMMR: " + currentmmr3, true);

            embed.AddField("highest rank",
                "Highest: " + maxrank1 + "\nMMR: " + maxmmr1, true);

            embed.AddField("highest rank",
                "Highest: " + maxrank2 + "\nMMR: " + maxmmr2, true);

            embed.AddField("highest rank",
                "Highest: " + maxrank3 + "\nMMR: " + maxmmr3, true);

            await ReplyAsync(null, false, embed.Build(), null);
        }

        [Command("history")]
        public async Task history(string username, string platform = "pc")
        {
            HTMLDataParser parser = new HTMLDataParser();

            if (await parser.UpdateDocAsync("https://r6.tracker.network/profile/" + platform + "/" + username + "/mmr-history"))
                Console.WriteLine("HTML doc updated.");
            else
                Console.WriteLine("Couldn't update HTML doc!");

            string profilePic = parser.ParseImgUrl("//*[@id='profile']/div[1]/div/div[1]/img");

            string mmr1 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[1]/td[5]");
            string mmr2 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[2]/td[5]");
            string mmr3 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[3]/td[5]");
            string mmr4 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[4]/td[5]");
            string mmr5 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[5]/td[5]");

            string result1 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[1]/td[2]/span[1]");
            string result2 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[2]/td[2]/span[1]");
            string result3 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[3]/td[2]/span[1]");
            string result4 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[4]/td[2]/span[1]");
            string result5 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[5]/td[2]/span[1]");

            string kd1 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[1]/td[6]");
            string kd2 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[2]/td[6]");
            string kd3 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[3]/td[6]");
            string kd4 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[4]/td[6]");
            string kd5 = parser.ParseString("//*[@id='profile']/div[2]/div[1]/div[3]/div[2]/table[1]/tbody/tr[5]/td[6]");

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("result",
                result1 + "\n" + result2 + "\n" + result3 + "\n" + result4 + "\n" + result5, true)
                .WithFooter(footer => footer.Text = "R6 stats powered by Morgan Bot")
                .WithTitle("Match History: " + username)
                .WithUrl("https://morgan.games/discord-bot/")
                .WithCurrentTimestamp()
                .WithThumbnailUrl(profilePic)
                .Build();

            embed.AddField("mmr change",
                mmr1 + "\n" + mmr2 + "\n" + mmr3 + "\n" + mmr4 + "\n" + mmr5, true);

            embed.AddField("K/D",
                kd1 + "\n" + kd2 + "\n" + kd3 + "\n" + kd4 + "\n" + kd5, true);

            await ReplyAsync(null, false, embed.Build(), null);
        }
    }
}
