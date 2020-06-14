using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sevenisko.SharpWood
{
    public delegate void OakCommandCallback(OakwoodPlayer player, params object[] args);
    public delegate void OakEventCallback(params object[] args);

    /// <summary>
    /// Attribute used for command registration
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        internal string command;
        internal string description;

        public CommandAttribute(string command, string description)
        {
            this.command = command;
            this.description = description;
        }
    }

    /// <summary>
    /// Command system
    /// </summary>
    public class OakwoodCommandSystem
    {
        private static Dictionary<string, OakCommandCallback> cmdRepository = new Dictionary<string, OakCommandCallback>();
        private static Dictionary<string, OakEventCallback> eventRepository = new Dictionary<string, OakEventCallback>();
        /// <summary>
        /// Command descriptions
        /// </summary>
        public static List<string> cmdDescriptions { get; private set; }

        /// <summary>
        /// Command names
        /// </summary>
        public static List<string> cmdNames { get; private set; }

        internal static void Init(MethodInfo[] methods)
        {
            cmdDescriptions = new List<string>();

            foreach (var method in methods)
            {
                var attribute = (CommandAttribute)Attribute.GetCustomAttribute(method, typeof(CommandAttribute));
                if (attribute != null)
                {
                    cmdNames.Add(attribute.command);
                    cmdDescriptions.Add(attribute.description);

                    try
                    {
                        RegisterCommand(attribute.command, (OakwoodPlayer player, object[] args) => method.Invoke(null, new object[] { player, args }));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error registering a {attribute.command} command: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Registers a command to use
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="callback">Command callback function</param>
        internal static bool RegisterCommand(string command, OakCommandCallback callback)
        {
            if(!HasCommand(command))
            {
                cmdRepository[command] = new OakCommandCallback(callback);
                return true;
            }
            else
            {
                Console.WriteLine($"[WARN] Command `{command}` is already registered.");
                return false;
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
        internal static int GetEventCount()
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
        internal static bool ExecuteCommand(OakwoodPlayer player, string command, string[] args)
        {
            if (HasCommand(command))
            {
                cmdRepository[command](player, args);
                return true;
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
                eventRepository[eventName](args);
                return true;
            }
            else
            {
                Console.WriteLine($"[WARN]: Event '{eventName}' isn't registered!");
                return false;
            }
        }
    }
}
