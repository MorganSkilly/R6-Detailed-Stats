using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R6DetailedStats
{
    class Program
    {
        private static bool started = false, stop = false;
        private const string version = "0.0.1";
        private static ConsoleKeyInfo keyInfo;
        private const bool displayDebug = true;

        static void Start()
        {
            OutputString("BUILD", $"VERSION: {version}");
            OutputString("DEBUG", "start tick");
            started = true;

            bool invalid = true;

            while (invalid)
            {
                Clear();
                OutputString("BUILD", "PRESS ENTER TO START");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"INPUT {DateTime.Now.ToString("HH:mm:ss")} | ");
                keyInfo = Console.ReadKey();
                Console.WriteLine();
                ConsoleKey consoleKeyPressed = keyInfo.Key;
                Console.ResetColor();

                switch (consoleKeyPressed)
                {
                    case ConsoleKey.Enter:
                        OutputString("DEBUG", "enter key pressed");
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
            OutputString("DEBUG", "update tick");

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
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                else if (type.Contains("INPUT"))
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"{type} {DateTime.Now.ToString("HH:mm:ss")} | {msg}");
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
