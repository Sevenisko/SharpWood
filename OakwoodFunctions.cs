using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sevenisko.SharpWood
{
    #region HUD Functions
    /// <summary>
    /// HUD Functions
    /// </summary>
    public class OakHUD
    {
        public OakwoodPlayer player;

        internal OakHUD(OakwoodPlayer pl)
        {
            player = pl;
        }

        /// <summary>
        /// Fades a screen into color
        /// </summary>
        /// <param name="type">Fade type</param>
        /// <param name="duration">Fade duration</param>
        /// <param name="color">Fade color in HEX format</param>
        /// <returns>True if the function is successful</returns>
        public bool Fade(OakwoodFade type, int duration, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_fadeout", new object[] { player.ID, (int)type, duration, color })[1].ToString());

            if (ret == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fades a screen into color
        /// </summary>
        /// <param name="type">Fade type</param>
        /// <param name="duration">Fade duration</param>
        /// <param name="color">Fade color</param>
        /// <returns>True if the function is successful</returns>
        public bool Fade(OakwoodFade type, int duration, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_fadeout", new object[] { player.ID, (int)type, duration, (int)color })[1].ToString());

            if (ret == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts a countdown
        /// </summary>
        /// <param name="countdown">Countdown length in seconds</param>
        /// <param name="duration">Countdown duration</param>
        /// <param name="color">Countdown color in HEX format</param>
        /// <returns>True if the function is successful</returns>
        public bool Countdown(int countdown, int duration, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_countdown", new object[] { player.ID, countdown, duration, color })[1].ToString());

            if (ret == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts a countdown
        /// </summary>
        /// <param name="countdown">Countdown length in seconds</param>
        /// <param name="duration">Countdown duration</param>
        /// <param name="color">Countdown color</param>
        /// <returns>True if the function is successful</returns>
        public bool Countdown(int countdown, int duration, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_countdown", new object[] { player.ID, countdown, duration, (int)color })[1].ToString());

            if (ret == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts a countdown
        /// </summary>
        /// <param name="text">Announce text</param>
        /// <param name="duration">Announce duration</param>
        /// <returns>True if the function is successful</returns>
        public bool Announce(string text, float duration)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_announce", new object[] { player.ID, text + "\0", text.Length + 1, duration })[1].ToString());

            if (ret == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Prints a message in Mafia Message Box
        /// </summary>
        /// <param name="text">Message text</param>
        /// <param name="color">Text color in HEX format</param>
        /// <returns>True if the function is successful</returns>
        public bool Message(string text, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_message", new object[] { player.ID, text + "\0", text.Length + 1, color })[1].ToString());

            if (ret == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Prints a message in Mafia Message Box
        /// </summary>
        /// <param name="text">Message text</param>
        /// <param name="color">Text color</param>
        /// <returns>True if the function is successful</returns>
        public bool Message(string text, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_message", new object[] { player.ID, text + "\0", text.Length + 1, (int)color })[1].ToString());

            if (ret == -1)
            {
                return false;
            }

            return true;
        }
    }
    #endregion

    #region Camera Functions
    /// <summary>
    /// Camera Functions
    /// </summary>
    public class OakCamera
    {
        public OakwoodPlayer player;

        internal OakCamera(OakwoodPlayer pl)
        {
            player = pl;
        }

        /// <summary>
        /// Sets camera position
        /// </summary>
        /// <param name="position">Camera position</param>
        /// <param name="direction">Camera direction</param>
        /// <returns>True if the function is successful</returns>
        public bool Set(OakVec3 position, OakVec3 direction)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_set", new object[] { player.ID, new float[] { position.x, position.y, position.z }, new float[] { direction.x, direction.y, direction.z } })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets player's camera view to default
        /// </summary>
        /// <returns>True if the function is successful</returns>
        public bool Reset()
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_unlock", new object[] { player.ID })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Targets player's camera to another player
        /// </summary>
        /// <param name="spectatedPlayer">Player to target</param>
        /// <returns>True if the function is successful</returns>
        public bool TargetPlayer(OakwoodPlayer spectatedPlayer)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_player", new object[] { player.ID, spectatedPlayer.ID })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Targets player's camera to vehicle
        /// </summary>
        /// <param name="spectatedVehicle">Vehicle to target</param>
        /// <returns>True if the function is successful</returns>
        public bool TargetVehicle(OakwoodVehicle spectatedVehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_vehicle", new object[] { player.ID, spectatedVehicle.ID })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Un-Targets a player's camera
        /// </summary>
        /// <returns>True if the function is successful</returns>
        public bool Untarget()
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_unset", new object[] { player.ID })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }
    #endregion

    #region Chat Functions
    internal class OakChat
    {
        internal static bool Send(OakwoodPlayer player, string message)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_chat_send", new object[] { player.ID, message + "\0", message.Length + 1 })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        
        internal static bool SendAll(string message)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_chat_broadcast", new object[] { message + "\0", message.Length + 1 })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }
    #endregion

    #region Vehicle-Player Functions
    /// <summary>
    /// Vehicle-Player Functions
    /// </summary>
    public class OakVehPlayer
    {
        public OakwoodPlayer player;

        internal OakVehPlayer(OakwoodPlayer pl)
        {
            player = pl;
        }

        /// <summary>
        /// Puts a player inside of vehicle
        /// </summary>
        /// <param name="vehicle">Target vehicle</param>
        /// <param name="seat">Vehicle's seat</param>
        /// <returns>True if the function is successful</returns>
        public bool Put(OakwoodVehicle vehicle, VehicleSeat seat)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_put", new object[] { vehicle.ID, player.ID, (int)seat })[1].ToString());

            if (ret == 0)
            {
                player.Vehicle = vehicle;
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
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_remove", new object[] { vehicle.ID, player.ID })[1].ToString());

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
        /// <param name="vehicle">Target vehicle</param>
        /// <returns>Vehicle's Seat</returns>
        public VehicleSeat GetSeat(OakwoodVehicle vehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_seat_get", new object[] { vehicle.ID, player.ID })[1].ToString());

            if (ret == -1)
            {
                return VehicleSeat.None;
            }

            return (VehicleSeat)ret;
        }

        /// <summary>
        /// Gets player from seat of the vehicle
        /// </summary>
        /// <param name="vehicle">Target vehicle</param>
        /// <param name="seat">Vehicle's seat</param>
        /// <returns>Player instance, otherwise null</returns>
        public OakwoodPlayer AtSeat(OakwoodVehicle vehicle, VehicleSeat seat)
        {
            if (seat == VehicleSeat.None) return null;

            string res = Oakwood.CallFunction("oak_vehicle_player_at_seat", new object[] { vehicle.ID, (int)seat })[1].ToString();

            int ret = int.Parse(res);

            if (ret == -1 || res == "4294967293")
            {
                return null;
            }

            foreach(OakwoodPlayer player in Oakwood.Players)
            {
                if(player.ID == ret)
                {
                    return player;
                }
            }

            return null;
        }
    }
    #endregion

    #region Miscellaneous Functions
    internal class OakMisc
    {
        internal static bool Log(string message)
        {
            string msg = message + "\n\0";
            int ret = int.Parse(Oakwood.CallFunction("oak_logn", new object[] { msg, message.Length + 2 })[1].ToString());

            if(ret == 0)
            {
                return true;
            }

            return false;
        }

        internal static bool SetKillbox(float height)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_killbox_set", new object[] { height })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        internal static float GetKillbox()
        {
            float killbox = float.Parse(Oakwood.CallFunction("oak_killbox_get", null)[1].ToString());

            return killbox;
        }
    }
    #endregion
}
