using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Sevenisko.SharpWood
{
    public class OakwoodEvents
    {
        public delegate void Start();
        public static event Start OnStart;

        public delegate void Stop();
        public static event Stop OnStop;

        public delegate void OakConsole(string text);
        public static event OakConsole OnConsoleText;

        public delegate void OnPConnect(OakwoodPlayer player);
        public static event OnPConnect OnPlayerConnect;

        public delegate void OnPHit(OakwoodPlayer player, OakwoodPlayer attacker, float damage);
        public static event OnPHit OnPlayerHit;

        public delegate void OnPVehUse(OakwoodVehicle vehicle, OakwoodPlayer player, bool success, VehicleSeat seat, VehicleEnterState leaveState);
        public static event OnPVehUse OnPlayerVehicleUse;

        public delegate void OnPKeyUp(OakwoodPlayer player, VirtualKey key);
        public static event OnPKeyUp OnPlayerKeyDown;
        public delegate void OnPKeyDown(OakwoodPlayer player, VirtualKey key);
        public static event OnPKeyDown OnPlayerKeyUp;

        public delegate void OnPDeath(OakwoodPlayer player);
        public static event OnPDeath OnPlayerDeath;

        public delegate void OnPDisconnect(OakwoodPlayer player);
        public static event OnPDisconnect OnPlayerDisconnect;

        public delegate void OnVDestroy(OakwoodVehicle vehicle);
        public static event OnVDestroy OnVehicleDestroy;

        public delegate void OnPChat(OakwoodPlayer player, string text);
        public static event OnPChat OnPlayerChat;

        public delegate void OnCBreak(CtrlType type);
        public static event OnCBreak OnConsoleBreak;

        internal static bool start(object[] args)
        {
            OakMisc.Log($"SharpWood {Oakwood.GetVersion()} successfuly started on this server!");
            Console.WriteLine($"[INFO] Registered {OakwoodCommandSystem.GetCommandCount()} commands, {OakwoodCommandSystem.GetEventCount()} events");
            if(OnStart != null)
            {
                OnStart();
            }
            return true;
        }

        internal static bool stop(object[] args)
        {
            if(OnStop != null)
            {
                OnStop();
            }
            return true;
        }

        internal static bool OnConBreak(object[] args)
        {
            CtrlType ctrlType = (CtrlType)int.Parse(args[0].ToString());
            if(OnConsoleBreak != null)
            {
                OnConsoleBreak(ctrlType);
            }
            return true;
        }

        internal static bool OnConnect(object[] args)
        {
            int playerID = int.Parse(args[0].ToString());

            OakwoodPlayer newPlayer = new OakwoodPlayer();

            newPlayer.ID = playerID;

            newPlayer.Name = OakPlayer.GetName(newPlayer);

            Random rand = new Random();

            int index = rand.Next(OakwoodResources.PlayerModels.Length);

            newPlayer.Model = OakwoodResources.PlayerModels[index].Modelname;

            newPlayer.Health = 200.0f;

            if (newPlayer.Name == "|>>>failed<<<|" || newPlayer.Name == "Server")
            {
                OakPlayer.Kick(newPlayer, "Player name cannot be empty!");
            }
            else
            {
                Oakwood.Players.Add(newPlayer);

                OakPlayer.SetModel(newPlayer, newPlayer.Model);

                OakPlayer.Spawn(newPlayer, new OakVec3(-1986.852539f, -5.089742f, 25.776871f), 180.0f);

                if (OnPlayerConnect != null)
                {
                    OnPlayerConnect(newPlayer);
                }
            }
            return true;
        }

        internal static bool OnConsole(object[] args)
        {
            string msg = (string)args[0];

            if(msg.StartsWith("!"))
            {
                OakChat.SendAll($"[CHAT] Server: {msg.Substring(1)}");
                OakMisc.Log($"[CHAT] Server: {msg.Substring(1)}");
            }
            else
            {
                switch (msg)
                {
                    case "shwood":
                        {
                            OakMisc.Log($"This server is using SharpWood {Oakwood.GetVersion()} made by Sevenisko.");
                        }
                        break;
                    case "shwood-throwfatal":
                        {
                            Oakwood.ThrowFatal("User-called");
                        }
                        break;
                }

                if (OnConsoleText != null)
                {
                    OnConsoleText(msg);
                }
            }

            return true;
        }

        internal static bool OnPKill(object[] args)
        {
            int playerID = int.Parse(args[0].ToString());
            OakwoodPlayer player = null;
            foreach (OakwoodPlayer pl in Oakwood.Players)
            {
                if(pl.ID == playerID)
                {
                    player = pl;
                    break;
                }
            }
            if(OnPlayerDeath != null)
            {
                OnPlayerDeath(player);
            }
            return true;
        }

        internal static bool OnPlHit(object[] args)
        {
            int playerID = int.Parse(args[0].ToString());
            int attackerID = int.Parse(args[1].ToString());
            float damage = float.Parse(args[2].ToString());

            OakwoodPlayer player = null;
            OakwoodPlayer attacker = null;

            foreach(OakwoodPlayer pl in Oakwood.Players)
            {
                if(pl.ID == playerID)
                {
                    player = pl;
                }

                if(pl.ID == attackerID)
                {
                    attacker = pl;
                }
            }

            if(OnPlayerHit != null)
            {
                OnPlayerHit(player, attacker, damage);
            }
            return true;
        }

        internal static bool OnPlVehicleUse(object[] args)
        {
            int vehID = int.Parse(args[0].ToString());
            int plID = int.Parse(args[1].ToString());
            bool success = Convert.ToBoolean(int.Parse(args[2].ToString()));
            int sID = int.Parse(args[3].ToString());
            int e = int.Parse(args[4].ToString());

            OakwoodPlayer player = null;
            OakwoodVehicle vehicle = null;

            foreach(OakwoodVehicle v in Oakwood.Vehicles)
            {
                if(v.ID == vehID)
                {
                    vehicle = v;
                    break;
                }
            }

            foreach(OakwoodPlayer p in Oakwood.Players)
            {
                if(p.ID == plID)
                {
                    player = p;
                    break;
                }
            }

            if((VehicleEnterState)e == VehicleEnterState.Enter)
            {
                player.Vehicle = vehicle;
            }
            else
            {
                player.Vehicle = null;
            }

            if(OnPlayerVehicleUse != null)
            {
                OnPlayerVehicleUse(vehicle, player, success, (VehicleSeat)sID, (VehicleEnterState)e);
            }

            return true;
        }

        internal static bool OnPlKey(object[] args)
        {
            int plID = int.Parse(args[0].ToString());
            int vKey = int.Parse(args[1].ToString());
            int isD = int.Parse(args[2].ToString());

            bool isDown = isD != 0;

            OakwoodPlayer player = null;

            foreach (OakwoodPlayer p in Oakwood.Players)
            {
                if (p.ID == plID)
                {
                    player = p;
                    break;
                }
            }

            if(isD == 0)
            {
                if (OnPlayerKeyDown != null)
                {
                    OnPlayerKeyDown(player, (VirtualKey)vKey);
                }
            }
            else
            {
                if (OnPlayerKeyUp != null)
                {
                    OnPlayerKeyUp(player, (VirtualKey)vKey);
                }
            }

            return true;
        }

        internal static bool OnVehDestroy(object[] args)
        {
            int vehID = int.Parse(args[0].ToString());

            OakwoodVehicle vehicle = null;

            foreach(OakwoodVehicle veh in Oakwood.Vehicles)
            {
                if(veh.ID == vehID)
                {
                    vehicle = veh;
                }
            }

            if(OnVehicleDestroy != null)
            {
                OnVehicleDestroy(vehicle);
            }

            return true;
        }

        internal static bool OnDisconnect(object[] args)
        {
            int playerID = int.Parse(args[0].ToString());
            OakwoodPlayer player = null;

            foreach (OakwoodPlayer p in Oakwood.Players)
            {
                if (p.ID == playerID)
                {
                    player = p;
                    
                    break;
                }
            }

            if(OnPlayerDisconnect != null)
            {
                OnPlayerDisconnect(player);
            }

            Oakwood.Players.Remove(player);

            return true;
        }

        internal static bool OnChat(object[] args)
        {
            int playerID = int.Parse(args[0].ToString());
            string msg = (string)args[1];

            OakwoodPlayer player = null;
            foreach (OakwoodPlayer pl in Oakwood.Players)
            {
                if (pl.ID == playerID)
                {
                    player = pl;
                    break;
                }
            }

            if (msg.StartsWith("/"))
            {
                string[] t = msg.Split(' ');
                string cmd = t[0];
                string[] cmdargs = t.Skip(1).ToArray();

                if(cmd.Substring(1) == "shwood")
                {
                    OakHUD.Message(player, $"SharpWood {Oakwood.GetVersion()} made by Sevenisko.", OakColor.White);
                }
                else if (!OakwoodCommandSystem.ExecuteCommand(player, cmd.Substring(1), cmdargs))
                {
                    OakwoodCommandSystem.CallEvent("unknownCommand", new object[] { player, cmd.Substring(1) });
                }
            }
            else
            {
                if(OnPlayerChat != null)
                {
                    OnPlayerChat(player, msg);
                }
            }

            return true;
        }
    }
}