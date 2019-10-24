using System;
using Sevenisko.SharpWood;
using System.Drawing;

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
            OakwoodEvents.OnPlayerDeath += OnPlayerDeath;
            OakwoodEvents.OnPlayerChat += OnPlayerChat;
            OakwoodEvents.OnPlayerKey += OnPlayerKey;

            Oakwood.CreateClient("ipc://oakwood-inbound", "ipc://oakwood-outbound");

            OakwoodCommandSystem.RegisterCommand("test", TestCommand);
            OakwoodCommandSystem.RegisterCommand("players", ShowPlayersCommand);
            OakwoodCommandSystem.RegisterCommand("skin", SetSkin);
            OakwoodCommandSystem.RegisterCommand("spawn", SpawnCommand);
            OakwoodCommandSystem.RegisterCommand("getpos", GetPos);
            OakwoodCommandSystem.RegisterCommand("car", SpawnCar);

            OakwoodCommandSystem.RegisterEvent("unknownCommand", PlUnknownCmd);
        }

        private static void OnPlayerDeath(OakwoodPlayer player)
        {
            if(OakPlayer.IsValid(player))
            {
                OakChat.SendAll($"[INFO] {player} died.");
                OakPlayer.SpawnTempWeapons(player);

                OakPlayer.SetHealth(player, 200.0f);
                OakPlayer.Spawn(player, new OakVec3(-1986.852539f, -5.089742f, 25.776871f));


                OakHUD.Fade(player, OakwoodFade.FadeIn, 500, 0xFFFFFF);
                OakHUD.Fade(player, OakwoodFade.FadeOut, 500, 0xFFFFFF);
            }
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

        static bool SetSkin(OakwoodPlayer player, object[] args)
        {
            if(args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /skin <skinID>");
                return true;
            }

            int skinID = int.Parse(args[0].ToString());

            if(skinID >= 0 && skinID < OakwoodResources.PlayerModels.Length - 1)
            {
                OakPlayer.SetModel(player, OakwoodResources.PlayerModels[skinID].Modelname);
                OakChat.Send(player, "[INFO] Skin successfully changed!");
            }
            else
            {
                OakChat.Send(player, "[WARN] Skin ID you provided is wrong!");
            }
            

            return true;
        }

        static bool GetPos(OakwoodPlayer player, object[] args)
        {
            OakVec3 pos = OakPlayer.GetPosition(player);
            OakChat.Send(player, $"[INFO] Actual position: [{pos.x}, {pos.y}, {pos.z}]");
            return true;
        }

        static bool SpawnCar(OakwoodPlayer player, object[] args)
        {
            if(args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /car <carID>");
                return true;
            }

            int carID = int.Parse(args[0].ToString());

            if (carID >= 0 && carID < OakwoodResources.VehicleModels.Length - 1)
            {
                OakVec3 plPos = OakPlayer.GetPosition(player);

                OakwoodVehicle sCar = OakVehicle.Spawn(OakwoodResources.VehicleModels[carID], plPos, 0f);

                if(sCar != null)
                {
                    OakVehPlayer.Put(sCar, player, VehicleSeat.FrontLeft);

                    OakChat.Send(player, "[INFO] Car successfully spawned!");
                    return true;
                }
                else
                {
                    OakChat.Send(player, "[ERROR] Car spawning failed!");
                    return false;
                }
            }

            return true;
        }

        static bool TestCommand(OakwoodPlayer player, object[] args)
        {
            OakChat.Send(player, $"SharpWood testing command.");
            return true;
        }
        
        static bool SpawnCommand(OakwoodPlayer player, object[] args)
        {
            OakPlayer.SpawnTempWeapons(player);

            OakPlayer.SetHealth(player, 200.0f);
            OakPlayer.Spawn(player, new OakVec3(-1986.852539f, -5.089742f, 25.776871f));


            OakHUD.Fade(player, OakwoodFade.FadeIn, 500, 0xFFFFFF);
            OakHUD.Fade(player, OakwoodFade.FadeOut, 500, 0xFFFFFF);

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

                OakPlayer.SpawnTempWeapons(player);

                OakHUD.Announce(player, "Welcome to SharpWood testing server!", 4.5f);
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
