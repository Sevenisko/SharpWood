using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sevenisko.SharpWood
{
    #region Player Functions
    public class OakPlayer
    {
        public static List<OakwoodPlayer> GetList()
        {
            return Oakwood.Players;
        }

        public static bool SpawnTempWeapons(OakwoodPlayer player)
        {
            object[] response = Oakwood.CallFunction("oak_temp_weapons_spawn", new object[] { player.ID });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        internal static string GetName(OakwoodPlayer player)
        {
            object[] ret = Oakwood.CallFunction("oak_player_name_get", new object[] { player.ID });
            int retCode = int.Parse(ret[0].ToString());
            string plName = "|>>>NoName<<<|";

            if (ret[1] != null)
            {
                plName = ret[1].ToString();
            }

            if(retCode != 0 || plName == "|>>>NoName<<<|")
            {
                return "|>>>failed<<<|";
            }
            else
            {
                return plName;
            }
        }

        public static bool Spawn(OakwoodPlayer player, OakVec3 pos)
        {
            object[] response = Oakwood.CallFunction("oak_player_spawn", new object[] { player.ID, new float[] { pos.x, pos.y, pos.z }, 0.0f });

            int ret = int.Parse(response[0].ToString());
            
            if(ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool Despawn(OakwoodPlayer player)
        {
            object[] response = Oakwood.CallFunction("oak_player_despawn", new object[] { player.ID });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsValid(OakwoodPlayer player)
        {
            foreach(OakwoodPlayer pl in Oakwood.Players)
            {
                if(pl == player)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Kick(OakwoodPlayer player, string reason)
        {
            object[] response = Oakwood.CallFunction("oak_player_kick", new object[] { player.ID, reason + "\0", reason.Length + 1 });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool Kill(OakwoodPlayer player)
        {
            object[] response = Oakwood.CallFunction("oak_player_kill", new object[] { player.ID });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool PlayAnim(OakwoodPlayer player, string animName)
        {
            object[] response = Oakwood.CallFunction("oak_player_play_anim", new object[] { player.ID, animName + "\0", animName.Length });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetModel(OakwoodPlayer player, string modelName)
        {
            object[] response = Oakwood.CallFunction("oak_player_model_set", new object[] { player.ID, modelName + "\0", modelName.Length });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetHealth(OakwoodPlayer player, float health)
        {
            object[] response = Oakwood.CallFunction("oak_player_health_set", new object[] { player.ID, health });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetPosition(OakwoodPlayer player, OakVec3 position)
        {
            object[] response = Oakwood.CallFunction("oak_player_position_set", new object[] { player.ID, position });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetDirection(OakwoodPlayer player, OakVec3 direction)
        {
            object[] response = Oakwood.CallFunction("oak_player_direction_set", new object[] { player.ID, direction });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetHeading(OakwoodPlayer player, float angle)
        {
            object[] response = Oakwood.CallFunction("oak_player_heading_set", new object[] { player.ID, angle });

            int ret = int.Parse(response[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static string GetModel(OakwoodPlayer player)
        {
            return Oakwood.CallFunction("oak_player_model_get", new object[] { player.ID })[1].ToString();
        }

        public static float GetHealth(OakwoodPlayer player)
        {
            return float.Parse(Oakwood.CallFunction("oak_player_health_get", new object[] { player.ID })[1].ToString());
        }

        public static float GetHeading(OakwoodPlayer player)
        {
            return float.Parse(Oakwood.CallFunction("oak_player_heading_get", new object[] { player.ID })[1].ToString());
        }

        public static OakVec3 GetPosition(OakwoodPlayer player)
        {
            object[] pos = (object[])Oakwood.CallFunctionArray("oak_player_position_get", new object[] { player.ID })[1];

            if(pos == null)
            {
                return new OakVec3(0, 0, 0);
            }

            return new OakVec3(float.Parse(pos[0].ToString()), float.Parse(pos[1].ToString()), float.Parse(pos[2].ToString()));
        }

        public static OakVec3 GetDirection(OakwoodPlayer player)
        {
            object[] dir = (object[])Oakwood.CallFunction("oak_player_direction_get", new object[] { player.ID })[1];

            return new OakVec3(float.Parse(dir[0].ToString()), float.Parse(dir[1].ToString()), float.Parse(dir[2].ToString()));
        }

        public static int GetVisibility(OakwoodPlayer player, Visibility type)
        {
            return int.Parse(Oakwood.CallFunction("oak_player_visibility_get", new object[] { player.ID, (int)type })[1].ToString());
        }

        public static bool SetVisibility(OakwoodPlayer player, Visibility type, int state)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_player_visibility_set", new object[] { player.ID, (int)type, state })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }
    #endregion

    #region Vehicle Functions
    public class OakVehicle
    {
        public static List<OakwoodVehicle> GetList()
        {
            return Oakwood.Vehicles;
        }

        public static OakwoodVehicle Spawn(OakwoodVehicleModel model, OakVec3 position, float angle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_spawn", new object[] { model.Modelname + "\0", model.Modelname.Length + 1, new float[] { position.x, position.y, position.z }, angle })[0].ToString());

            if (ret == -1)
            {
                return null;
            }

            OakwoodVehicle newVeh = new OakwoodVehicle();
            newVeh.ID = ret;
            newVeh.Model = model.Modelname;

            Oakwood.Vehicles.Add(newVeh);

            return newVeh;
        }

        public static bool Despawn(OakwoodVehicle vehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_despawn", new object[] { vehicle.ID })[0].ToString());

            if (ret == 0)
            {
                foreach (OakwoodVehicle veh in Oakwood.Vehicles)
                {
                    if (veh.ID == vehicle.ID)
                    {
                        Oakwood.Vehicles.Remove(veh);
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        public static bool IsValid(OakwoodVehicle vehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_invalid", new object[] { vehicle.ID })[0].ToString());

            if (ret == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Repair(OakwoodVehicle vehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_repair", new object[] { vehicle.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetPosition(OakwoodVehicle vehicle, OakVec3 position)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_position_set", new object[] { vehicle.ID, new float[] { position.x, position.y, position.z } })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetDirection(OakwoodVehicle vehicle, OakVec3 direction)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_direction_set", new object[] { vehicle.ID, new float[] { direction.x, direction.y, direction.z } })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetHeading(OakwoodVehicle vehicle, float angle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_direction_set", new object[] { vehicle.ID, angle })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetFuel(OakwoodVehicle vehicle, float fuelLevel)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_fuel_set", new object[] { vehicle.ID, fuelLevel })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetTransparency(OakwoodVehicle vehicle, float transparency)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_transparency_set", new object[] { vehicle.ID, transparency })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SetLock(OakwoodVehicle vehicle, VehicleLockState state)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_lock_set", new object[] { vehicle.ID, (int)state })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static OakVec3 GetPosition(OakwoodVehicle vehicle)
        {
            string posStr = Oakwood.CallFunction("oak_vehicle_position_get", new object[] { vehicle.ID })[1].ToString();
            Console.WriteLine(posStr);
            string[] p = posStr.Split(',');

            float posX = float.Parse(p[0]);
            float posY = float.Parse(p[1]);
            float posZ = float.Parse(p[2]);

            return new OakVec3(posX, posY, posZ);
        }

        public static OakVec3 GetDirection(OakwoodVehicle vehicle)
        {
            string dirStr = Oakwood.CallFunction("oak_vehicle_direction_get", new object[] { vehicle.ID })[1].ToString();
            Console.WriteLine(dirStr);
            string[] p = dirStr.Split(',');

            float dirX = float.Parse(p[0]);
            float dirY = float.Parse(p[1]);
            float dirZ = float.Parse(p[2]);

            return new OakVec3(dirX, dirY, dirZ);
        }

        public static float GetHeading(OakwoodVehicle vehicle)
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_heading_get", new object[] { vehicle.ID })[1].ToString());
        }

        public static float GetFuel(OakwoodVehicle vehicle)
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_fuel_get", new object[] { vehicle.ID })[1].ToString());
        }

        public static float GetTransparency(OakwoodVehicle vehicle)
        {
            return float.Parse(Oakwood.CallFunction("oak_vehicle_transparency_get", new object[] { vehicle.ID })[1].ToString());
        }

        public static VehicleLockState GetLock(OakwoodVehicle vehicle)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_lock_get", new object[] { vehicle.ID })[1].ToString());

            return (VehicleLockState)ret;
        }

        public static int GetVisibility(OakwoodVehicle vehicle, Visibility type)
        {
            return int.Parse(Oakwood.CallFunction("oak_vehicle_visibility_get", new object[] { vehicle.ID, (int)type })[1].ToString());
        }

        public static bool SetVisibility(OakwoodVehicle vehicle, Visibility type, int state)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_visibility_set", new object[] { vehicle.ID, (int)type, state })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }
    #endregion

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

        public static bool Countdown(OakwoodPlayer player, int countdown, int duration, Color color)
        {
            int cl = int.Parse(color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2"));

            int ret = int.Parse(Oakwood.CallFunction("oak_hud_countdown", new object[] { player.ID, countdown, duration, cl })[0].ToString());

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

        public static bool Message(OakwoodPlayer player, string text, Color color)
        {
            int cl = int.Parse(color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2"));

            int ret = int.Parse(Oakwood.CallFunction("oak_hud_message", new object[] { player.ID, text + "\0", text.Length + 1, cl })[0].ToString());

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
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_set", new object[] { player.ID, position, direction })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool Reset(OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_camera_unlock", new object[] { player.ID})[0].ToString());

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
            int ret = int.Parse(Oakwood.CallFunction("oak_chat_send", new object[] { player.ID, message + "\0", message.Length })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool SendAll(string message)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_chat_broadcast", new object[] { message + "\0", message.Length })[0].ToString());

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
                return true;
            }

            return false;
        }

        public static bool Remove(OakwoodVehicle vehicle, OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_remove", new object[] { vehicle.ID, player.ID })[0].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }

        public static bool Inside(OakwoodPlayer player)
        {
            int ret = int.Parse(Oakwood.CallFunction("oak_vehicle_player_inside", new object[] { player.ID })[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
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
