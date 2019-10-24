using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevenisko.SharpWood
{
    public delegate bool OakCommandCallback(OakwoodPlayer player, params object[] args);
    public delegate bool OakEventCallback(params object[] args);

    public class OakwoodCommandSystem
    {
        private static Dictionary<string, OakCommandCallback> cmdRepository = new Dictionary<string, OakCommandCallback>();
        private static Dictionary<string, OakEventCallback> eventRepository = new Dictionary<string, OakEventCallback>();

        public static void RegisterCommand(string command, OakCommandCallback callback)
        {
            if(!HasCommand(command))
            {
                cmdRepository[command] = new OakCommandCallback(callback);
                Console.WriteLine($"[INFO] Registered command `{command}`.");
            }
            else
            {
                Console.WriteLine($"[WARN] Command `{command}` is already registered.");
            }
        }

        public static void RegisterEvent(string eventName, OakEventCallback callback)
        {
            if(!HasEvent(eventName))
            {
                eventRepository[eventName] = callback;
                Console.WriteLine($"[INFO] Registered event `{eventName}`.");
            }
            else
            {
                Console.WriteLine($"[WARN] Event `{eventName}` is already registered.");
            }
        }

        public static bool HasCommand(string command)
        {
            return cmdRepository.ContainsKey(command);
        }

        internal static bool HasEvent(string eventName)
        {
            return eventRepository.ContainsKey(eventName);
        }

        public static bool ExecuteCommand(OakwoodPlayer player, string command, string[] args)
        {
            if (HasCommand(command))
            {
                return cmdRepository[command](player, args);
            }
            else
            {
                Console.WriteLine($"[ERROR] Command '{command}' doesn't exist!");
                return false;
            }
        }

        internal static bool CallEvent(string eventName, object[] args)
        {
            if(HasEvent(eventName))
            {
                return eventRepository[eventName](args);
            }
            else
            {
                Console.WriteLine($"[WARN]: Event '{eventName}' isn't registered!");
                return false;
            }
        }
    }
}
