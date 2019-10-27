using System;
using NNanomsg;
using NNanomsg.Protocols;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using CS;
using System.Runtime.InteropServices;

namespace Sevenisko.SharpWood
{
    public class OakwoodPlayer
    {
        public string Name;
        public int ID;
        public string Model;
        public float Health;
        public int TpaID;
        public float Heading;
    }

    public class OakwoodVehicle
    {
        public int ID;
        public string Model;
    }

    public enum CtrlType
    {
        CtrlC = 0,
        CtrlBreak,
        CtrlClose,
        CtrlLogoff = 5,
        CtrlShutdown
    }

    public class Oakwood
    {
        public static List<string> OakMethods = new List<string>();

        internal static List<OakwoodPlayer> Players = new List<OakwoodPlayer>();
        internal static List<OakwoodVehicle> Vehicles = new List<OakwoodVehicle>();

        private static string UniqueID;

        private static int apiThreadTimeout = 0;
        private static int eventThreadTimeout = 0;

        public delegate bool HandlerRoutine(CtrlType CtrlType);

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        [DllImport("Kernel32")]
        static extern bool SetConsoleTitle(string lpConsoleTitle);

        public static bool Working { get; private set; } = false;

        private static bool CanRestart = false;

        static Thread APIThread;
        static Thread ListenerThread;
        static Thread WatchdogThread;

        static Assembly currentAssembly;
        static FileVersionInfo ver;

        static RequestSocket reqSocket;

        private static void WatchDog()
        {
            while(true)
            {
                if(Working)
                {
                    float apiPing = 350000.0f - (350000.0f - apiThreadTimeout);
                    float eventPing = 350000.0f - (350000.0f - eventThreadTimeout);

                    SetConsoleTitle($"SharpWood Development Console | API response: [{apiPing}/{eventPing}ms]");

                    if (apiThreadTimeout > 350000)
                        ThrowFatal("API Thread has stopped responding!");

                    if (eventThreadTimeout > 350000)
                        ThrowFatal("Event Thread has stopped responding!");

                    apiThreadTimeout++;
                    eventThreadTimeout++;
                }
            }
        }

        private static bool ConsoleCtrlCheck(CtrlType ctrlType)
        {
            OakwoodCommandSystem.CallEvent("shConBreak", new object[] { ctrlType });
            return true;
        }

        internal static void UpdateEvents()
        {
            eventThreadTimeout = 0;
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

            reqSocket.Send(data);

            byte[] d = reqSocket.Receive();

            MPackArray res;

            int statuscode = -1;
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
                    Console.WriteLine("[ERROR] Runtime error:" + error);
                }
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

            reqSocket.Send(data);

            byte[] d = reqSocket.Receive();

            MPackArray res;

            int statuscode = -1;
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
                    Console.WriteLine("[ERROR] Runtime error:" + error);
                }
            }

            return new object[] { statuscode, result };
        }

        public static void ReceiveRespondentMessage(IntPtr data, int length)
        {
            if(data != null)
            {
                if(length < 0 || data == null)
                {
                    return;
                }

                byte[] d = new byte[length];

                Marshal.Copy(data, d, 0, length);

                MPack rec = MPack.ParseFromBytes(d);

                string eventName = rec[0].ToString();

                List<object> args = new List<object>();

                foreach(MPack v in ((MPackArray)rec).Skip(1))
                {
                    args.Add(v.Value);
                }

                if(eventName != "playerKey")
                {
                    Console.WriteLine("{0}: {1}", eventName, string.Join(", ", args.ToArray()));
                }

                if(!OakwoodCommandSystem.CallEvent(eventName, args.ToArray()))
                {
                    Console.WriteLine($"[ERROR] Cannot execute event '{eventName}'!");
                }

                EventListener.SendData("ok");
            }
        }

        public static bool KillClient()
        {
            Working = false;
            OakwoodCommandSystem.CallEvent("stop", null);
            APIThread.Abort();
            ListenerThread.Abort();
            EventListener.StopClient();
            return true;
        }

        public static void ThrowFatal(string message)
        {
            foreach(OakwoodPlayer player in Oakwood.Players)
            {
                OakPlayer.Kick(player, "Gamemode has stopped responding");
            }

            SetConsoleTitle("SharpWood Development Console | Stopped working!");

            OakMisc.Log("[ERROR] SharpWood has stopped working!");

            APIThread.Abort();
            EventListener.StopClient();
            ListenerThread.Abort();
            OakwoodCommandSystem.CallEvent("stop", null);

            Console.WriteLine("========[FATAL ERROR]========");
            Console.WriteLine(message);
            Console.WriteLine("=============================");
            if (CanRestart)
            {
                Process myProcess = new Process();

                try
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = Assembly.GetEntryAssembly().Location;
                    myProcess.StartInfo.CreateNoWindow = false;

                    Console.WriteLine("Restarting in 3 seconds...");

                    System.Timers.Timer resetTimer = new System.Timers.Timer();
                    resetTimer.Interval = 3000;
                    resetTimer.AutoReset = false;
                    resetTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                    {
                        Console.Clear();

                        myProcess.Start();

                        Environment.Exit(1);
                    };

                    resetTimer.Start();

                    while(true)
                    {
                        // Do nothing
                    }
                }
                catch
                {
                    CanRestart = false;
                    ThrowFatal("Cannot execute restart process!");
                }
            }
            else
            {
                Console.WriteLine("Enter any key to exit...");
                Console.ReadKey();
            }
            Environment.Exit(1);
        }

        public static void UpdateClient()
        {
            Console.WriteLine("[INFO] Started API thread.");

            int update = 0;
            int heartbeat = 1500;

            while (true)
            {
                apiThreadTimeout = 0;

                if (update > heartbeat)
                {
                    object uido = CallFunction("oak__heartbeat", null)[1];

                    if (uido != null)
                    {
                        if (uido.ToString() == UniqueID)
                        {
                            update = 0;
                        }
                        else
                        {
                            UniqueID = uido.ToString();

                            if(!Working)
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
                                Working = true;
                                update = 0;
                            }
                        }
                    }
                    update = 0;
                }

                update++;
            }
        }

        public static void WriteConLine(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void CreateClient(string inbound, string outbound, bool autoRestart)
        {
            string inboundAddr = "ipc://oakwood-inbound";
            string outboundAddr = "ipc://oakwood-outbound";

            CanRestart = autoRestart;

            if (inbound != null)
                inboundAddr = inbound;

            if (outbound != null)
                outboundAddr = outbound;

            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            SetConsoleTitle("SharpWood Development Console");

            Console.WriteLine("============================");
            Console.WriteLine($"    SharpWood {GetVersion()}    ");
            Console.WriteLine($"     Made by Sevenisko     ");
            Console.WriteLine("============================");

            reqSocket = new RequestSocket();

            if(CanRestart)
            {
                Console.WriteLine("Auto-Restart on fatalError enabled.");
            }

            Console.WriteLine($"Connecting inbound to '{inboundAddr}'");
            Console.WriteLine($"Connecting outbound to '{outboundAddr}'");

            NanoFunctions functions = new NanoFunctions();
            functions.Receive = ReceiveRespondentMessage;
            functions.WriteLine = WriteConLine;
            functions.UpdateEvents = UpdateEvents;

            ListenerThread = new Thread(() => EventListener.StartClient(inboundAddr, functions));
            reqSocket.Connect(outboundAddr);

            ListenerThread.Start();

            APIThread = new Thread(new ThreadStart(UpdateClient));

            WatchdogThread = new Thread(new ThreadStart(WatchDog));

            APIThread.Start();

            WatchdogThread.Start();
        }

        public static string GetVersion()
        {
            currentAssembly = typeof(Oakwood).Assembly;
            ver = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            return $"v{ver.FileMajorPart}.{ver.FileMinorPart}.{ver.FileBuildPart}";
        }
    }
}
