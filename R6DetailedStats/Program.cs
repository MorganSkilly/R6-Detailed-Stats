using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R6DetailedStats
{
    class Program
    {
        private static bool started = false, stop = false, browserLoaded = false;
        private const string version = "0.0.1";
        private static ConsoleKeyInfo keyInfo;
        private const bool displayDebug = false;
        private static DiscordBot R6DiscordBot;
        private static ChromiumWebBrowser browser;
        private const string testurl = "https://r6.tracker.network/profile/pc/AbuseAndDesfuse/operators?seasonal=1";
        private static string jsOutput = "null";

        static void StartBrowser()
        {
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
            var settings = new CefSettings()
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp/Cache")
            };
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            browser = new ChromiumWebBrowser(testurl);
            browser.LoadingStateChanged += BrowserLoadingStateChanged;

            OutputString("DEBUG", "browser initialised");

            while (!browserLoaded)
            {
                OutputString("DEBUG", "loading browser window");
            }
        }

        private static void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                OutputString("DEBUG", $"page {testurl} loaded");
                browserLoaded = true;
            }
            else
            {
                OutputString("DEBUG", $"page {testurl} is loading");
                browserLoaded = false;
            }
        }

        static void Start()
        {
            OutputString("BUILD", $"VERSION: {version}");
            OutputString("DEBUG", "start tick");

            StartBrowser();
            started = true;

            R6DiscordBot = new DiscordBot("https://discordapp.com/api/webhooks/723230554798948474/pRrkub-hThHh0eehV2-IyCwigZadlXzQbdmvLaAcEKOTaaa-rJUr9q3kjD-gTIHleQU7", "R6 Stats Bot", "https://cdn.discordapp.com/avatars/251440518658129923/a_62c5797990f35c8927e542037bef3031.gif");

            bool invalid = true;

            while (invalid)
            {
                Clear();

                OutputString("BUILD", "PRESS ENTER TO START");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{DateTime.Now.ToString("HH:mm:ss")} | ");
                keyInfo = Console.ReadKey();
                Console.WriteLine();
                ConsoleKey consoleKeyPressed = keyInfo.Key;
                Console.ResetColor();

                switch (consoleKeyPressed)
                {
                    case ConsoleKey.Enter:
                        OutputString("DEBUG", "enter key pressed");
                        SendWebhookSimple($"Program started by {Environment.MachineName.ToString()}!");
                        invalid = false;
                        break;
                    default:
                        OutputString("DEBUG", "invalid key pressed");
                        break;
                }
            }
        }
        static void Update()
        {
            OutputString("DEBUG", $"js out: {jsOutput}");
            OutputString("DEBUG", "update tick");

            Clear();
            OutputString("BUILD", "TYPE A PLAYERS NAME THEN PRESS <ENTER> TO SEARCH STATS");
            Console.WriteLine();

                
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{DateTime.Now.ToString("HH:mm:ss")} | ");
            string username = Console.ReadLine();
            Console.ResetColor();

            OutputString("BUILD", $"SEARCHING FOR {username}");
            Console.WriteLine();

            OutputString("BUILD", $"MOST PLAYED ATTACKER THIS SEASON: {SearchPlayerData(/*"abuseanddesfuse", "document.querySelector('#post-55 > div.post-inner > div > h1').textContent"*/)}");
        }

        private static string SearchPlayerData(/*string username, string jspath*/)
        {
            //browser.Load("https://r6.tracker.network/profile/pc/" + username + "/operators?seasonal=1");

            System.IO.File.WriteAllText(@"H:\Desktop\output.txt", browser.GetSourceAsync().Result.ToString());
            SendWebHookEmbed();

            //string script = string.Format(jspath);
            //browser.EvaluateScriptAsync(script).ContinueWith(x =>
            //{
            //    var response = x.Result;

            //    if (response.Success && response.Result != null)
            //    {
            //        OutputString("DEBUG", "js response: " + response.Result.ToString());
            //        return response.Result.ToString();
            //    }
            //    else
            //    {
            //        OutputString("DEBUG", "error!");
            //        return null;
            //    }
            //});

            return null;
        }

        static void Main(string[] args)
        {
            do
            {
                if (!started)
                {
                    Start();
                }
                else
                {
                    Update();
                }
            } while (!stop);

            Environment.Exit(0);
        }

        static string JavaQuery(string script)
        {
            string output = "";

            var scriptTask = browser.EvaluateScriptAsync(script);
            scriptTask.ContinueWith(u =>
            {
                if (u.Result.Success && u.Result.Result != null)
                {
                    var response = u.Result.Result;
                    output = response.ToString();
                }
            });

            return output;
        }

        static void SendWebhookSimple(string messagetext)
        {
            R6DiscordBot.SendDiscordWebHookSimple(messagetext);
            OutputString("DEBUG", $"sent simple webhook: {messagetext}");
        }

        static void SendWebHookEmbed()
        {
            R6DiscordBot.SendDiscordWebHookEmbeded("https://ubisoft-avatars.akamaized.net/28667467-87e8-4aa1-98de-9bdc047cd022/default_256_256.png", "R6 Detailed Stats: AbuseAndDesfuse", "https://morgan.games/", new DiscordEmbedField("top seasonal attackers", "GRIDLOCK, ACE"), new DiscordEmbedField("top seasonal defenders", "MAESTRO, KAID"), new DiscordEmbedField("rank", "Silver II (2,464 MMR)"), new DiscordEmbedField("last match info", "DEFEAT (K/D 0.40)"));
            OutputString("DEBUG", $"sent embedded webhook");
        }

        static void OutputString(string type, string msg)
        {
            if (type.Contains("DEBUG") && !displayDebug)
            {

            }
            else
            {
                if (type.Contains("DEBUG"))
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else if (type.Contains("BUILD"))
                    Console.ForegroundColor = ConsoleColor.White;
                else if (type.Contains("INPUT"))
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} | {msg}");
            }
            Console.ResetColor();
        }

        static void Clear()
        {
            if (!displayDebug)
            {
                Console.Clear();
                OutputString("BUILD", $"VERSION: {version}");
            }
            else
            {
                OutputString("DEBUG", "console.clear called");
            }
        }

    }
}
