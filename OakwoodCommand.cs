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

        /// <summary>
        /// Registers a command to use
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="callback">Command callback function</param>
        public static void RegisterCommand(string command, OakCommandCallback callback)
        {
            if(!HasCommand(command))
            {
                cmdRepository[command] = new OakCommandCallback(callback);
            }
            else
            {
                Console.WriteLine($"[WARN] Command `{command}` is already registered.");
            }
        }

        /// <summary>
        /// Registers a custom event
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="callback">Event callback function</param>
        public static void RegisterEvent(string eventName, OakEventCallback callback)
        {
            if(!HasEvent(eventName))
            {
                eventRepository[eventName] = callback;
            }
            else
            {
                Console.WriteLine($"[WARN] Event `{eventName}` is already registered.");
            }
        }

        /// <summary>
        /// Get registered commands count
        /// </summary>
        /// <returns>Count of registered commands</returns>
        public static int GetCommandCount()
        {
            return cmdRepository.Count;
        }

        /// <summary>
        /// Get registered events count
        /// </summary>
        /// <returns>Count of registered events</returns>
        public static int GetEventCount()
        {
            return eventRepository.Count;
        }

        /// <summary>
        /// Determines if command is registered or not
        /// </summary>
        /// <param name="command">Command name</param>
        /// <returns>True if command exists</returns>
        public static bool HasCommand(string command)
        {
            return cmdRepository.ContainsKey(command);
        }

        /// <summary>
        /// Determines if event is registered or not
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>True if event exists</returns>
        internal static bool HasEvent(string eventName)
        {
            return eventRepository.ContainsKey(eventName);
        }

        /// <summary>
        /// Calls a command
        /// </summary>
        /// <param name="player">Player sender</param>
        /// <param name="command">Command name</param>
        /// <param name="args">Command arguments</param>
        /// <returns></returns>
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

        /// <summary>
        /// Calls a event
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="args">Event arguments</param>
        /// <returns></returns>
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
