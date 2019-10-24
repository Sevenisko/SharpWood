﻿using System;
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
    internal class OakwoodCommand
    {
        public string Command;
        public string[] Arguments;
    }

    public class OakwoodPlayer
    {
        public string Name;
        public int ID;
        public string Model;
        public float Health;
    }

    public class OakwoodVehicle
    {
        public int ID;
        public string Model;
    }

    public class Oakwood
    {
        public static List<string> OakMethods = new List<string>();

        internal static List<OakwoodPlayer> Players = new List<OakwoodPlayer>();
        internal static List<OakwoodVehicle> Vehicles = new List<OakwoodVehicle>();

        private static string UniqueID;

        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        public static bool Working { get; private set; } = false;

        static Thread APIThread;
        static Thread ListenerThread;

        static Assembly currentAssembly;
        static FileVersionInfo ver;

        static RequestSocket reqSocket;

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            APIThread.Abort();
            ListenerThread.Abort();
            EventListener.StopClient();
            Environment.Exit(0);
            return true;
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

                Console.WriteLine(res.ToString());

                statuscode = int.Parse(res[0].Value.ToString());
                result = new object[] { res[1][0].Value, res[1][1].Value, res[1][2].Value };

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

                object arg1;
                object arg2;
                object arg3;
                object arg4;
                object arg5;

                try
                {
                    arg1 = rec[1].ToString();
                }
                catch
                {
                    arg1 = -5;
                }

                try
                {
                    arg2 = rec[2].ToString();
                }
                catch
                {
                    arg2 = -5;
                }

                try
                {
                    arg3 = rec[3].ToString();
                }
                catch
                {
                    arg3 = -5;
                }

                try
                {
                    arg4 = rec[4].ToString();
                }
                catch
                {
                    arg4 = -5;
                }

                try
                {
                    arg5 = rec[5].ToString();
                }
                catch
                {
                    arg5 = -5;
                }

                object[] args = new object[] { arg1, arg2, arg3, arg4, arg5 };

                if(!OakwoodCommandSystem.CallEvent(eventName, args))
                {
                    Console.WriteLine($"[ERROR] Cannot execute event '{eventName}'!");
                }

                EventListener.SendData("ok");
            }
        }

        public static void ThrowFatal(string message)
        {
            APIThread.Abort();
            EventListener.StopClient();
            ListenerThread.Abort();
            OakwoodCommandSystem.CallEvent("stop", null);

            Console.WriteLine("========[FATAL ERROR]========");
            Console.WriteLine(message);
            Console.WriteLine("=============================");
            Console.WriteLine("Enter any key to exit...");
            Console.ReadKey();
            Environment.Exit(1);
        }

        public static void UpdateClient()
        {
            Console.WriteLine("[INFO] Started API thread.");

            int update = 0;
            int heartbeat = 1500;

            while (true)
            {
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

        public static void CreateClient(string inbound, string outbound)
        {
            string inboundAddr = "ipc://oakwood-inbound";
            string outboundAddr = "ipc://oakwood-outbound";

            if (inbound != null)
                inboundAddr = inbound;

            if (outbound != null)
                outboundAddr = outbound;

            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            Console.Title = "SharpWood Development Console";

            Console.WriteLine("============================");
            Console.WriteLine($"    SharpWood {GetVersion()}    ");
            Console.WriteLine($"     Made by Sevenisko     ");
            Console.WriteLine("============================");

            reqSocket = new RequestSocket();

            Console.WriteLine($"Connecting inbound to '{inboundAddr}'");
            Console.WriteLine($"Connecting outbound to '{outboundAddr}'");

            NanoFunctions functions = new NanoFunctions();
            functions.Receive = ReceiveRespondentMessage;
            functions.WriteLine = WriteConLine;

            ListenerThread = new Thread(() => EventListener.StartClient(inboundAddr, functions));
            reqSocket.Connect(outboundAddr);

            ListenerThread.Start();

            APIThread = new Thread(new ThreadStart(UpdateClient));

            APIThread.Start();
        }

        public static string GetVersion()
        {
            currentAssembly = typeof(Oakwood).Assembly;
            ver = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            return $"v{ver.FileMajorPart}.{ver.FileMinorPart}.{ver.FileBuildPart}";
        }
    }
}
