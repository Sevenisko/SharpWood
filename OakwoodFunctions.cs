using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sevenisko.SharpWood
{
    #region HUD Functions
    public class OakHUD
    {
        public static bool Fade(OakwoodPlayer player, OakwoodFade type, int duration, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_fadeout", new object[] { player.ID, (int)type, duration, color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Fade(OakwoodPlayer player, OakwoodFade type, int duration, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_fadeout", new object[] { player.ID, (int)type, duration, (int)color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Countdown(OakwoodPlayer player, int countdown, int duration, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_countdown", new object[] { player.ID, countdown, duration, color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Countdown(OakwoodPlayer player, int countdown, int duration, OakColor color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_countdown", new object[] { player.ID, countdown, duration, (int)color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Announce(OakwoodPlayer player, string text, float duration)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_announce", new object[] { player.ID, text + "\0", text.Length + 1, duration })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Message(OakwoodPlayer player, string text, int color)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_hud_message", new object[] { player.ID, text + "\0", text.Length + 1, color })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

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
    public class OakCamera
    {
        public static bool Set(OakwoodPlayer player, OakVec3 position, OakVec3 direction)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_set", new object[] { player.ID, new float[] { position.x, position.y, position.z }, new float[] { direction.x, direction.y, direction.z } })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool Reset(OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_unlock", new object[] { player.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool TargetPlayer(OakwoodPlayer player, OakwoodPlayer spectatedPlayer)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_player", new object[] { player.ID, spectatedPlayer.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool TargetVehicle(OakwoodPlayer player, OakwoodVehicle spectatedVehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_target_vehicle", new object[] { player.ID, spectatedVehicle.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

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
    public class OakChat
    {
        public static bool Send(OakwoodPlayer player, string message)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_chat_send", new object[] { player.ID, message + "\0", message.Length + 1 })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

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
    public class OakVehPlayer
    {
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

        public static OakwoodVehicle Inside(OakwoodPlayer player)
        {
            /*object[] ret = Oakwood.CallFunction("oak_vehicle_player_inside", new object[] { player.ID });

            int vehID = -69;

            int retCode = int.Parse(ret[0].ToString());
            
            try
            {
                vehID = int.Parse(ret[1].ToString());
            }
            catch
            {
                vehID = -64;
            }

            if (retCode == 0)
            {
                foreach(OakwoodVehicle veh in Oakwood.Vehicles)
                {
                    if(veh.ID == vehID)
                    {
                        return veh;
                    }
                }
            }*/

            return player.Vehicle;
        }

        public static VehicleSeat GetSeat(OakwoodVehicle vehicle, OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_seat_get", new object[] { player.ID })[1].ToString());

            if (ret == -1)
            {
                return VehicleSeat.None;
            }

            return (VehicleSeat)ret;
        }

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

        public static bool SetKillbox(float height)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_killbox_set", new object[] { height })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static float GetKillbox()
        {
            float killbox = float.Parse(Oakwood.CallFunction("oak_killbox_get", null)[1].ToString());

            return killbox;
        }
    }
    #endregion
}
