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
    public class OakwoodPlayer
    {
        public string Name;
        public int ID;
        public string Model;
        public object PlayerData;
        public float Heading;
        public OakwoodVehicle Vehicle;
    }

    public class OakwoodVehicle
    {
        public int ID;
        public string Model;
        public string Name;
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

    public class Oakwood
    {
        public delegate bool EventHandler(CtrlType sig);

        public static List<string> OakMethods = new List<string>();

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

            while (true)
            {
                eventThreadTimeout = 0;

                byte[] data = Nanomsg.Receive(respSocket, Nanomsg.SendRecvFlags.DONTWAIT);

                if(data != null)
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

        public static bool KillClient()
        {
            IsRunning = false;
            Working = false;
            OakwoodCommandSystem.CallEvent("stop", null);
            Nanomsg.UShutdown(respSocket, 0);
            Nanomsg.UShutdown(reqSocket, 0);
                        
            return true;
        }

        static bool fatalTriggered = false;

        public static void ThrowFatal(string message)
        {
            Log("Main", $"Fatal error: {message}");

            if (!fatalTriggered)
            {
                fatalTriggered = true;

                foreach (OakwoodPlayer player in Oakwood.Players)
                {
                    OakPlayer.Kick(player, "Gamemode has stopped responding");
                }

                IsRunning = false;
                Working = false;

                OakwoodCommandSystem.CallEvent("stop", null);
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
                            string meths = CallFunction("oak__methods", null)[1].ToString();

                            string[] methods = meths.Split(';');

                            foreach (string method in methods)
                            {
                                OakMethods.Add(method.Replace("oak_", ""));
                            }

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
            };

            updateTimer.Start();

            while (true)
            {
                apiThreadTimeout = 0;
            }
        }

        /// <summary>
        /// Creates a API client instance
        /// </summary>
        /// <param name="inbound">Inbound address (used for Events)</param>
        /// <param name="outbound">Outbound address (used for Function calls)</param>
        /// <param name="autoRestart">Auto-Restart on failure</param>
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

                if(reqSocket <= -1)
                {
                    ThrowFatal("Cannot create REQ socket!");
                }
                if(respSocket <= -1)
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
            }
            else
            {
                Log("Oakwood" ,"Error: Oakwood API instance is already running!");
            }
        }

        public static string GetVersion()
        {
            currentAssembly = typeof(Oakwood).Assembly;
            ver = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            return $"v{ver.FileMajorPart}.{ver.FileMinorPart}.{ver.FileBuildPart}";
        }
    }
}
