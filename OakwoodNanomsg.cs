using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Sevenisko.SharpWood
{
    public class Nanomsg
    {
        internal enum Domain
        {
            SP = 1,
            SP_RAW = 2
        }

        internal enum SocketOption
        {
            LINGER = 1,
            SNDBUF = 2,
            RCVBUF = 3,
            SNDTIMEO = 4,
            RCVTIMEO = 5,
            RECONNECT_IVL = 6,
            RECONNECT_IVL_MAX = 7,
            SNDPRIO = 8,
            SNDFD = 10,
            RCVFD = 11,
            DOMAIN = 12,
            PROTOCOL = 13,
            IPV4ONLY = 14,
            TCP_NODELAY = Constants.NN_TCP_NODELAY,
            SURVEYOR_DEADLINE = Constants.NN_SURVEYOR_DEADLINE,
            REQ_RESEND_IVL = Constants.NN_REQ_RESEND_IVL,
            SUB_SUBSCRIBE = Constants.NN_SUB_SUBSCRIBE,
            SUB_UNSUBSCRIBE = Constants.NN_SUB_UNSUBSCRIBE
        }

        internal enum SocketOptionTcp
        {
            NoDelay = Constants.NN_TCP_NODELAY
        }

        internal enum SocketOptionSurvey
        {
            SurveyorDeadline = Constants.NN_SURVEYOR_DEADLINE
        }

        internal enum SocketOptionRequest
        {
            RequestResendInterval = Constants.NN_REQ_RESEND_IVL
        }

        internal enum SocketOptionSub
        {
            Subscribe = Constants.NN_SUB_SUBSCRIBE,
            Unsubscribe = Constants.NN_SUB_UNSUBSCRIBE
        }

        internal enum SocketOptionLevel
        {
            Default = Constants.NN_SOL_SOCKET,
            Ipc = Constants.NN_IPC,
            InProcess = Constants.NN_INPROC,
            Tcp = Constants.NN_TCP,
            Pair = Constants.NN_PAIR,
            Publish = Constants.NN_PUB,
            Subscribe = Constants.NN_SUB,
            Request = Constants.NN_REQ,
            Reply = Constants.NN_REP,
            Push = Constants.NN_PUSH,
            Pull = Constants.NN_PULL,
            Surveyor = Constants.NN_SURVEYOR,
            Respondent = Constants.NN_RESPONDENT,
            Bus = Constants.NN_BUS
        }

        internal enum Protocol
        {
            PAIR = Constants.NN_PAIR,
            PUB = Constants.NN_PUB,
            SUB = Constants.NN_SUB,
            REQ = Constants.NN_REQ,
            REP = Constants.NN_REP,
            PUSH = Constants.NN_PUSH,
            PULL = Constants.NN_PULL,
            SURVEYOR = Constants.NN_SURVEYOR,
            RESPONDENT = Constants.NN_RESPONDENT,
            BUS = Constants.NN_BUS
        }

        internal enum Transport
        {
            IPC = Constants.NN_IPC,
            INPROC = Constants.NN_INPROC,
            TCP = Constants.NN_TCP
        }

        internal enum SendRecvFlags
        {
            NONE = 0,
            DONTWAIT = 1
        }

        internal enum Error
        {
            NONE = 0
        }

        [Flags]
        internal enum Events
        {
            POLLIN = 0x01,
            POLLOUT = 0x02
        }

        internal class Constants
        {
            internal const int

            NN_SOL_SOCKET = 0,

            NN_MSG = -1,

            // pair protocol related constants
            NN_PROTO_PAIR = 1,
            NN_PAIR = NN_PROTO_PAIR * 16 + 0,

            // pubsub protocol related constants
            NN_PROTO_PUBSUB = 2,
            NN_PUB = NN_PROTO_PUBSUB * 16 + 0,
            NN_SUB = NN_PROTO_PUBSUB * 16 + 1,
            NN_SUB_SUBSCRIBE = 1,
            NN_SUB_UNSUBSCRIBE = 2,

            // reqrep protocol related constants
            NN_PROTO_REQREP = 3,
            NN_REQ = NN_PROTO_REQREP * 16 + 0,
            NN_REP = NN_PROTO_REQREP * 16 + 1,
            NN_REQ_RESEND_IVL = 1,

            // pipeline protocol related constants
            NN_PROTO_PIPELINE = 5,
            NN_PUSH = NN_PROTO_PIPELINE * 16 + 0,
            NN_PULL = NN_PROTO_PIPELINE * 16 + 1,

            // survey protocol related constants
            NN_PROTO_SURVEY = 6,
            NN_SURVEYOR = NN_PROTO_SURVEY * 16 + 2,
            NN_RESPONDENT = NN_PROTO_SURVEY * 16 + 3,
            NN_SURVEYOR_DEADLINE = 1,

            // bus protocol related constants
            NN_PROTO_BUS = 7,
            NN_BUS = NN_PROTO_BUS * 16 + 0,

            // tcp transport related constants
            NN_TCP = -3,
            NN_TCP_NODELAY = 1,

            NN_IPC = -2,
            NN_INPROC = -1;
        }

        [DllImport("nanomsg.dll", EntryPoint = "nn_socket")]
        internal static extern int UCreateSocket(int domain, int protocol);
        [DllImport("nanomsg.dll", EntryPoint = "nn_connect")]
        internal static extern int UConnect(int socket, [MarshalAs(UnmanagedType.LPStr)]string address);
        [DllImport("nanomsg.dll", EntryPoint = "nn_bind")]
        internal static extern int UBind(int socket, [MarshalAs(UnmanagedType.LPStr)]string address);
        [DllImport("nanomsg.dll", EntryPoint = "nn_setsockopt")]
        internal static extern int USetSocketOption(int socket, int level, int option, IntPtr val, int length);
        [DllImport("nanomsg.dll", EntryPoint = "nn_getsockopt")]
        internal static extern int UGetSockOption(int socket, int level, int option, ref int val, ref int length);
        [DllImport("nanomsg.dll", EntryPoint = "nn_recv")]
        private static extern int UReceive(int socket, ref IntPtr buf, int constant, int flags);
        [DllImport("nanomsg.dll", EntryPoint = "nn_send")]
        internal static extern int USend(int socket, byte[] data, int length, int flags);
        [DllImport("nanomsg.dll", EntryPoint = "nn_close")]
        internal static extern int UClose(int socket);
        [DllImport("nanomsg.dll", EntryPoint = "nn_freemsg")]
        private static extern int UFreeMessage(IntPtr buf);
        [DllImport("nanomsg.dll", EntryPoint = "nn_shutdown")]
        internal static extern int UShutdown(int socket, int how);

        internal static int Socket(Domain domain, Protocol protocol)
        {
            return UCreateSocket((int)domain, (int)protocol);
        }

        internal static int Connect(int s, string addr)
        {
            return UConnect(s, addr);
        }

        internal static int Bind(int s, string addr)
        {
            return UBind(s, addr);
        }

        internal static int SetSockOpt(int s, SocketOption option, string val)
        {
            unsafe
            {
                var bs = Encoding.UTF8.GetBytes(val);
                fixed (byte* pBs = bs)
                {
                    return USetSocketOption(s, Constants.NN_SOL_SOCKET, (int)option, new IntPtr(pBs), bs.Length);
                }
            }
        }

        internal static int SetSockOpt(int s, SocketOptionLevel level, SocketOption option, string val)
        {
            unsafe
            {
                var bs = Encoding.UTF8.GetBytes(val);
                fixed (byte* pBs = bs)
                {
                    return USetSocketOption(s, (int)level, (int)option, new IntPtr(pBs), bs.Length);
                }
            }
        }

        internal static int SetSockOpt(int s, SocketOption option, int val)
        {
            unsafe
            {
                return USetSocketOption(s, Constants.NN_SOL_SOCKET, (int)option, new IntPtr(&val), sizeof(int));
            }
        }

        internal static int SetSockOpt(int s, SocketOptionLevel level, SocketOption option, int val)
        {
            unsafe
            {
                return USetSocketOption(s, (int)level, (int)option, new IntPtr(&val), sizeof(int));
            }
        }

        internal static int GetSockOpt(int s, SocketOption option, out int val)
        {
            int optvallen = sizeof(int);
            int optval = 0;

            int rc = UGetSockOption(s, Constants.NN_SOL_SOCKET, (int)option, ref optval, ref optvallen);

            val = optval;

            return rc;
        }

        internal static int GetSockOpt(int s, Protocol level, int option, out int val)
        {
            int optvallen = sizeof(int);
            int optval = 0;

            int rc =  UGetSockOption(s, (int)level, option, ref optval, ref optvallen);

            val = optval;

            return rc;
        }

        internal static byte[] Receive(int s, SendRecvFlags flags)
        {
            IntPtr buffer = IntPtr.Zero;
            int rc = UReceive(s, ref buffer, Constants.NN_MSG, (int)flags);

            if (rc < 0 || buffer == null)
            {
                return null;
            }

            byte[] buf = new byte[rc];

            try
            {
                Marshal.Copy(buffer, buf, 0, rc);
            }
            finally
            {
                int rc_free = UFreeMessage(buffer);
                if (rc_free != 0)
                    throw new InvalidOperationException("Cannot free message!");
            }

            return buf;
        }

        internal static int Send(int s, byte[] buf, SendRecvFlags flags)
        {
            return USend(s, buf, buf.Length, (int)flags);
        }
    }
}
