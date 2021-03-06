﻿using System;
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
using System.IO;

namespace Sevenisko.SharpWood
{
    internal class OakwoodWeap
    {
        internal OakwoodWeapon Weapon;
        internal int Ammo1, Ammo2;
        public OakwoodWeap(OakwoodWeapon weapon, int ammo1, int ammo2)
        {
            Weapon = weapon;
            Ammo1 = ammo1;
            Ammo2 = ammo2;
        }
    }

    /// <summary>
    /// Player class
    /// </summary>
    public class OakwoodPlayer
    {
        OakwoodWeap[] inventory = new OakwoodWeap[8];

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
        /// Current player health
        /// </summary>
        public float Health
        {
            get
            {
                return GetHealth();
            }
            set
            {
                SetHealth(value);
            }
        }
        /*/// <summary>
        /// Current player color
        /// </summary>
        public int Color
        {
            get
            {
                return GetColor();
            }
            set
            {
                SetColor(value);
            }
        }*/
        /// <summary>
        /// Current player heading
        /// </summary>
        public float Heading
        {
            get
            {
                return GetHeading();
            }
            set
            {
                SetHeading(value);
            }
        }
        /// <summary>
        /// Current player position
        /// </summary>
        public OakVec3 Position
        {
            get
            {
                return GetPosition();
            }
            set
            {
                SetPosition(value);
            }
        }
        /// <summary>
        /// Current player direction
        /// </summary>
        public OakVec3 Direction
        {
            get
            {
                return GetDirection();
            }
            set
            {
                SetDirection(value);
            }
        }

        /// <summary>
        /// Current player IP Address
        /// </summary>
        public string IP
        {
            get
            {
                return GetIP().Replace("::ffff:", "");
            }
        }

        /// <summary>
        /// Current player State code
        /// </summary>
        public string StateCode
        {
            get
            {
                return GetStateCode();
            }
        }

        /// <summary>
        /// Current player State name
        /// </summary>
        public string StateName
        {
            get
            {
                return GetStateName();
            }
        }

        /// <summary>
        /// Vehicle, where player is sitting
        /// </summary>
        public OakwoodVehicle Vehicle;

        /// <summary>
        /// Camera functions
        /// </summary>
        public OakCamera Camera { get; private set; }

        /// <summary>
        /// HUD functions
        /// </summary>
        public OakHUD HUD { get; private set; }

        internal OakwoodPlayer Killer;

        /// <summary>
        /// Creates a player instance
        /// </summary>
        public OakwoodPlayer()
        {
            Camera = new OakCamera(this);
            HUD = new OakHUD(this);
        }

        /// <summary>
        /// Clears player's chat
        /// </summary>
        /// <returns>True if function was successful</returns>
        public bool ClearChat()
        {
            return OakChat.Clear(this);
        }

        /// <summary>
        /// Enables/Disables player input
        /// </summary>
        /// <param name="state">Input state</param>
        /// <returns>True if function was successful</returns>
        public bool EnableInput(bool state)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_player_input_enable", new object[] { ID, Convert.ToInt32(state) })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sends a message to player
        /// </summary>
        /// <param name="message">Message text</param>
        /// <returns>True if the function is successful</returns>
        public bool SendMessage(string message)
        {
            return OakChat.Send(this, message);
        }

        /// <summary>
        /// Gives player a weapon
        /// </summary>
        /// <param name="weapon">Weapon</param>
        /// <param name="ammoLoaded">Ammo in magazine</param>
        /// <param name="ammoInInventory">Ammo in inventory</param>
        /// <returns>True if function is successful.</returns>
        public bool GiveWeapon(OakwoodWeapon weapon, int ammoLoaded, int ammoInInventory)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null || i == inventory.Length - 1)
                {
                    object[] response = Oakwood.CallFunction("oak_player_give_weapon", new object[] { ID, (int)weapon, ammoLoaded, ammoInInventory });

                    int ret = int.Parse(response[1].ToString());

                    if (ret == 0)
                    {
                        inventory[i] = new OakwoodWeap(weapon, ammoLoaded, ammoInInventory);

                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Clears player's inventory
        /// </summary>
        /// <returns>True if functions was successful</returns>
        public bool ClearInventory()
        {
            bool ret = false;

            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null)
                {
                    ret = RemoveWeapon(inventory[i].Weapon);      
                }
            }

            return ret;
        }

        /// <summary>
        /// Removes a weapon from player's inventory
        /// </summary>
        /// <param name="weapon">Weapon</param>
        /// <returns>True if function is successful.</returns>
        public bool RemoveWeapon(OakwoodWeapon weapon)
        {
            for(int i = 0; i < inventory.Length; i++)
            {
                if(inventory[i] != null && inventory[i].Weapon == weapon)
                {
                    object[] response = Oakwood.CallFunction("oak_player_remove_weapon", new object[] { ID, (int)weapon });

                    int ret = int.Parse(response[1].ToString());

                    if (ret == 0)
                    {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        internal int GetColor()
        {
            object[] response = Oakwood.CallFunction("oak_player_color_get", new object[] { ID });

            int ret = int.Parse(response[1].ToString());

            if (ret != 0 || ret != -1)
            {
                return ret;
            }

            return -1;
        }

        internal bool SetColor(OakColor color)
        {
            object[] response = Oakwood.CallFunction("oak_player_color_set", new object[] { ID, color.ConvertToInt32() });

            int ret = int.Parse(response[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        internal string GetIP()
        {
            object[] ret = Oakwood.CallFunction("oak_player_ip_get", new object[] { ID });
            int retCode = int.Parse(ret[0].ToString());
            string ip = "invalid";

            if (ret[1] != null)
            {
                ip = ret[1].ToString();
            }

            return ip;
        }

        internal string GetStateCode()
        {
            object[] ret = Oakwood.CallFunction("oak_player_statecode_get", new object[] { ID });
            int retCode = int.Parse(ret[0].ToString());
            string r = "invalid";

            if (ret[1] != null)
            {
                r = ret[1].ToString();
            }

            if (r.Contains("--"))
            {
                if (IP == "127.0.0.1")
                {
                    r = "LOC";
                }
                else
                {
                    r = "IDK";
                }
            }

            return r;
        }

        internal string GetStateName()
        {
            object[] ret = Oakwood.CallFunction("oak_player_statename_get", new object[] { ID });
            int retCode = int.Parse(ret[0].ToString());
            string r = "invalid";

            if (ret[1] != null)
            {
                r = ret[1].ToString();
            }

            if (r.Contains("N/A"))
            {
                if(IP == "127.0.0.1")
                {
                    r = "Local";
                }
                else
                {
                    r = "Unknown";
                }
            }

            return r;
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

            int ret = int.Parse(response[1].ToString());

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

            int ret = int.Parse(response[1].ToString());

            if (ret == 0)
            {
                Vehicle = null;
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

            int ret = int.Parse(response[1].ToString());

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

            int ret = int.Parse(response[1].ToString());

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

            int ret = int.Parse(response[1].ToString());

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

            int ret = int.Parse(response[1].ToString());

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
        private bool SetHealth(float health)
        {
            object[] response = Oakwood.CallFunction("oak_player_health_set", new object[] { ID, health });

            int ret = int.Parse(response[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetPosition(OakVec3 position)
        {
            object[] response = Oakwood.CallFunction("oak_player_position_set", new object[] { ID, new float[] { position.x, position.y, position.z } });

            int ret = int.Parse(response[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetDirection(OakVec3 direction)
        {
            object[] response = Oakwood.CallFunction("oak_player_direction_set", new object[] { ID, new float[] { direction.x, direction.y, direction.z } });

            int ret = int.Parse(response[1].ToString());

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
        private bool SetHeading(float angle)
        {
            object[] response = Oakwood.CallFunction("oak_player_heading_set", new object[] { ID, angle });

            int ret = int.Parse(response[1].ToString());

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
        private float GetHealth()
        {
            return float.Parse(Oakwood.CallFunction("oak_player_health_get", new object[] { ID })[1].ToString());
        }

        /// <summary>
        /// Gets player's heading
        /// </summary>
        /// <returns>Player's heading</returns>
        private float GetHeading()
        {
            return float.Parse(Oakwood.CallFunction("oak_player_heading_get", new object[] { ID })[1].ToString());
        }

        private OakVec3 GetPosition()
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

        private OakVec3 GetDirection()
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

        /// <summary>
        /// Puts a player inside of vehicle
        /// </summary>
        /// <param name="vehicle">Target vehicle</param>
        /// <param name="seat">Vehicle's seat</param>
        /// <returns>True if the function is successful</returns>
        public bool Put(OakwoodVehicle vehicle, VehicleSeat seat)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_put", new object[] { vehicle.ID, ID, (int)seat })[1].ToString());

            if (ret == 0)
            {
                Vehicle = vehicle;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a player from vehicle
        /// </summary>
        /// <param name="vehicle">Target vehicle</param>
        /// <returns>True if the function is successful</returns>
        public bool Remove(OakwoodVehicle vehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_remove", new object[] { vehicle.ID, ID })[1].ToString());

            if (ret == 0)
            {
                Vehicle = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a seat of player
        /// </summary>
        /// <param name="vehicle">Target vehicle</param>
        /// <returns>Vehicle's Seat</returns>
        public VehicleSeat GetSeat(OakwoodVehicle vehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_seat_get", new object[] { vehicle.ID, ID })[1].ToString());

            if (ret == -1)
            {
                return VehicleSeat.None;
            }

            return (VehicleSeat)ret;
        }

        /// <summary>
        /// Creates a explosion
        /// </summary>
        /// <param name="radius">Explosion radius</param>
        /// <param name="force">Explosion force</param>
        /// <returns>True if the function is successful</returns>
        public bool CreateExplosion(float radius, float force)
        {
            OakVec3 pos = Position;

            int ret = int.Parse(Oakwood.CallFunction("oak_create_explosion", new object[] { ID, new float[] { pos.x, pos.y, pos.z }, radius, force })[1].ToString());

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
        /// Current vehicle fuel
        /// </summary>
        public float Fuel
        {
            get
            {
                return GetFuel();
            }
            set
            {
                SetFuel(value);
            }
        }
        /// <summary>
        /// Current vehicle heading
        /// </summary>
        public float Heading
        {
            get
            {
                return GetHeading();
            }
            set
            {
                SetHeading(value);
            }
        }
        /// <summary>
        /// Current vehicle position
        /// </summary>
        public OakVec3 Position
        {
            get
            {
                return GetPosition();
            }
            set
            {
                SetPosition(value);
            }
        }
        /// <summary>
        /// Current vehicle direction
        /// </summary>
        public OakVec3 Direction
        {
            get
            {
                return GetDirection();
            }
            set
            {
                SetDirection(value);
            }
        }

        /// <summary>
        /// Current vehicle speed
        /// </summary>
        public float Speed
        {
            get
            {
                return GetSpeed();
            }
            set
            {
                SetSpeed(value);
            }
        }

        /// <summary>
        /// Current vehicle velocity
        /// </summary>
        public OakVec3 Velocity
        {
            get
            {
                return GetVelocity();
            }
            set
            {
                SetVelocity(value);
            }
        }

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
            var playersInVehicle = new List<OakwoodPlayer>();
            foreach (VehicleSeat seat in Enum.GetValues(typeof(VehicleSeat)))
            {
                if (seat == VehicleSeat.None)
                    continue;

                var player = AtSeat(seat);
                if (player != null)
                    playersInVehicle.Add(player);
            }

            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_despawn", new object[] { ID })[1].ToString());

            if (ret == 0)
            {
                foreach (OakwoodVehicle veh in Oakwood.Vehicles)
                {
                    if (veh.ID == ID)
                    {
                        foreach (var player in playersInVehicle)
                            player.Vehicle = null;
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
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_invalid", new object[] { ID })[1].ToString());

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
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_repair", new object[] { ID })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetPosition(OakVec3 position)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_position_set", new object[] { ID, new float[] { position.x, position.y, position.z } })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetDirection(OakVec3 direction)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_direction_set", new object[] { ID, new float[] { direction.x, direction.y, direction.z } })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetHeading(float angle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_heading_set", new object[] { ID, angle })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetFuel(float fuelLevel)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_fuel_set", new object[] { ID, fuelLevel })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetSpeed(float newSpeed)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_speed_set", new object[] { ID, newSpeed })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private bool SetVelocity(OakVec3 newVelocity)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_velocity_set", new object[] { ID, new float[] { newVelocity.x, newVelocity.y, newVelocity.z } })[1].ToString());

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
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_transparency_set", new object[] { ID, transparency })[1].ToString());

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
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_lock_set", new object[] { ID, (int)state })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        private OakVec3 GetPosition()
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

        private OakVec3 GetDirection()
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

        private float GetHeading()
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_heading_get", new object[] { ID })[1].ToString());
        }

        private float GetFuel()
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_fuel_get", new object[] { ID })[1].ToString());
        }

        private float GetSpeed()
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_speed_get", new object[] { ID })[1].ToString());
        }

        private OakVec3 GetVelocity()
        {
            object[] pos = (object[])Oakwood.CallFunctionArray("oak_vehicle_velocity_get", new object[] { ID })[1];

            if (pos == null)
            {
                return new OakVec3(0, 0, 0);
            }

            float velX = 0.0f;
            float velY = 0.0f;
            float velZ = 0.0f;

            try
            {
                velX = float.Parse(pos[0].ToString());
            }
            catch
            {
                velX = 0.0f;
            }

            try
            {
                velY = float.Parse(pos[1].ToString());
            }
            catch
            {
                velY = 0.0f;
            }

            try
            {
                velZ = float.Parse(pos[2].ToString());
            }
            catch
            {
                velZ = 0.0f;
            }

            return new OakVec3(velX, velY, velZ);
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
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_visibility_set", new object[] { ID, (int)type, Convert.ToInt32(state) })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Puts a player inside of vehicle
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="seat">Vehicle's seat</param>
        /// <returns>True if the function is successful</returns>
        public bool Put(OakwoodPlayer player, VehicleSeat seat)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_put", new object[] { ID, player.ID, (int)seat })[1].ToString());

            if (ret == 0)
            {
                player.Vehicle = this;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a player from vehicle
        /// </summary>
        /// <param name="player">Target player</param>
        /// <returns>True if the function is successful</returns>
        public bool Remove(OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_remove", new object[] { ID, player.ID })[1].ToString());

            if (ret == 0)
            {
                player.Vehicle = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a seat of player
        /// </summary>
        /// <param name="player">Target player</param>
        /// <returns>Vehicle's Seat</returns>
        public VehicleSeat GetSeat(OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_seat_get", new object[] { ID, player.ID })[1].ToString());

            if (ret == -1)
            {
                return VehicleSeat.None;
            }

            return (VehicleSeat)ret;
        }

        /// <summary>
        /// Gets player from seat of the vehicle
        /// </summary>
        /// <param name="seat">Vehicle's seat</param>
        /// <returns>Player instance, otherwise null</returns>
        public OakwoodPlayer AtSeat(VehicleSeat seat)
        {
            if (seat == VehicleSeat.None) return null;

            string res = Oakwood.CallFunction("oak_vehicle_player_at_seat", new object[] { ID, (int)seat })[1].ToString();

            uint ret = uint.Parse(res);
            if (ret >= int.MaxValue)
            {
                return null;
            }

            foreach (OakwoodPlayer player in Oakwood.Players)
            {
                if (player.ID == ret)
                {
                    return player;
                }
            }

            return null;
        }

        /// <summary>
        /// Explodes a car
        /// </summary>
        /// <returns>True if function is successful</returns>
        public bool Explode()
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_explode", new object[] { ID })[1].ToString());

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

        /// <summary>
        /// Player list
        /// </summary>
        public static List<OakwoodPlayer> Players { get; internal set; } = new List<OakwoodPlayer>();
        /// <summary>
        /// Vehicle list
        /// </summary>
        public static List<OakwoodVehicle> Vehicles { get; internal set; } = new List<OakwoodVehicle>();

        private static string UniqueID;

        private static int apiThreadTimeout = 0;
        private static int eventThreadTimeout = 0;

        /// <summary>
        /// Determines if SharpWood is working or not
        /// </summary>
        public static bool Working { get; private set; } = false;
        /// <summary>
        /// Determines if SharpWood is running or not
        /// </summary>
        public static bool IsRunning = false;

        static Thread APIThread;
        static Thread ListenerThread;
        static Thread WatchdogThread;

        static Assembly currentAssembly;
        static FileVersionInfo ver;

        static int reqSocket;
        static int respSocket;

        static object networkLock;

        /// <summary>
        /// Prints a message into server console
        /// </summary>
        /// <param name="message">Message to print</param>
        /// <returns>True if function was successful</returns>
        public static bool ConLog(string message)
        {
            return OakMisc.Log(message);
        }

        /// <summary>
        /// Server Killbox
        /// </summary>
        public static float Killbox
        {
            get
            {
                return OakMisc.GetKillbox();
            }
            set
            {
                OakMisc.SetKillbox(value);
            }
        }

        internal static void Log(string source, string message)
        {
            OakwoodEvents.log(DateTime.Now, source, message);
        }

        internal static void ThrowRuntimeError(string msg)
        {
            Log("API", $"Runtime error: {msg}");
        }

        /// <summary>
        /// Sends a message to everyone on server
        /// </summary>
        /// <param name="message">Message text</param>
        /// <returns>True if the function is successful</returns>
        public static bool SendChatMessage(string message)
        {
            return OakChat.SendAll(message);
        }

        private static void WatchDog()
        {
            Timer updateTimer = new Timer();
            updateTimer.Interval = 1000;
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
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

            while (true)
            {
                eventThreadTimeout = 0;

                lock (networkLock)
                {
                    byte[] data;
                    while ((data = Nanomsg.Receive(respSocket, Nanomsg.SendRecvFlags.DONTWAIT)) != null)
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
                }

                Thread.Sleep(2);
            }
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
            byte[] d;

            lock (networkLock)
            {
                Nanomsg.Send(reqSocket, data, Nanomsg.SendRecvFlags.DONTWAIT);

                d = Nanomsg.Receive(reqSocket, Nanomsg.SendRecvFlags.NONE);
            }

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

            //Log("DebugAPI", $"[{functionName} = {statuscode}]: {result}");

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
            byte[] d;

            lock (networkLock)
            {
                Nanomsg.Send(reqSocket, data, Nanomsg.SendRecvFlags.DONTWAIT);

                d = Nanomsg.Receive(reqSocket, Nanomsg.SendRecvFlags.NONE);
            }

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

                //Log("DebugAPI", $"[{functionName} = {statuscode}]: ({val1}, {val2}, {val3})");

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
                            OakwoodCommandSystem.RegisterEvent("dialogClose", OakwoodEvents.OnDlgClose);
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if(RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    File.WriteAllBytes("nanomsg.dll", Resources.nanomsg_x64);
                else if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                    File.WriteAllBytes("nanomsg.dll", Resources.nanomsg_x86);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                File.WriteAllBytes("libnanomsg.so", Resources.libnanomsg);

            if (!IsRunning)
            {
                networkLock = new object();

                string inboundAddr = "ipc://oakwood-inbound";
                string outboundAddr = "ipc://oakwood-outbound";

                if (inbound != null)
                    inboundAddr = inbound;

                if (outbound != null)
                    outboundAddr = outbound;

                Log("Main", $"SharpWood {GetVersion()}");
                Log("Main", $"Made by Sevenisko & NanobotZ");

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

                OakwoodCommandSystem.Init(Assembly.GetCallingAssembly().GetTypes().SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)).ToArray());

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
        /// <returns>Version string 'vX.X.X'</returns>
        public static string GetVersion()
        {
            currentAssembly = typeof(Oakwood).Assembly;
            ver = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            return $"v{ver.FileMajorPart}.{ver.FileMinorPart}.{ver.FileBuildPart}";
        }
    }
}
