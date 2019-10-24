using System;
using Sevenisko.SharpWood;

namespace SharpWoodTest
{
    class Program
    {
        static void Main(string[] args)
        {
            OakwoodEvents.OnStart += OnGMStart;
            OakwoodEvents.OnStop += OnGMStop;
            OakwoodEvents.OnPlayerConnect += OnPlayerConnect;
            OakwoodEvents.OnPlayerDisconnect += OnPlayerDisconnect;
            OakwoodEvents.OnPlayerChat += OnPlayerChat;
            OakwoodEvents.OnPlayerKey += OnPlayerKey;

            Oakwood.CreateClient("ipc://oakwood-inbound", "ipc://oakwood-outbound");

            OakwoodCommandSystem.RegisterCommand("test", TestCommand);
            OakwoodCommandSystem.RegisterCommand("players", ShowPlayersCommand);

            OakwoodCommandSystem.RegisterEvent("unknownCommand", PlUnknownCmd);
        }

        private static bool PlUnknownCmd(object[] args)
        {
            OakwoodPlayer player = (OakwoodPlayer)args[0];
            string cmd = args[1].ToString();

            OakChat.Send(player, $"[CMD] Command '{cmd}' was not found.");

            return true;
        }

        private static void OnPlayerKey(OakwoodPlayer player, VirtualKey key, bool isDown)
        {
            // Do nothing
        }

        static bool ShowPlayersCommand(OakwoodPlayer player, object[] args)
        {
            OakChat.Send(player, "Player list:");
            foreach(OakwoodPlayer pl in OakPlayer.GetList())
            {
                OakChat.Send(player, $"{pl.Name}#{pl.ID}");
            }

            return true;
        }

        static bool TestCommand(OakwoodPlayer player, object[] args)
        {
            OakChat.Send(player, $"SharpWood testing command.");
            return true;
        }

        static void OnGMStart()
        {
            Console.WriteLine("Gamemode has successfully started.");
            OakMisc.Log("Testing gamemode has been started.");
        }

        static void OnGMStop()
        {
            OakMisc.Log("Gamemode has been stopped.");
            Console.WriteLine("Gamemode has been stopped.");
        }

        static void OnPlayerConnect(OakwoodPlayer player)
        {
            if(OakPlayer.IsValid(player))
            {
                OakChat.SendAll($"[INFO] {player.Name} joined the game.");
            }
        }

        private static void OnPlayerDisconnect(OakwoodPlayer player)
        {
            if(OakPlayer.IsValid(player))
            {
                OakChat.SendAll($"[INFO] {player.Name} left the game.");
            }
        }

        private static void OnPlayerChat(OakwoodPlayer player, string message)
        {
            if (OakPlayer.IsValid(player))
            {
                OakChat.SendAll($"[CHAT] {player.Name}: {message}");
                OakMisc.Log($"[CHAT] {player.Name}: {message}");
            }
        }
    }
}
