using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Sevenisko.SharpWood
{
    /// <summary>
    /// Oakwood Event System
    /// </summary>
    public class OakwoodEvents
    {
        public delegate void Start();
        /// <summary>
        /// Triggered when SharpWood is initialized
        /// </summary>
        public static event Start OnStart;

        public delegate void Stop();
        /// <summary>
        /// Triggered when SharpWood is shutting down
        /// </summary>
        public static event Stop OnStop;

        public delegate void OakConsole(string text);
        /// <summary>
        /// Triggered when a console input is received
        /// </summary>
        public static event OakConsole OnConsoleText;

        public delegate void OnPConnect(OakwoodPlayer player);
        /// <summary>
        /// Triggered when player joins the server
        /// </summary>
        public static event OnPConnect OnPlayerConnect;

        public delegate void OnPHit(OakwoodPlayer player, OakwoodPlayer attacker, float damage);
        /// <summary>
        /// Triggered when player gets damaged
        /// </summary>
        public static event OnPHit OnPlayerHit;

        public delegate void OnPVehUse(OakwoodVehicle vehicle, OakwoodPlayer player, bool success, VehicleSeat seat, VehicleEnterState leaveState);
        /// <summary>
        /// Triggered when player enters/leaves vehicle
        /// </summary>
        public static event OnPVehUse OnPlayerVehicleUse;

        public delegate void OnPKeyUp(OakwoodPlayer player, VirtualKey key);
        /// <summary>
        /// Triggered when player presses the key down
        /// </summary>
        public static event OnPKeyUp OnPlayerKeyDown;
        public delegate void OnPKeyDown(OakwoodPlayer player, VirtualKey key);
        /// <summary>
        /// Triggered when player pulls the key up
        /// </summary>
        public static event OnPKeyDown OnPlayerKeyUp;

        public delegate void OnPDeath(OakwoodPlayer player, OakwoodPlayer killer);
        /// <summary>
        /// Triggered when player dies
        /// </summary>
        public static event OnPDeath OnPlayerDeath;

        public delegate void OnPDisconnect(OakwoodPlayer player);
        /// <summary>
        /// Triggered when player lefts the server
        /// </summary>
        public static event OnPDisconnect OnPlayerDisconnect;

        public delegate void OnVDestroy(OakwoodVehicle vehicle);
        /// <summary>
        /// Triggered when vehicle is destroyed
        /// </summary>
        public static event OnVDestroy OnVehicleDestroy;

        public delegate void OnPChat(OakwoodPlayer player, string text);
        /// <summary>
        /// Triggered when player sends a chat message
        /// </summary>
        public static event OnPChat OnPlayerChat;

        public delegate void OnCBreak(CtrlType type);
        /// <summary>
        /// Triggered when a console break event is triggered
        /// </summary>
        public static event OnCBreak OnConsoleBreak;

        public delegate void OnOLog(DateTime time, string source, string message);
        /// <summary>
        /// Triggered when a Log message is sent
        /// </summary>
        public static event OnOLog OnLog;

        internal static void log(DateTime time, string source, string message, bool intoOakConsole = false)
        {
            if(intoOakConsole)
            {
                OakMisc.Log($"[{time.ToString("HH:mm:ss")} - {source}] {message}");
            }
            if (OnLog != null) OnLog(time, source, message);
        }

        internal static void start(object[] args)
        {
            OakMisc.Log($"SharpWood {Oakwood.GetVersion()} successfuly started on this server!");
            Console.WriteLine($"[INFO] Registered {OakwoodCommandSystem.GetCommandCount()} commands, {OakwoodCommandSystem.GetEventCount()} events");
            if(OnStart != null) OnStart();
        }

        internal static void stop(object[] args)
        {
            if(OnStop != null) OnStop();
        }

        internal static void OnConBreak(object[] args)
        {
            CtrlType ctrlType = (CtrlType)int.Parse(args[0].ToString());
            if(OnConsoleBreak != null) OnConsoleBreak(ctrlType);
        }

        internal static void OnConnect(object[] args)
        {
            int playerID = int.Parse(args[0].ToString());

            OakwoodPlayer newPlayer = new OakwoodPlayer();

            newPlayer.ID = playerID;

            newPlayer.Name = newPlayer.GetName();

            Random rand = new Random();

            int index = rand.Next(OakwoodResources.PlayerModels.Length);

            newPlayer.Model = OakwoodResources.PlayerModels[index].Modelname;

            if (newPlayer.Name == "|>>>failed<<<|" || newPlayer.Name == "Server")
            {
                newPlayer.Kick("Player name cannot be empty!");
            }
            else
            {
                Oakwood.Players.Add(newPlayer);

                newPlayer.SetModel(newPlayer.Model);

                newPlayer.Spawn(new OakVec3(-1986.852539f, -5.089742f, 25.776871f), 180.0f);

                if (OnPlayerConnect != null) OnPlayerConnect(newPlayer);
            }
        }

        internal static void OnConsole(object[] args)
        {
            string msg = (string)args[0];

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

            if (OnConsoleText != null) OnConsoleText(msg);
        }

        internal static void OnPKill(object[] args)
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
            if(OnPlayerDeath != null) OnPlayerDeath(player, player.Killer == null ? player : player.Killer);
            player.Killer = null;
        }

        internal static void OnPlHit(object[] args)
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

            if (player.Health == 0) player.Killer = attacker;

            if(OnPlayerHit != null) OnPlayerHit(player, attacker, damage);
        }

        internal static void OnPlVehicleUse(object[] args)
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

            if(OnPlayerVehicleUse != null) OnPlayerVehicleUse(vehicle, player, success, (VehicleSeat)sID, (VehicleEnterState)e);
        }

        internal static void OnPlKey(object[] args)
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
                if (OnPlayerKeyDown != null) OnPlayerKeyDown(player, (VirtualKey)vKey);
            }
            else
            {
                if (OnPlayerKeyUp != null) OnPlayerKeyUp(player, (VirtualKey)vKey);
            }
        }

        internal static void OnVehDestroy(object[] args)
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

            if(OnVehicleDestroy != null) OnVehicleDestroy(vehicle);
        }

        internal static void OnDisconnect(object[] args)
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

            if(OnPlayerDisconnect != null) OnPlayerDisconnect(player);

            Oakwood.Players.Remove(player);
        }

        internal static void OnChat(object[] args)
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

                if (cmd.Substring(1) == "shwood")
                {
                    player.HUD.Message($"SharpWood {Oakwood.GetVersion()} made by Sevenisko.", OakColor.White);
                    player.HUD.Message($"Command System customized by NanobotZ.", OakColor.White);
                }
                else if (cmd.Substring(1) == "help")
                {
                    player.SendMessage($"Command help:");
                    for (int i = 0; i < OakwoodCommandSystem.cmdDescriptions.Count; i++)
                    {
                        player.SendMessage($" > {OakwoodCommandSystem.cmdDescriptions[i]}");
                    }
                }
                else if (!OakwoodCommandSystem.ExecuteCommand(player, cmd.Substring(1), cmdargs))
                {
                    OakwoodCommandSystem.CallEvent("unknownCommand", new object[] { player, cmd.Substring(1) });
                }
            }
            else
            {
                if(OnPlayerChat != null) OnPlayerChat(player, msg);
            }
        }
    }
}