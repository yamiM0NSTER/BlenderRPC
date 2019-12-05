using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BlenderRPC
{
    class DiscordRPC
    {
        static public DiscordRpcClient client;
        static string appID = "652080164305502208";

        private static DateTime startTime = DateTime.UtcNow;

        private static RichPresence richPresence;

        static public void Init()
        {
            
	        // Create a discord client
            client = new DiscordRpcClient(appID);

            // Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            // Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };
            
            // Connect to the RPC
            client.Initialize();

            // Create RichPresence object
            richPresence = new RichPresence()
            {
                Details = "Working On A Project",
                State = Path.GetFileName(ProcessHandler.PullProjectName()),
                Assets = new Assets()
                {
                    LargeImageKey = "blender_icon_1024x1024",
                    LargeImageText = "Blender " + ProcessHandler.PullBlenderVersion()
                },
                Timestamps = new Timestamps
                {
                    Start = startTime
                }
            };

            // Set the rich presence
            client.SetPresence(richPresence);
        }

        static public void Run()
        {
            while(true)
            {
                if (!ProcessHandler.isBlenderRunning())
                    break;

                UpdateStatus();


                Thread.Sleep(1000);
            }

            Console.WriteLine("Blender not running anymore.");
            client.Dispose();
        }

        private static void UpdateStatus()
        {
            // Update current project
            richPresence.State = Path.GetFileName(ProcessHandler.PullProjectName());

            client.SetPresence(richPresence);
        }
    }
}
