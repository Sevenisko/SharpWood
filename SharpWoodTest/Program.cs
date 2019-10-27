using System;
using Sevenisko.SharpWood;
using System.IO;
using System.Linq;
using System.Timers;
using System.Collections.Generic;

namespace Sevenisko.SharpWoodTestGamemode
{
    class SWTestGamemode
    {
        public static void Repeat(int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        #region Initialization entry
        static void Main(string[] args)
        {
            OakwoodEvents.OnStart += OnGMStart;
            OakwoodEvents.OnStop += OnGMStop;
            OakwoodEvents.OnPlayerConnect += OnPlayerConnect;
            OakwoodEvents.OnPlayerDisconnect += OnPlayerDisconnect;
            OakwoodEvents.OnPlayerDeath += OnPlayerDeath;
            OakwoodEvents.OnPlayerChat += OnPlayerChat;
            OakwoodEvents.OnPlayerKeyDown += OnPlayerKey;
            OakwoodEvents.OnConsoleBreak += OnConsoleBreak;

            Oakwood.CreateClient("ipc://oakwood-inbound", "ipc://oakwood-outbound", true);

            OakwoodCommandSystem.RegisterCommand("test", TestCommand);
            OakwoodCommandSystem.RegisterCommand("players", ShowPlayersCommand);
            OakwoodCommandSystem.RegisterCommand("skin", SetSkin);
            OakwoodCommandSystem.RegisterCommand("spawn", SpawnCommand);
            OakwoodCommandSystem.RegisterCommand("getpos", GetPos);
            OakwoodCommandSystem.RegisterCommand("getdir", GetDir);
            OakwoodCommandSystem.RegisterCommand("car", SpawnCar);
            OakwoodCommandSystem.RegisterCommand("saveloc", SaveLoc);
            OakwoodCommandSystem.RegisterCommand("loadloc", LoadLoc);
            OakwoodCommandSystem.RegisterCommand("clearchat", ClearChatCommand);
            OakwoodCommandSystem.RegisterCommand("tpa", TpaCommand);
            OakwoodCommandSystem.RegisterCommand("tpaccept", TpacceptCommand);
            OakwoodCommandSystem.RegisterCommand("pm", PmCommand);
            OakwoodCommandSystem.RegisterCommand("delcar", DelCarCommand);
            OakwoodCommandSystem.RegisterCommand("repair", RepairCommand);
            OakwoodCommandSystem.RegisterCommand("heal", HealCommand);
            OakwoodCommandSystem.RegisterCommand("warp", Warp);
            OakwoodCommandSystem.RegisterCommand("createwarp", CreateWarp);
            OakwoodCommandSystem.RegisterCommand("updatewarp", EditWarp);
            OakwoodCommandSystem.RegisterCommand("deletewarp", DeleteWarp);

            OakwoodCommandSystem.RegisterEvent("unknownCommand", PlUnknownCmd);
        }
        #endregion

        #region Events
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

        private static void OnConsoleBreak(CtrlType ctrlType)
        {
            Console.WriteLine("Exiting gamemode...");
            Oakwood.KillClient();
            Console.WriteLine("Have a good day and goodbye. :)");
            System.Threading.Thread.Sleep(1500);
            Environment.Exit(0);
        }

        static void OnPlayerConnect(OakwoodPlayer player)
        {
            if (OakPlayer.IsValid(player))
            {
                foreach (OakwoodPlayer p in OakPlayer.GetList())
                {
                    OakHUD.Message(p, $"{player.Name} joined the game.", OakColor.White);
                }

                OakPlayer.SpawnTempWeapons(player);

                OakPlayer.Spawn(player, new OakVec3(-2136.182f, -5.768807f, -521.3138f), 90.0f);

                player.TpaID = -1;

                OakHUD.Announce(player, "Welcome to SharpWood testing server!", 4.5f);
            }
        }

        private static void OnPlayerDisconnect(OakwoodPlayer player)
        {
            if (OakPlayer.IsValid(player))
            {
                foreach(OakwoodPlayer p in OakPlayer.GetList())
                {
                    OakHUD.Message(p, $"{player.Name} left the game.", OakColor.White);
                }
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

        private static void OnPlayerDeath(OakwoodPlayer player)
        {
            if(OakPlayer.IsValid(player))
            {
                foreach(OakwoodPlayer p in OakPlayer.GetList())
                {
                    OakHUD.Message(p, $"{player.Name} died.", OakColor.White);
                }

                OakHUD.Announce(player, "Wasted", 4.85f);
                
                Timer spawnTimer = new Timer();
                spawnTimer.Interval = 5000;
                spawnTimer.AutoReset = false;
                spawnTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    Timer waitTimer = new Timer();
                    OakHUD.Fade(player, OakwoodFade.FadeIn, 2500, OakColor.Black);
                    waitTimer.Interval = 2500;
                    waitTimer.AutoReset = false;
                    waitTimer.Elapsed += (object snd, ElapsedEventArgs ea) =>
                    {
                        OakPlayer.SpawnTempWeapons(player);
                        OakPlayer.SetHealth(player, 200.0f);
                        OakPlayer.Spawn(player, new OakVec3(-759.3801f, 13.24883f, 761.6967f), 180.0f);
                        OakHUD.Fade(player, OakwoodFade.FadeOut, 2500, OakColor.Black);
                        /*Timer protTimer = new Timer();
                        System.Threading.Thread godThread = new System.Threading.Thread(() => Repeat(6500, () => OakPlayer.SetHealth(player, 200.0f)));
                        protTimer.Interval = 6500;
                        protTimer.AutoReset = false;
                        protTimer.Elapsed += (object a, ElapsedEventArgs b) =>
                        {
                            OakHUD.Message(player, "Spawn protection is now disabled.", OakColor.White);
                            godThread.Abort();
                        };
                        
                        godThread.Start();
                        protTimer.Start();*/
                    };
                    waitTimer.Start();
                };

                spawnTimer.Start();
            }
        }

        private static bool PlUnknownCmd(object[] args)
        {
            OakwoodPlayer player = (OakwoodPlayer)args[0];
            string cmd = args[1].ToString();

            OakChat.Send(player, $"[CMD] Command '{cmd}' was not found.");

            return true;
        }

        private static void OnPlayerKey(OakwoodPlayer player, VirtualKey key)
        {
            // Do nothing
        }

        #endregion

        #region Commands
        static bool Warp(OakwoodPlayer player, object[] args)
        {
            if (args.Length == 0)
            {
                string List = "";

                foreach(string f in Directory.GetFiles(Environment.CurrentDirectory + @"\Warps"))
                {
                    List += $"{Path.GetFileNameWithoutExtension(f)}, ";
                }

                OakChat.Send(player, $"[WARPS ({Directory.GetFiles(Environment.CurrentDirectory + @"\Warps").Length})] {List}");
                return true;
            }

            string locName = args[0].ToString();

            string file = Environment.CurrentDirectory + @"\Warps\" + $"{locName}.txt";

            if (File.Exists(file))
            {
                OakHUD.Message(player, $"Warping you to '{locName}'...", 0xFFFFFF);
                string[] pos = File.ReadAllText(file).Split(';');
                OakVec3 newPos = new OakVec3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
                OakVec3 newDir = new OakVec3(float.Parse(pos[3]), float.Parse(pos[4]), float.Parse(pos[5]));
                OakPlayer.SetPosition(player, newPos);
                OakPlayer.SetDirection(player, newDir);
            }
            else
            {
                OakHUD.Message(player, $"Warp '{locName}' doesn't exist!", 0xFF0000);
            }

            return true;
        }

        static bool DeleteWarp(OakwoodPlayer player, object[] args)
        {
            if (args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /deletewarp <locName>");
                return true;
            }

            string locName = args[0].ToString();

            string file = Environment.CurrentDirectory + @"\Warps\" + $"{locName}.txt";

            if(File.Exists(file))
            {
                File.Delete(file);
                OakHUD.Message(player, $"Warp '{locName}' deleted.", OakColor.White);
            }
            else
            {
                OakHUD.Message(player, $"Warp '{locName}' doesn't exist!", OakColor.Red);
            }

            return true;
        }

        static bool EditWarp(OakwoodPlayer player, object[] args)
        {
            if (args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /updatewarp <locName>");
                return true;
            }

            string locName = args[0].ToString();

            string file = Environment.CurrentDirectory + @"\Warps\" + $"{locName}.txt";

            if (!Directory.Exists(Path.GetDirectoryName(file)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }

            if(File.Exists(file))
            {
                OakVec3 playerPos = OakPlayer.GetPosition(player);
                OakVec3 playerDir = OakPlayer.GetDirection(player);

                if (playerPos.x != 0 && playerPos.y != 0 && playerPos.z != 0 && playerDir.x != 0 && playerDir.z != 0)
                {
                    File.WriteAllText(file, $"{playerPos.x};{playerPos.y};{playerPos.z};{playerDir.x};{playerDir.y};{playerDir.z}");

                    OakHUD.Message(player, $"Warp '{locName}' updated!", 0xFFFFFF);
                }
                else
                {
                    OakHUD.Message(player, "Cannot create warp!", 0xFF0000);
                }
            }
            else
            {
                OakHUD.Message(player, $"Warp '{locName}' doesn't exist!", OakColor.Red);
            }

            return true;
        }

        static bool CreateWarp(OakwoodPlayer player, object[] args)
        {
            if (args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /createwarp <locName>");
                return true;
            }

            string locName = args[0].ToString();

            string file = Environment.CurrentDirectory + @"\Warps\" + $"{locName}.txt";

            if (!Directory.Exists(Path.GetDirectoryName(file)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }

            if (!File.Exists(file))
            {
                OakVec3 playerPos = OakPlayer.GetPosition(player);
                OakVec3 playerDir = OakPlayer.GetDirection(player);

                if (playerPos.x != 0 && playerPos.y != 0 && playerPos.z != 0 && playerDir.x != 0 && playerDir.z != 0)
                {
                    File.WriteAllText(file, $"{playerPos.x};{playerPos.y};{playerPos.z};{playerDir.x};{playerDir.y};{playerDir.z}");

                    OakHUD.Message(player, $"Warp '{locName}' created!", 0xFFFFFF);
                }
                else
                {
                    OakHUD.Message(player, "Cannot create warp!", 0xFF0000);
                }
            }
            else
            {
                OakHUD.Message(player, $"Warp '{locName}' already exists!", OakColor.Red);
            }

            return true;
        }

        static bool ClearChatCommand(OakwoodPlayer player, object[] args)
        {
            for(int x = 0; x < 24; x++)
            {
                OakChat.Send(player, "\n");
            }

            OakHUD.Message(player, "Chat cleared.", 0xFFFFFF);

            return true;
        }

        static bool DelCarCommand(OakwoodPlayer player, object[] args)
        {
            OakwoodVehicle car = OakVehPlayer.Inside(player);

            if (car != null)
            {
                OakVehPlayer.Remove(car, player);
                OakVehicle.Despawn(car);
                OakHUD.Message(player, "Vehicle successfully removed.", 0xFFFFFF);
            }
            else
            {
                OakHUD.Message(player, "You're not in any vehicle!", 0xFF0000);
            }
            return true;
        }

        static bool HealCommand(OakwoodPlayer player, object[] args)
        {
            OakPlayer.SetHealth(player, 200.0f);
            return true;
        }

        static bool RepairCommand(OakwoodPlayer player, object[] args)
        {
            OakwoodVehicle car = OakVehPlayer.Inside(player);

            if (car != null)
            {
                OakVehicle.Repair(car);
                OakHUD.Message(player, "Vehicle successfully repaired.", 0xFFFFFF);
            }
            else
            {
                OakHUD.Message(player, "You're not in any vehicle!", 0xFF0000);
            }
            return true;
        }

        static bool PmCommand(OakwoodPlayer player, object[] args)
        {
            if (args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /pm <playerName> <message>");
                return true;
            }

            string plName = args[0].ToString();

            object[] msg = args.Skip(1).ToArray();

            string message = "";

            foreach(object m in msg)
            {
                if(message == "")
                {
                    message = $"{m.ToString()} ";
                }
                else
                {
                    message += $"{m.ToString()} ";
                }
            }

            if(plName == player.Name)
            {
                OakChat.Send(player, "[WARN] You can't just send PM to yourself!");
                return true;
            }

            foreach(OakwoodPlayer p in OakPlayer.GetList())
            {
                if(plName == p.Name)
                {
                    OakChat.Send(player, $"[Me -> {p.Name}] {message}");
                    OakChat.Send(p, $"[{player.Name} -> Me] {message}");
                    return true;
                }
            }

            OakChat.Send(player, $"[WARN] Player '{plName}' was not found.");

            return true;
        }

        static bool TpaCommand(OakwoodPlayer player, object[] args)
        {
            if(args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /tpa <playerName>");
                return true;
            }

            string plName = args[0].ToString();

            if(plName == player.Name)
            {
                OakChat.Send(player, "[WARN] You can't just teleport to yourself!");
                return true;
            }

            foreach(OakwoodPlayer p in OakPlayer.GetList())
            {
                if(plName == p.Name)
                {
                    p.TpaID = player.ID;
					
					OakHUD.Message(player, $"Sending teleport request to {p.Name}...", 0xFFFFFF);

                    OakChat.Send(p, $"[TPA] Player {player.Name} wants to teleport to you:");
                    OakChat.Send(p, $"  > Write '/tpaccept' to accept the request.");
                    OakChat.Send(p, $"  > Or just ignore it, if you want to reject the request.");

                    return true;
                }
            }

            OakChat.Send(player, $"[WARN] Player '{plName}' was not found.");
            return true;
        }

        static bool TpacceptCommand(OakwoodPlayer player, object[] args)
        {
            if(player.TpaID != -1)
            {
                foreach(OakwoodPlayer p in OakPlayer.GetList())
                {
                    if(p.ID == player.TpaID)
                    {
                        OakPlayer.SetPosition(p, OakPlayer.GetPosition(player));
                        OakHUD.Message(p, "Your teleport request was accepted.", 0xFFFFFF);
                        player.TpaID = -1;
                        p.TpaID = -1;
                        return true;
                    }
                }
            }

            OakChat.Send(player, "[TPA] There's no teleport request.");

            return true;
        }

        static bool ShowPlayersCommand(OakwoodPlayer player, object[] args)
        {
            OakChat.Send(player, "[INFO] Player list:");
            foreach(OakwoodPlayer pl in OakPlayer.GetList())
            {
                OakChat.Send(player, $" > {pl.Name}#{pl.ID}");
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

            OakwoodVehicle car = OakVehPlayer.Inside(player);

            if (car == null)
            {
                if (skinID >= 0 && skinID < OakwoodResources.PlayerModels.Length - 1)
                {
                    OakPlayer.SetModel(player, OakwoodResources.PlayerModels[skinID].Modelname);
                    OakHUD.Message(player, "Skin successfully changed!", 0xFFFFFF);
                }
                else
                {
                    OakHUD.Message(player, "Skin ID you provided is wrong!", 0xFCB603);
                }
            }
            else
            {
                OakHUD.Message(player, "You can't be inside a vehicle!", 0xFF0000);
            }

            return true;
        }

        static bool GetPos(OakwoodPlayer player, object[] args)
        {
            OakVec3 pos = OakPlayer.GetPosition(player);
            OakHUD.Message(player, $"Actual position: [{pos.x}; {pos.y}; {pos.z}]", 0xFFFFFF);
            return true;
        }
        
        static bool GetDir(OakwoodPlayer player, object[] args)
        {
            OakVec3 dir = OakPlayer.GetDirection(player);
            OakHUD.Message(player, $"Actual direction: [{dir.x * 360.0f}; {dir.y * 360.0f}; {dir.z * 360.0f}]", 0xFFFFFF);
            return true;
        }

        static bool LoadLoc(OakwoodPlayer player, object[] args)
        {
            if (args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /loadloc <locName>");
                return true;
            }

            string locName = args[0].ToString();

            string file = Environment.CurrentDirectory + @"\savedLocations\" + $"{locName}.txt";

            if(File.Exists(file))
            {
                string[] pos = File.ReadAllText(file).Split(';');
                OakVec3 newPos = new OakVec3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
                OakVec3 newDir = new OakVec3(float.Parse(pos[3]), float.Parse(pos[4]), float.Parse(pos[5]));
                OakPlayer.SetPosition(player, newPos);
                OakPlayer.SetDirection(player, newDir);
                OakHUD.Message(player, $"Teleported to '{locName}'!", 0xFFFFFF);
            }
            else
            {
                OakHUD.Message(player, $"Location '{locName}' doesn't exist!", 0xFF0000);
            }

            return true;
        }

        static bool SaveLoc(OakwoodPlayer player, object[] args)
        {
            if(args.Length == 0)
            {
                OakChat.Send(player, "[USAGE] /saveloc <locName>");
                return true;
            }

            string locName = args[0].ToString();

            string file = Environment.CurrentDirectory + @"\savedLocations\" + $"{locName}.txt";

            if(!Directory.Exists(Path.GetDirectoryName(file)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }

            OakVec3 playerPos = OakPlayer.GetPosition(player);
            OakVec3 playerDir = OakPlayer.GetDirection(player);

            if(playerPos.x != 0 && playerPos.y != 0 && playerPos.z != 0 && playerDir.x != 0 && playerDir.z != 0)
            {
                File.WriteAllText(file, $"{playerPos.x};{playerPos.y};{playerPos.z};{playerDir.x};{playerDir.y};{playerDir.z}");

                OakHUD.Message(player, $"Location '{locName}' saved!", 0xFFFFFF);
            }
            else
            {
                OakHUD.Message(player, "Cannot save location!", 0xFF0000);
            }
            
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

                OakVec3 plDir = OakPlayer.GetDirection(player);

                OakwoodVehicle sCar = OakVehicle.Spawn(OakwoodResources.VehicleModels[carID], plPos, plDir.x / 360.0f);

                if(sCar != null)
                {
                    OakVehPlayer.Put(sCar, player, VehicleSeat.FrontLeft);

                    OakHUD.Message(player, "Car successfully spawned!", 0xFFFFFF);
                    return true;
                }
                else
                {
                    OakHUD.Message(player, "Cannot spawn car!", 0xFF0000);
                    return true;
                }
            }
            else
            {
                OakHUD.Message(player, "Car ID you provided is wrong!", 0xFCB603);
                return true;
            }
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
            OakPlayer.Spawn(player, new OakVec3(-1986.852539f, -5.089742f, 25.776871f), 180.0f);

            OakHUD.Fade(player, OakwoodFade.FadeIn, 500, 0xFFFFFF);
            OakHUD.Fade(player, OakwoodFade.FadeOut, 500, 0xFFFFFF);

            return true;
        }

        #endregion
    }
}
