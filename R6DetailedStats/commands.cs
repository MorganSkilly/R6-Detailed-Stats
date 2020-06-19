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
    public class commands : ModuleBase<SocketCommandContext>
    {   
        [Command("hello")]
        public async Task hello()
        {
            await ReplyAsync("YEET");
        }

        [Command("cunt")]
        public async Task cunt()
        {
            await ReplyAsync("Did you mean \"Corey Swankie\"?");
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
