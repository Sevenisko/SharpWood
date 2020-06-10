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
        /// <summary>
        /// Fades a screen into color
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="type">Fade type</param>
        /// <param name="duration">Fade duration</param>
        /// <param name="color">Fade color in HEX format</param>
        /// <returns>True if the function is successful</returns>
        public static bool Fade(OakwoodPlayer player, OakwoodFade type, int duration, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_fadeout", new object[] { player.ID, (int)type, duration, color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fades a screen into color
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="type">Fade type</param>
        /// <param name="duration">Fade duration</param>
        /// <param name="color">Fade color</param>
        /// <returns>True if the function is successful</returns>
        public static bool Fade(OakwoodPlayer player, OakwoodFade type, int duration, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_fadeout", new object[] { player.ID, (int)type, duration, (int)color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts a countdown
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="countdown">Countdown length in seconds</param>
        /// <param name="duration">Countdown duration</param>
        /// <param name="color">Countdown color in HEX format</param>
        /// <returns>True if the function is successful</returns>
        public static bool Countdown(OakwoodPlayer player, int countdown, int duration, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_countdown", new object[] { player.ID, countdown, duration, color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts a countdown
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="countdown">Countdown length in seconds</param>
        /// <param name="duration">Countdown duration</param>
        /// <param name="color">Countdown color</param>
        /// <returns>True if the function is successful</returns>
        public static bool Countdown(OakwoodPlayer player, int countdown, int duration, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_countdown", new object[] { player.ID, countdown, duration, (int)color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts a countdown
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="text">Announce text</param>
        /// <param name="duration">Announce duration</param>
        /// <returns>True if the function is successful</returns>
        public static bool Announce(OakwoodPlayer player, string text, float duration)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_announce", new object[] { player.ID, text + "\0", text.Length + 1, duration })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Prints a message in Mafia Message Box
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="text">Message text</param>
        /// <param name="color">Text color in HEX format</param>
        /// <returns>True if the function is successful</returns>
        public static bool Message(OakwoodPlayer player, string text, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_message", new object[] { player.ID, text + "\0", text.Length + 1, color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Prints a message in Mafia Message Box
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="text">Message text</param>
        /// <param name="color">Text color</param>
        /// <returns>True if the function is successful</returns>
        public static bool Message(OakwoodPlayer player, string text, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_message", new object[] { player.ID, text + "\0", text.Length + 1, (int)color })[0].ToString());

            if (ret == 0)
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
        /// <summary>
        /// Sets camera position
        /// </summary>
        /// <param name="player">Player to apply this effect</param>
        /// <param name="position">Camera position</param>
        /// <param name="direction">Camera direction</param>
        /// <returns>True if the function is successful</returns>
        public static bool Set(OakwoodPlayer player, OakVec3 position, OakVec3 direction)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_set", new object[] { player.ID, new float[] { position.x, position.y, position.z }, new float[] { direction.x, direction.y, direction.z } })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets player's camera view to default
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <returns>True if the function is successful</returns>
        public static bool Reset(OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_unlock", new object[] { player.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Targets player's camera to another player
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="spectatedPlayer">Player to target</param>
        /// <returns>True if the function is successful</returns>
        public static bool TargetPlayer(OakwoodPlayer player, OakwoodPlayer spectatedPlayer)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_player", new object[] { player.ID, spectatedPlayer.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Targets player's camera to vehicle
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <param name="spectatedVehicle">Vehicle to target</param>
        /// <returns>True if the function is successful</returns>
        public static bool TargetVehicle(OakwoodPlayer player, OakwoodVehicle spectatedVehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_vehicle", new object[] { player.ID, spectatedVehicle.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Un-Targets a player's camera
        /// </summary>
        /// <param name="player">Player to apply this function</param>
        /// <returns>True if the function is successful</returns>
        public static bool Untarget(OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_unset", new object[] { player.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }
    #endregion

    #region Chat Functions
    /// <summary>
    /// Chat Functions
    /// </summary>
    public class OakChat
    {
        /// <summary>
        /// Sends a message to player
        /// </summary>
        /// <param name="player">Message recipient</param>
        /// <param name="message">Message text</param>
        /// <returns>True if the function is successful</returns>
        public static bool Send(OakwoodPlayer player, string message)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_chat_send", new object[] { player.ID, message + "\0", message.Length + 1 })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sends a message to everyone on server
        /// </summary>
        /// <param name="message">Message text</param>
        /// <returns>True if the function is successful</returns>
        public static bool SendAll(string message)
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
        /// <summary>
        /// Puts a player inside of vehicle
        /// </summary>
        /// <param name="vehicle">Target vehicle</param>
        /// <param name="player">Player to put inside</param>
        /// <param name="seat">Vehicle's seat</param>
        /// <returns>True if the function is successful</returns>
        public static bool Put(OakwoodVehicle vehicle, OakwoodPlayer player, VehicleSeat seat)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_put", new object[] { vehicle.ID, player.ID, (int)seat })[0].ToString());

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
        /// <param name="player">Player to remove from</param>
        /// <returns>True if the function is successful</returns>
        public static bool Remove(OakwoodVehicle vehicle, OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_remove", new object[] { vehicle.ID, player.ID })[0].ToString());

            if (ret == 0)
            {
                player.Vehicle = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets player's vehicle
        /// </summary>
        /// <param name="player">Target player</param>
        /// <returns>Vehicle instance, otherwise null</returns>
        public static OakwoodVehicle Inside(OakwoodPlayer player)
        {
            return player.Vehicle;
        }

        /// <summary>
        /// Gets a seat of player
        /// </summary>
        /// <param name="vehicle">Target player</param>
        /// <param name="player">Target vehicle</param>
        /// <returns>Vehicle's Seat</returns>
        public static VehicleSeat GetSeat(OakwoodVehicle vehicle, OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_seat_get", new object[] { player.ID })[1].ToString());

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
        public static OakwoodPlayer AtSeat(OakwoodVehicle vehicle, VehicleSeat seat)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_at_seat", new object[] { vehicle.ID, (int)seat })[1].ToString());

            if (ret == -1)
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
    public class OakMisc
    {
        /// <summary>
        /// Sends a log message inside of server console
        /// </summary>
        /// <param name="message">Message text</param>
        /// <returns>True if the function is successful</returns>
        public static bool Log(string message)
        {
            string msg = message + "\n\0";
            int ret = int.Parse(Oakwood.CallFunction("oak_logn", new object[] { msg, message.Length + 2 })[0].ToString());

            if(ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets map's killbox
        /// </summary>
        /// <param name="height">Killbox height</param>
        /// <returns>True if the function is successful</returns>
        public static bool SetKillbox(float height)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_killbox_set", new object[] { height })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets map's killbox
        /// </summary>
        /// <returns>Killbox height</returns>
        public static float GetKillbox()
        {
            float killbox = float.Parse(Oakwood.CallFunction("oak_killbox_get", null)[1].ToString());

            return killbox;
        }
    }
    #endregion
}
