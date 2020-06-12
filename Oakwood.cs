using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Reflection;
using System.Diagnostics;
using CS;
using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;

namespace Sevenisko.SharpWood
{
    /// <summary>
    /// Player class
    /// </summary>
    public class OakwoodPlayer
    {
        /// <summary>
        /// Player name
        /// </summary>
        public string Name;
        /// <summary>
        /// Player ID
        /// </summary>
        public int ID;
        /// <summary>
        /// Current model name used by Player
        /// </summary>
        public string Model;
        /// <summary>
        /// Stored player data
        /// </summary>
        public object PlayerData;
        /// <summary>
        /// Current player heading
        /// </summary>
        public float Heading;
        /// <summary>
        /// Vehicle, where player is sitting
        /// </summary>
        public OakwoodVehicle Vehicle;

        /// <summary>
        /// Gives player weapons on next spawn
        /// </summary>
        /// <returns>True if the function is successful</returns>
        public bool SpawnTempWeapons()
        {
            object[] response = Oakwood.CallFunction("oak_temp_weapons_spawn", new object[] { ID });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        internal string GetName()
        {
            object[] ret = Oakwood.CallFunction("oak_player_name_get", new object[] { ID });
            int retCode = int.Parse(ret[0].ToString());
            int nInt = -5;
            string plName = "|>>>NoName<<<|";

            if (ret[1] != null)
            {
                plName = ret[1].ToString();
            }

            if (int.TryParse(plName, out nInt))
            {
                plName = "|>>>NoName<<<|";
            }

            if (retCode != 0 || plName == "|>>>NoName<<<|" || plName == "Server")
            {
                return "|>>>failed<<<|";
            }
            else
            {
                return plName;
            }
        }

        /// <summary>
        /// Spawns a player
        /// </summary>
        /// <param name="pos">Spawn position</param>
        /// <param name="angle">Spawn angle</param>
        /// <returns>True if the function is successful</returns>
        public bool Spawn(OakVec3 pos, float angle)
        {
            object[] response = Oakwood.CallFunction("oak_player_spawn", new object[] { ID, new float[] { pos.x, pos.y, pos.z }, angle });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// De-Spawns a player
        /// </summary>
        /// <returns>True if the function is successful</returns>
        public bool Despawn()
        {
            object[] response = Oakwood.CallFunction("oak_player_despawn", new object[] { ID });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if player is valid
        /// </summary>
        /// <returns>True if the player is valid</returns>
        public bool IsValid()
        {
            foreach (OakwoodPlayer pl in Oakwood.Players)
            {
                if (pl == this)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Kicks a player from server
        /// </summary>
        /// <param name="reason">Kick reason</param>
        /// <returns>True if the function is successful</returns>
        public bool Kick(string reason)
        {
            object[] response = Oakwood.CallFunction("oak_player_kick", new object[] { ID, reason + "\0", reason.Length + 1 });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Kills a player
        /// </summary>
        /// <returns>True if the function is successful</returns>
        public bool Kill()
        {
            object[] response = Oakwood.CallFunction("oak_player_kill", new object[] { ID });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Plays animation on player
        /// </summary>
        /// <param name="animName">Animation file name</param>
        /// <returns>True if the function is successful</returns>
        public bool PlayAnim(string animName)
        {
            object[] response = Oakwood.CallFunction("oak_player_playanim", new object[] { ID, animName + "\0", animName.Length });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets player model
        /// </summary>
        /// <param name="modelName">Model name</param>
        /// <returns>True if the function is successful</returns>
        public bool SetModel(string modelName)
        {
            object[] response = Oakwood.CallFunction("oak_player_model_set", new object[] { ID, modelName + "\0", modelName.Length });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets player health
        /// </summary>
        /// <param name="health">Health value</param>
        /// <returns>True if the function is successful</returns>
        public bool SetHealth(float health)
        {
            object[] response = Oakwood.CallFunction("oak_player_health_set", new object[] { ID, health });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets player position
        /// </summary>
        /// <param name="position">New position</param>
        /// <returns>True if the function is successful</returns>
        public bool SetPosition(OakVec3 position)
        {
            object[] response = Oakwood.CallFunction("oak_player_position_set", new object[] { ID, new float[] { position.x, position.y, position.z } });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets player direction
        /// </summary>
        /// <param name="direction">New direction</param>
        /// <returns>True if the function is successful</returns>
        public bool SetDirection(OakVec3 direction)
        {
            object[] response = Oakwood.CallFunction("oak_player_direction_set", new object[] { ID, new float[] { direction.x, direction.y, direction.z } });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets player heading
        /// </summary>
        /// <param name="angle">Heading angle</param>
        /// <returns>True if the function is successful</returns>
        public bool SetHeading(float angle)
        {
            object[] response = Oakwood.CallFunction("oak_player_heading_set", new object[] { ID, angle });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets player's current model
        /// </summary>
        /// <returns>Model name</returns>
        public string GetModel()
        {
            return Oakwood.CallFunction("oak_player_model_get", new object[] { ID })[1].ToString();
        }

        /// <summary>
        /// Gets player's health
        /// </summary>
        /// <returns>Player's health</returns>
        public float GetHealth()
        {
            return float.Parse(Oakwood.CallFunction("oak_player_health_get", new object[] { ID })[1].ToString());
        }

        /// <summary>
        /// Gets player's heading
        /// </summary>
        /// <returns>Player's heading</returns>
        public float GetHeading()
        {
            return float.Parse(Oakwood.CallFunction("oak_player_heading_get", new object[] { ID })[1].ToString());
        }

        /// <summary>
        /// Gets player's position
        /// </summary>
        /// <returns>Vector of player's position, otherwise nulled Vector</returns>
        public OakVec3 GetPosition()
        {
            object[] pos = (object[])Oakwood.CallFunctionArray("oak_player_position_get", new object[] { ID })[1];

            float posX = 0.0f;
            float posY = 0.0f;
            float posZ = 0.0f;

            try
            {
                posX = float.Parse(pos[0].ToString());
            }
            catch
            {
                posX = 0.0f;
            }

            try
            {
                posY = float.Parse(pos[1].ToString());
            }
            catch
            {
                posY = 0.0f;
            }

            try
            {
                posZ = float.Parse(pos[2].ToString());
            }
            catch
            {
                posZ = 0.0f;
            }

            return new OakVec3(posX, posY, posZ);
        }

        /// <summary>
        /// Gets player's direction
        /// </summary>
        /// <returns>Vector of player's direction, otherwise nulled Vector</returns>
        public OakVec3 GetDirection()
        {
            object[] dir = (object[])Oakwood.CallFunctionArray("oak_player_direction_get", new object[] { ID })[1];

            float dirX = 0.0f;
            float dirY = 0.0f;
            float dirZ = 0.0f;

            try
            {
                dirX = float.Parse(dir[0].ToString());
            }
            catch
            {
                dirX = 0.0f;
            }

            try
            {
                dirY = float.Parse(dir[1].ToString());
            }
            catch
            {
                dirY = 0.0f;
            }

            try
            {
                dirZ = float.Parse(dir[2].ToString());
            }
            catch
            {
                dirZ = 0.0f;
            }

            return new OakVec3(dirX, dirY, dirZ);
        }

        /// <summary>
        /// Gets player's visibility
        /// </summary>
        /// <param name="type">Visibility type</param>
        /// <returns></returns>
        public bool GetVisibility(Visibility type)
        {
            return Convert.ToBoolean(int.Parse(Oakwood.CallFunction("oak_player_visibility_get", new object[] { ID, (int)type })[1].ToString()));
        }

        /// <summary>
        /// Sets player's visibility
        /// </summary>
        /// <param name="type">Visibility type</param>
        /// <param name="state">Visibility state</param>
        /// <returns></returns>
        public bool SetVisibility(Visibility type, bool state)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_player_visibility_set", new object[] { ID, (int)type, Convert.ToInt32(state) })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Vehicle class
    /// </summary>
    public class OakwoodVehicle
    {
        /// <summary>
        /// Vehicle ID
        /// </summary>
        public int ID;
        /// <summary>
        /// Vehicle model name
        /// </summary>
        public string Model;
        /// <summary>
        /// Vehicle name
        /// </summary>
        public string Name;

        /// <summary>
        /// Spawns a vehicle
        /// </summary>
        /// <param name="model">Car Model</param>
        /// <param name="position">Spawn Position</param>
        /// <param name="angle">Spawn Angle in degrees</param>
        /// <returns>True if function is successful</returns>
        public static OakwoodVehicle Spawn(OakwoodVehicleModel model, OakVec3 position, float angle)
        {
            object[] r = Oakwood.CallFunction("oak_vehicle_spawn", new object[] { model.Modelname + "\0", model.Modelname.Length + 1, new float[] { position.x, position.y, position.z }, angle });

            int vehID = -1;

            int ret = int.Parse(r[0].ToString());

            try
            {
                vehID = int.Parse(r[1].ToString());
            }
            catch
            {
                vehID = -1;
            }

            if (vehID == -1)
            {
                return null;
            }

            if (ret == -1)
            {
                Oakwood.CallFunction("oak_vehicle_despawn", new object[] { vehID });
                return null;
            }

            OakwoodVehicle newVeh = new OakwoodVehicle();
            newVeh.ID = vehID;
            newVeh.Model = model.Modelname;
            newVeh.Name = model.Name;

            Oakwood.Vehicles.Add(newVeh);

            return newVeh;
        }

        /// <summary>
        /// De-Spawns (deletes) a car
        /// </summary>
        /// <returns>True if function is successful</returns>
        public bool Despawn()
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_despawn", new object[] { ID })[0].ToString());

            if (ret == 0)
            {
                foreach (OakwoodVehicle veh in Oakwood.Vehicles)
                {
                    if (veh.ID == ID)
                    {
                        Oakwood.Vehicles.Remove(veh);
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks, if the car is valid
        /// </summary>
        /// <returns>True if car is valid</returns>
        public bool IsValid()
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_invalid", new object[] { ID })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Repairs a car
        /// </summary>
        /// <returns>True if function is successful</returns>
        public bool Repair()
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_repair", new object[] { ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets car position
        /// </summary>
        /// <param name="position">New Position</param>
        /// <returns>True if function is successful</returns>
        public bool SetPosition(OakVec3 position)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_position_set", new object[] { ID, new float[] { position.x, position.y, position.z } })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets car direction
        /// </summary>
        /// <param name="direction">New Direction</param>
        /// <returns>True if function is successful</returns>
        public bool SetDirection(OakVec3 direction)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_direction_set", new object[] { ID, new float[] { direction.x, direction.y, direction.z } })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets car heading
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>True if function is successful</returns>
        public bool SetHeading(float angle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_direction_set", new object[] { ID, angle })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets car fuel
        /// </summary>
        /// <param name="fuelLevel">Fuel Level</param>
        /// <returns>True if function is successful</returns>
        public bool SetFuel(float fuelLevel)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_fuel_set", new object[] { ID, fuelLevel })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets car transparency
        /// </summary>
        /// <param name="transparency">How transparent the vehicle will be</param>
        /// <returns>True if function is successful</returns>
        public bool SetTransparency(float transparency)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_transparency_set", new object[] { ID, transparency })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets car lock state
        /// </summary>
        /// <param name="state">New Lock State</param>
        /// <returns>True if function is successful</returns>
        public bool SetLock(VehicleLockState state)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_lock_set", new object[] { ID, (int)state })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets car position
        /// </summary>
        /// <returns>Vector of car position, otherwise returns nulled Vector</returns>
        public OakVec3 GetPosition()
        {
            object[] pos = (object[])Oakwood.CallFunctionArray("oak_vehicle_position_get", new object[] { ID })[1];

            if (pos == null)
            {
                return new OakVec3(0, 0, 0);
            }

            float posX = 0.0f;
            float posY = 0.0f;
            float posZ = 0.0f;

            try
            {
                posX = float.Parse(pos[0].ToString());
            }
            catch
            {
                posX = 0.0f;
            }

            try
            {
                posY = float.Parse(pos[1].ToString());
            }
            catch
            {
                posY = 0.0f;
            }

            try
            {
                posZ = float.Parse(pos[2].ToString());
            }
            catch
            {
                posZ = 0.0f;
            }

            return new OakVec3(posX, posY, posZ);
        }

        /// <summary>
        /// Gets car direction
        /// </summary>
        /// <returns>Vector of car direction, otherwise returns nulled Vector</returns>
        public OakVec3 GetDirection()
        {
            object[] dir = (object[])Oakwood.CallFunctionArray("oak_vehicle_direction_get", new object[] { ID })[1];

            float dirX = 0.0f;
            float dirY = 0.0f;
            float dirZ = 0.0f;

            try
            {
                dirX = float.Parse(dir[0].ToString());
            }
            catch
            {
                dirX = 0.0f;
            }

            try
            {
                dirY = float.Parse(dir[1].ToString());
            }
            catch
            {
                dirY = 0.0f;
            }

            try
            {
                dirZ = float.Parse(dir[2].ToString());
            }
            catch
            {
                dirZ = 0.0f;
            }

            return new OakVec3(dirX, dirY, dirZ);
        }

        /// <summary>
        /// Gets car heading
        /// </summary>
        /// <returns>Car's heading</returns>
        public float GetHeading()
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_heading_get", new object[] { ID })[1].ToString());
        }

        /// <summary>
        /// Gets car fuel
        /// </summary>
        /// <returns>Car's fuel level</returns>
        public float GetFuel()
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_fuel_get", new object[] { ID })[1].ToString());
        }

        /// <summary>
        /// Gets car transparency
        /// </summary>
        /// <returns>Car's transparency</returns>
        public float GetTransparency()
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_transparency_get", new object[] { ID })[1].ToString());
        }

        /// <summary>
        /// Gets car's lock state
        /// </summary>
        /// <returns>Car's lock state</returns>
        public VehicleLockState GetLock()
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_lock_get", new object[] { ID })[1].ToString());

            return (VehicleLockState)ret;
        }

        /// <summary>
        /// Gets car visibility
        /// </summary>
        /// <param name="type">Visibility type</param>
        /// <returns>True if the selected visibility is enabled</returns>
        public bool GetVisibility(Visibility type)
        {
            return Convert.ToBoolean(int.Parse(Oakwood.CallFunction("oak_vehicle_visibility_get", new object[] { ID, (int)type })[1].ToString()));
        }

        /// <summary>
        /// Sets car visibility
        /// </summary>
        /// <param name="type">Visibility type</param>
        /// <param name="state">Visibility state</param>
        /// <returns></returns>
        public bool SetVisibility(Visibility type, bool state)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_visibility_set", new object[] { ID, (int)type, Convert.ToInt32(state) })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }

    public enum CtrlType
    {
        None = -1,
        CtrlC = 0,
        CtrlBreak,
        CtrlClose,
        CtrlLogoff = 5,
        CtrlShutdown
    }

    /// <summary>
    /// The main class for SharpWood
    /// </summary>
    public class Oakwood
    {
        public delegate bool EventHandler(CtrlType sig);

        internal static List<OakwoodPlayer> Players = new List<OakwoodPlayer>();
        internal static List<OakwoodVehicle> Vehicles = new List<OakwoodVehicle>();

        private static string UniqueID;

        private static int apiThreadTimeout = 0;
        private static int eventThreadTimeout = 0;

        public static bool Working { get; private set; } = false;
        public static bool IsRunning = false;

        static Thread APIThread;
        static Thread ListenerThread;
        static Thread WatchdogThread;

        static Assembly currentAssembly;
        static FileVersionInfo ver;

        static int reqSocket;
        static int respSocket;

        public static List<OakwoodPlayer> GetPlayerList()
        {
            return Players;
        }

        public static List<OakwoodVehicle> GetVehicleList()
        {
            return Vehicles;
        }

        internal static void Log(string source, string message)
        {
            OakwoodEvents.log(DateTime.Now, source, message);
        }

        internal static void ThrowRuntimeError(string msg)
        {
            Log("API", $"Runtime error: {msg}");
        }

        private static void WatchDog()
        {
            System.Timers.Timer updateTimer = new System.Timers.Timer();
            updateTimer.Interval = 1000;
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                if(IsRunning)
                {
                    apiThreadTimeout++;
                    eventThreadTimeout++;

                    if (apiThreadTimeout >= 30)
                    {
                        ThrowFatal("API Thread has stopped responding");
                    }

                    if (eventThreadTimeout >= 30)
                    {
                        ThrowFatal("Event Thread has stopped responding");
                    }
                }
            };
            updateTimer.Start();
        }

        internal static void UpdateEvents(string addr)
        {
            Log("Watchdog", "Started event thread.");
            int con = Nanomsg.Connect(respSocket, addr);

            if(con < 0)
            {
                throw new Exception("Cannot connect to server!");
            }

            Timer updateTimer = new Timer();
            updateTimer.Interval = 5;
            updateTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                eventThreadTimeout = 0;

                byte[] data = Nanomsg.Receive(respSocket, Nanomsg.SendRecvFlags.DONTWAIT);

                if (data != null)
                {
                    MPack rec = MPack.ParseFromBytes(data);

                    string eventName = rec[0].ToString();

                    List<object> args = new List<object>();

                    foreach (MPack v in ((MPackArray)rec).Skip(1))
                    {
                        args.Add(v.Value);
                    }

                    if (!OakwoodCommandSystem.CallEvent(eventName, args.ToArray()))
                    {
                        Log("EventHandler", $"Error: Cannot call event '{eventName}'!");
                    }

                    Nanomsg.Send(respSocket, Encoding.UTF8.GetBytes("ok"), Nanomsg.SendRecvFlags.DONTWAIT);
                }
            };
            updateTimer.Start();
        }

        internal static object[] CallFunction(string functionName, params object[] args)
        {
            MPackArray arg = new MPackArray();

            if(args != null)
            {
                foreach (object a in args)
                {
                    arg.Add(MPack.From(a));
                }
            }

            MPackArray req = new MPackArray();
            req.Add(functionName);
            req.Add(arg);

            byte[] data = req.EncodeToBytes();

            Nanomsg.Send(reqSocket, data, Nanomsg.SendRecvFlags.DONTWAIT);

            byte[] d = Nanomsg.Receive(reqSocket, Nanomsg.SendRecvFlags.NONE);

            MPackArray res;

            int statuscode = -500;
            object result = null;

            if(d != null)
            {
                res = (MPackArray)MPack.ParseFromBytes(d);

                statuscode = int.Parse(res[0].Value.ToString());
                result = res[1].Value;

                if (res[1].ToString().Contains("Error"))
                {
                    string error = res[1].ToString().Split(':')[1];
                    result = error;
                    ThrowRuntimeError(error.Substring(1));
                }
            }

            if(statuscode == -500 && result == null)
            {
                ThrowRuntimeError($"Response from '{functionName}' is NULL.");
            }

            return new object[] { statuscode, result };
        }

        internal static object[] CallFunctionArray(string functionName, params object[] args)
        {
            MPackArray arg = new MPackArray();

            if (args != null)
            {
                foreach (object a in args)
                {
                    arg.Add(MPack.From(a));
                }
            }

            MPackArray req = new MPackArray();
            req.Add(functionName);
            req.Add(arg);

            byte[] data = req.EncodeToBytes();

            Nanomsg.Send(reqSocket, data, Nanomsg.SendRecvFlags.DONTWAIT);

            byte[] d = Nanomsg.Receive(reqSocket, Nanomsg.SendRecvFlags.NONE);

            MPackArray res;

            int statuscode = -500;
            object[] result = null;

            if (d != null)
            {
                res = (MPackArray)MPack.ParseFromBytes(d);

                statuscode = int.Parse(res[0].Value.ToString());

                object val1 = null;
                object val2 = null;
                object val3 = null;

                try
                {
                    val1 = res[1][0].Value;
                }
                catch
                {
                    val1 = null;
                }

                try
                {
                    val2 = res[1][1].Value;
                }
                catch
                {
                    val2 = null;
                }

                try
                {
                    val3 = res[1][2].Value;
                }
                catch
                {
                    val3 = null;
                }

                result = new object[] { val1, val2, val3 };

                if (res[1].ToString().Contains("Error"))
                {
                    string error = res[1].ToString().Split(':')[1];
                    result[0] = error;
                    ThrowRuntimeError(error.Substring(1));
                }
            }

            if(statuscode == -500 && result == null)
            {
                ThrowRuntimeError($"Response from '{functionName}' is NULL.");   
            }

            return new object[] { statuscode, result };
        }

        /// <summary>
        /// Shuts down the SharpWood
        /// </summary>
        /// <returns>True on success, exception on failure.</returns>
        public static bool KillClient()
        {
            IsRunning = false;
            Working = false;
            OakwoodCommandSystem.CallEvent("stop", null);
            Nanomsg.UShutdown(respSocket, 0);
            Nanomsg.UShutdown(reqSocket, 0);

            _quitEvent.Set();

            return true;
        }

        static bool fatalTriggered = false;

        /// <summary>
        /// Throws an fatal error
        /// </summary>
        /// <param name="message"></param>
        public static void ThrowFatal(string message)
        {
            Log("Main", $"Fatal error: {message}");

            if (!fatalTriggered)
            {
                fatalTriggered = true;

                foreach (OakwoodPlayer player in Players)
                {
                    player.Kick("Gamemode has stopped responding");
                }

                IsRunning = false;
                Working = false;

                if(!OakwoodCommandSystem.CallEvent("stop", null))
                {
                    Environment.Exit(1);
                }
            }
        }

        internal static void UpdateClient(string addr)
        {
            Log("Watchdog", "Started API thread.");
            int con = Nanomsg.Connect(reqSocket, addr);

            if (con < 0)
            {
                throw new Exception("Cannot connect to server!");
            }

            int heartbeat = 15000;

            Timer updateTimer = new Timer();
            updateTimer.Interval = 1000; // Updates every second
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                object uido = CallFunction("oak__heartbeat", null)[1];

                if (uido != null)
                {
                    if (uido.ToString() != UniqueID)
                    {
                        UniqueID = uido.ToString();

                        if (!Working && IsRunning)
                        {
                            OakwoodCommandSystem.RegisterEvent("shConBreak", OakwoodEvents.OnConBreak);
                            OakwoodCommandSystem.RegisterEvent("playerConnect", OakwoodEvents.OnConnect);
                            OakwoodCommandSystem.RegisterEvent("playerDisconnect", OakwoodEvents.OnDisconnect);
                            OakwoodCommandSystem.RegisterEvent("playerDeath", OakwoodEvents.OnPKill);
                            OakwoodCommandSystem.RegisterEvent("playerHit", OakwoodEvents.OnPlHit);
                            OakwoodCommandSystem.RegisterEvent("vehicleUse", OakwoodEvents.OnPlVehicleUse);
                            OakwoodCommandSystem.RegisterEvent("vehicleDestroy", OakwoodEvents.OnVehDestroy);
                            OakwoodCommandSystem.RegisterEvent("playerKey", OakwoodEvents.OnPlKey);
                            OakwoodCommandSystem.RegisterEvent("playerChat", OakwoodEvents.OnChat);
                            OakwoodCommandSystem.RegisterEvent("console", OakwoodEvents.OnConsole);
                            OakwoodCommandSystem.RegisterEvent("start", OakwoodEvents.start);
                            OakwoodCommandSystem.RegisterEvent("stop", OakwoodEvents.stop);

                            OakwoodCommandSystem.CallEvent("start", null);

                            updateTimer.Interval = heartbeat;

                            Working = true;
                        }
                    }
                }

                apiThreadTimeout = 0;
            };

            updateTimer.Start();
        }

        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        /// <summary>
        /// Creates a API client instance
        /// </summary>
        /// <param name="inbound">Inbound address (used for Events)</param>
        /// <param name="outbound">Outbound address (used for Function calls)</param>
        public static void CreateClient(string inbound, string outbound)
        {
            if(!System.IO.File.Exists("nanomsg.dll") && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ThrowFatal("Cannot find nanomsg.dll!");
            }

            if (!System.IO.File.Exists("libnanomsg.so") && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ThrowFatal("Cannot find libnanomsg.so!");
            }

            if (!IsRunning)
            {
                string inboundAddr = "ipc://oakwood-inbound";
                string outboundAddr = "ipc://oakwood-outbound";

                if (inbound != null)
                    inboundAddr = inbound;

                if (outbound != null)
                    outboundAddr = outbound;

                Log("Main", $"SharpWood {GetVersion()}");
                Log("Main", $"Made by Sevenisko");

                Log("Connection", $"Connecting inbound to '{inboundAddr}'");
                Log("Connection", $"Connecting outbound to '{outboundAddr}'");

                reqSocket = Nanomsg.Socket(Nanomsg.Domain.SP, Nanomsg.Protocol.REQ);
                respSocket = Nanomsg.Socket(Nanomsg.Domain.SP, Nanomsg.Protocol.RESPONDENT);

                if(reqSocket < 0)
                {
                    ThrowFatal("Cannot create REQ socket!");
                }
                if(respSocket < 0)
                {
                    ThrowFatal("Cannot create RESPONDENT socket!");
                }

                Debug.Assert(Nanomsg.SetSockOpt(reqSocket, Nanomsg.SocketOption.SUB_SUBSCRIBE, 0) >= 0);

                APIThread = new Thread(() => UpdateClient(outboundAddr));

                ListenerThread = new Thread(() => UpdateEvents(inboundAddr));

                WatchdogThread = new Thread(new ThreadStart(WatchDog));

                ListenerThread.Name = "Event Listener Thread";
                APIThread.Name = "Function Call Thread";
                WatchdogThread.Name = "Watchdog Thread";

                APIThread.Start();

                ListenerThread.Start();

                WatchdogThread.Start();

                IsRunning = true;

                _quitEvent.WaitOne();
            }
            else
            {
                Log("Oakwood" ,"Error: Oakwood API instance is already running!");
            }
        }

        /// <summary>
        /// Get SharpWood version
        /// </summary>
        /// <returns>Version string 'vX.X.X.X'</returns>
        public static string GetVersion()
        {
            currentAssembly = typeof(Oakwood).Assembly;
            ver = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            return $"v{ver.FileMajorPart}.{ver.FileMinorPart}.{ver.FileBuildPart}";
        }
    }
}
