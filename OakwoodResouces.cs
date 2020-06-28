using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Sevenisko.SharpWood
{
    public enum DialogType
    {
        MsgBox, // Normal Message Box
        Input, // Text Input
        Password // Password Input
    }

    /// <summary>
    /// Class for Dialogs in Oakwood
    /// </summary>
    public class OakDialog
    {
        /// <summary>
        /// Dialog type
        /// </summary>
        public DialogType Type { get; private set; }

        /// <summary>
        /// Callback ID - Used in OnDialogClose event
        /// </summary>
        public int callbackID { get; private set; }

        /// <summary>
        /// Dialog title
        /// </summary>
        public string Title;

        /// <summary>
        /// Dialog content
        /// </summary>
        public string Message;

        /// <summary>
        /// First button text (OK button)
        /// </summary>
        public string ButtonOK;

        /// <summary>
        /// Second button text (Cancel button)
        /// </summary>
        public string ButtonCancel;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">Dialog Type</param>
        /// <param name="callbackid">Callback ID</param>
        /// <param name="title">Dialog Title</param>
        /// <param name="message">Dialog content</param>
        /// <param name="buttonok">Button #1 text (OK/Yes button)</param>
        /// <param name="buttoncancel">Button #2 text (Cancel/No button)</param>
        public OakDialog(DialogType type, int callbackid, string title, string message, string buttonok = "OK", string buttoncancel = "")
        {
            Type = type;
            callbackID = callbackid;
            Title = title;
            Message = message;
            ButtonOK = buttonok;
            ButtonCancel = buttoncancel;
        }

        /// <summary>
        /// Shows a dialog to player
        /// </summary>
        /// <param name="player">Player to see the dialog</param>
        /// <returns>True if function was successful</returns>
        public bool Show(OakwoodPlayer player)
        {
            object[] response = Oakwood.CallFunction("oak_dialog_show", new object[] { player.ID, Title + "\0", Title.Length + 1, Message + "\0", Message.Length + 1, ButtonOK + "\0", ButtonOK.Length + 1, ButtonCancel + "\0", ButtonCancel.Length + 1, callbackID, (int)Type });

            int ret = int.Parse(response[1].ToString());

            if (ret == 0)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Color used in Oakwood
    /// </summary>
    public class OakColor
    {
        public static OakColor White = new OakColor(255, 255, 255);
        public static OakColor Red = new OakColor(255, 0, 0);
        public static OakColor DarkRed = new OakColor(150, 0, 0);
        public static OakColor Blue = new OakColor(0, 0, 255);
        public static OakColor DarkBlue = new OakColor(0, 0, 150);
        public static OakColor Black = new OakColor(0, 0, 0);
        public static OakColor Green = new OakColor(0, 255, 0);
        public static OakColor DarkGreen = new OakColor(0, 150, 0);
        public static OakColor Yellow = new OakColor(255, 255, 0);
        public static OakColor Gold = new OakColor(14, 150, 0);
        public static OakColor Aqua = new OakColor(0, 255, 255);
        public static OakColor Pink = new OakColor(255, 0, 255);
        public static OakColor Purple = new OakColor(150, 0, 150);
        public static OakColor LightGray = new OakColor(130, 130, 130);
        public static OakColor Gray = new OakColor(70, 70, 70);
        public static OakColor DarkGray = new OakColor(18, 18, 18);

        public int r, g, b;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public OakColor(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        /// <summary>
        /// Converts a color into Int32 color
        /// </summary>
        /// <returns>Color in Int32</returns>
        public int ConvertToInt32()
        {
            int rgb = r;
            rgb = (rgb << 8) + g;
            rgb = (rgb << 8) + b;

            return rgb;
        }

        /// <summary>
        /// Creates a color from HEX value
        /// </summary>
        /// <param name="rgb">Color integer</param>
        /// <returns></returns>
        public static OakColor FromInt(int rgb)
        {
            return new OakColor((rgb >> 16) & 0xFF, (rgb >> 8) & 0xFF, rgb & 0xFF);
        }
    }

    /// <summary>
    /// Virtual Key Code
    /// </summary>
    public enum VirtualKey
    {
        LeftButton = 0x01,
        RightButton = 0x02,
        Cancel = 0x03,
        MiddleButton = 0x04,
        ExtraButton1 = 0x05,
        ExtraButton2 = 0x06,
        Back = 0x08,
        Tab = 0x09,
        Clear = 0x0C,
        Return = 0x0D,
        Shift = 0x10,
        Control = 0x11,
        /// <summary></summary>
        Menu = 0x12,
        /// <summary></summary>
        Pause = 0x13,
        /// <summary></summary>
        CapsLock = 0x14,
        /// <summary></summary>
        Kana = 0x15,
        /// <summary></summary>
        Hangeul = 0x15,
        /// <summary></summary>
        Hangul = 0x15,
        /// <summary></summary>
        Junja = 0x17,
        /// <summary></summary>
        Final = 0x18,
        /// <summary></summary>
        Hanja = 0x19,
        /// <summary></summary>
        Kanji = 0x19,
        /// <summary></summary>
        Escape = 0x1B,
        /// <summary></summary>
        Convert = 0x1C,
        /// <summary></summary>
        NonConvert = 0x1D,
        /// <summary></summary>
        Accept = 0x1E,
        /// <summary></summary>
        ModeChange = 0x1F,
        /// <summary></summary>
        Space = 0x20,
        /// <summary></summary>
        Prior = 0x21,
        /// <summary></summary>
        Next = 0x22,
        /// <summary></summary>
        End = 0x23,
        /// <summary></summary>
        Home = 0x24,
        /// <summary></summary>
        Left = 0x25,
        /// <summary></summary>
        Up = 0x26,
        /// <summary></summary>
        Right = 0x27,
        /// <summary></summary>
        Down = 0x28,
        /// <summary></summary>
        Select = 0x29,
        /// <summary></summary>
        Print = 0x2A,
        /// <summary></summary>
        Execute = 0x2B,
        /// <summary></summary>
        Snapshot = 0x2C,
        /// <summary></summary>
        Insert = 0x2D,
        /// <summary></summary>
        Delete = 0x2E,
        /// <summary></summary>
        Help = 0x2F,
        /// <summary></summary>
        N0 = 0x30,
        /// <summary></summary>
        N1 = 0x31,
        /// <summary></summary>
        N2 = 0x32,
        /// <summary></summary>
        N3 = 0x33,
        /// <summary></summary>
        N4 = 0x34,
        /// <summary></summary>
        N5 = 0x35,
        /// <summary></summary>
        N6 = 0x36,
        /// <summary></summary>
        N7 = 0x37,
        /// <summary></summary>
        N8 = 0x38,
        /// <summary></summary>
        N9 = 0x39,
        /// <summary></summary>
        A = 0x41,
        /// <summary></summary>
        B = 0x42,
        /// <summary></summary>
        C = 0x43,
        /// <summary></summary>
        D = 0x44,
        /// <summary></summary>
        E = 0x45,
        /// <summary></summary>
        F = 0x46,
        /// <summary></summary>
        G = 0x47,
        /// <summary></summary>
        H = 0x48,
        /// <summary></summary>
        I = 0x49,
        /// <summary></summary>
        J = 0x4A,
        /// <summary></summary>
        K = 0x4B,
        /// <summary></summary>
        L = 0x4C,
        /// <summary></summary>
        M = 0x4D,
        /// <summary></summary>
        N = 0x4E,
        /// <summary></summary>
        O = 0x4F,
        /// <summary></summary>
        P = 0x50,
        /// <summary></summary>
        Q = 0x51,
        /// <summary></summary>
        R = 0x52,
        /// <summary></summary>
        S = 0x53,
        /// <summary></summary>
        T = 0x54,
        /// <summary></summary>
        U = 0x55,
        /// <summary></summary>
        V = 0x56,
        /// <summary></summary>
        W = 0x57,
        /// <summary></summary>
        X = 0x58,
        /// <summary></summary>
        Y = 0x59,
        /// <summary></summary>
        Z = 0x5A,
        /// <summary></summary>
        LeftWindows = 0x5B,
        /// <summary></summary>
        RightWindows = 0x5C,
        /// <summary></summary>
        Application = 0x5D,
        /// <summary></summary>
        Sleep = 0x5F,
        /// <summary></summary>
        Numpad0 = 0x60,
        /// <summary></summary>
        Numpad1 = 0x61,
        /// <summary></summary>
        Numpad2 = 0x62,
        /// <summary></summary>
        Numpad3 = 0x63,
        /// <summary></summary>
        Numpad4 = 0x64,
        /// <summary></summary>
        Numpad5 = 0x65,
        /// <summary></summary>
        Numpad6 = 0x66,
        /// <summary></summary>
        Numpad7 = 0x67,
        /// <summary></summary>
        Numpad8 = 0x68,
        /// <summary></summary>
        Numpad9 = 0x69,
        /// <summary></summary>
        Multiply = 0x6A,
        /// <summary></summary>
        Add = 0x6B,
        /// <summary></summary>
        Separator = 0x6C,
        /// <summary></summary>
        Subtract = 0x6D,
        /// <summary></summary>
        Decimal = 0x6E,
        /// <summary></summary>
        Divide = 0x6F,
        /// <summary></summary>
        F1 = 0x70,
        /// <summary></summary>
        F2 = 0x71,
        /// <summary></summary>
        F3 = 0x72,
        /// <summary></summary>
        F4 = 0x73,
        /// <summary></summary>
        F5 = 0x74,
        /// <summary></summary>
        F6 = 0x75,
        /// <summary></summary>
        F7 = 0x76,
        /// <summary></summary>
        F8 = 0x77,
        /// <summary></summary>
        F9 = 0x78,
        /// <summary></summary>
        F10 = 0x79,
        /// <summary></summary>
        F11 = 0x7A,
        /// <summary></summary>
        F12 = 0x7B,
        /// <summary></summary>
        F13 = 0x7C,
        /// <summary></summary>
        F14 = 0x7D,
        /// <summary></summary>
        F15 = 0x7E,
        /// <summary></summary>
        F16 = 0x7F,
        /// <summary></summary>
        F17 = 0x80,
        /// <summary></summary>
        F18 = 0x81,
        /// <summary></summary>
        F19 = 0x82,
        /// <summary></summary>
        F20 = 0x83,
        /// <summary></summary>
        F21 = 0x84,
        /// <summary></summary>
        F22 = 0x85,
        /// <summary></summary>
        F23 = 0x86,
        /// <summary></summary>
        F24 = 0x87,
        /// <summary></summary>
        NumLock = 0x90,
        /// <summary></summary>
        ScrollLock = 0x91,
        /// <summary></summary>
        NEC_Equal = 0x92,
        /// <summary></summary>
        Fujitsu_Jisho = 0x92,
        /// <summary></summary>
        Fujitsu_Masshou = 0x93,
        /// <summary></summary>
        Fujitsu_Touroku = 0x94,
        /// <summary></summary>
        Fujitsu_Loya = 0x95,
        /// <summary></summary>
        Fujitsu_Roya = 0x96,
        /// <summary></summary>
        LeftShift = 0xA0,
        /// <summary></summary>
        RightShift = 0xA1,
        /// <summary></summary>
        LeftControl = 0xA2,
        /// <summary></summary>
        RightControl = 0xA3,
        /// <summary></summary>
        LeftMenu = 0xA4,
        /// <summary></summary>
        RightMenu = 0xA5,
        /// <summary></summary>
        BrowserBack = 0xA6,
        /// <summary></summary>
        BrowserForward = 0xA7,
        /// <summary></summary>
        BrowserRefresh = 0xA8,
        /// <summary></summary>
        BrowserStop = 0xA9,
        /// <summary></summary>
        BrowserSearch = 0xAA,
        /// <summary></summary>
        BrowserFavorites = 0xAB,
        /// <summary></summary>
        BrowserHome = 0xAC,
        /// <summary></summary>
        VolumeMute = 0xAD,
        /// <summary></summary>
        VolumeDown = 0xAE,
        /// <summary></summary>
        VolumeUp = 0xAF,
        /// <summary></summary>
        MediaNextTrack = 0xB0,
        /// <summary></summary>
        MediaPrevTrack = 0xB1,
        /// <summary></summary>
        MediaStop = 0xB2,
        /// <summary></summary>
        MediaPlayPause = 0xB3,
        /// <summary></summary>
        LaunchMail = 0xB4,
        /// <summary></summary>
        LaunchMediaSelect = 0xB5,
        /// <summary></summary>
        LaunchApplication1 = 0xB6,
        /// <summary></summary>
        LaunchApplication2 = 0xB7,
        /// <summary></summary>
        OEM1 = 0xBA,
        /// <summary></summary>
        OEMPlus = 0xBB,
        /// <summary></summary>
        OEMComma = 0xBC,
        /// <summary></summary>
        OEMMinus = 0xBD,
        /// <summary></summary>
        OEMPeriod = 0xBE,
        /// <summary></summary>
        OEM2 = 0xBF,
        /// <summary></summary>
        OEM3 = 0xC0,
        /// <summary></summary>
        OEM4 = 0xDB,
        /// <summary></summary>
        OEM5 = 0xDC,
        /// <summary></summary>
        OEM6 = 0xDD,
        /// <summary></summary>
        OEM7 = 0xDE,
        /// <summary></summary>
        OEM8 = 0xDF,
        /// <summary></summary>
        OEMAX = 0xE1,
        /// <summary></summary>
        OEM102 = 0xE2,
        /// <summary></summary>
        ICOHelp = 0xE3,
        /// <summary></summary>
        ICO00 = 0xE4,
        /// <summary></summary>
        ProcessKey = 0xE5,
        /// <summary></summary>
        ICOClear = 0xE6,
        /// <summary></summary>
        Packet = 0xE7,
        /// <summary></summary>
        OEMReset = 0xE9,
        /// <summary></summary>
        OEMJump = 0xEA,
        /// <summary></summary>
        OEMPA1 = 0xEB,
        /// <summary></summary>
        OEMPA2 = 0xEC,
        /// <summary></summary>
        OEMPA3 = 0xED,
        /// <summary></summary>
        OEMWSCtrl = 0xEE,
        /// <summary></summary>
        OEMCUSel = 0xEF,
        /// <summary></summary>
        OEMATTN = 0xF0,
        /// <summary></summary>
        OEMFinish = 0xF1,
        /// <summary></summary>
        OEMCopy = 0xF2,
        /// <summary></summary>
        OEMAuto = 0xF3,
        /// <summary></summary>
        OEMENLW = 0xF4,
        /// <summary></summary>
        OEMBackTab = 0xF5,
        /// <summary></summary>
        ATTN = 0xF6,
        /// <summary></summary>
        CRSel = 0xF7,
        /// <summary></summary>
        EXSel = 0xF8,
        /// <summary></summary>
        EREOF = 0xF9,
        /// <summary></summary>
        Play = 0xFA,
        /// <summary></summary>
        Zoom = 0xFB,
        /// <summary></summary>
        Noname = 0xFC,
        /// <summary></summary>
        PA1 = 0xFD,
        /// <summary></summary>
        OEMClear = 0xFE
    }

    /// <summary>
    /// Vehicle visibility
    /// </summary>
    public enum Visibility
    {
        Name = 0,
        Icon,
        Radar,
        Model,
        Collision
    };

    /// <summary>
    /// Vehicle seat
    /// </summary>
    public enum VehicleSeat
    {
        FrontLeft = 0,
        FrontRight,
        RearLeft,
        RearRight,
        None
    };

    /// <summary>
    /// Vehicle Lock State
    /// </summary>
    public enum VehicleLockState
    {
        Unlocked = 0,
        Locked
    };

    /// <summary>
    /// Vehicle Enter State
    /// </summary>
    public enum VehicleEnterState
    {
        Leave = 0,
        Enter
    }

    /// <summary>
    /// Oakwood Fade Type
    /// </summary>
    public enum OakwoodFade
    {
        FadeOut = 0,
        FadeIn
    };

    /// <summary>
    /// Simple Vector3 used for Oakwood
    /// </summary>
    public struct OakVec3
    {
        public float x, y, z;

        public OakVec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        static float Lerp(float first, float second, float by)
        {
            return first * (1 - by) + second * by;
        }

        public static OakVec3 Lerp(OakVec3 first, OakVec3 second, float by)
        {
            float retX = Lerp(first.x, second.x, by);
            float retY = Lerp(first.y, second.y, by);
            float retZ = Lerp(first.z, second.z, by);
            return new OakVec3(retX, retY, retZ);
        }

        public static OakVec3 operator -(OakVec3 vec)
        {
            return new OakVec3(-vec.x, -vec.y, -vec.z);
        }

        public static OakVec3 operator *(OakVec3 vec, float scalar)
        {
            return new OakVec3(vec.x * scalar, vec.y * scalar, vec.z * scalar);
        }

        public static OakVec3 operator *(OakVec3 vec1, OakVec3 vec2)
        {
            return new OakVec3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
        }

        public static OakVec3 operator /(OakVec3 vec, float scalar)
        {
            return new OakVec3(vec.x / scalar, vec.y / scalar, vec.z / scalar);
        }

        public static OakVec3 operator /(OakVec3 vec1, OakVec3 vec2)
        {
            return new OakVec3(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
        }

        public static OakVec3 operator +(OakVec3 vec1, OakVec3 vec2)
        {
            return new OakVec3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
        }

        public static OakVec3 operator -(OakVec3 vec1, OakVec3 vec2)
        {
            return new OakVec3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
        }

        public static float Distance(OakVec3 vector1, OakVec3 vector2)
        {
            return (float)Math.Sqrt(
                Math.Pow(vector1.x - vector2.x, 2) +
                Math.Pow(vector1.y - vector2.y, 2) +
                Math.Pow(vector1.z - vector2.z, 2)
            );
        }
    }

    /// <summary>
    /// Player model
    /// </summary>
    public class OakwoodPlayerModel
    {
        /// <summary>
        /// Player simplified model name
        /// </summary>
        public string Name;
        /// <summary>
        /// Player full model name
        /// </summary>
        public string Modelname;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Player simplified model name</param>
        /// <param name="model">Player full model name</param>
        public OakwoodPlayerModel(string name, string model)
        {
            Name = name;
            Modelname = model;
        }
    }

    /// <summary>
    /// Vehicle model
    /// </summary>
    public class OakwoodVehicleModel
    {
        /// <summary>
        /// Vehicle full name
        /// </summary>
        public string Name;
        /// <summary>
        /// Vehicle model file name
        /// </summary>
        public string Modelname;

        /// <summary>
        /// Just a constructor
        /// </summary>
        /// <param name="name">Vehicle full name</param>
        /// <param name="model">Vehicle model file name</param>
        public OakwoodVehicleModel(string name, string model)
        {
            Name = name;
            Modelname = model;
        }
    }

    /// <summary>
    /// Oakwood resources (Vehicles, PlayerModels)
    /// </summary>
    public class OakwoodResources
    {
        #region Player Models
        /// <summary>
        /// Player Models
        /// </summary>
        public static OakwoodPlayerModel[] PlayerModels { get; private set; } =
        {
            new OakwoodPlayerModel("Tommy", "Tommy.i3d"),
            new OakwoodPlayerModel("TommyBOXER", "TommyBOXER.i3d"),
            new OakwoodPlayerModel("TommyCOATHAT", "TommyCOATHAT.i3d"),
            new OakwoodPlayerModel("TommyDELNIK", "TommyDELNIK.i3d"),
            new OakwoodPlayerModel("TommyFREERIDER", "TommyFREERIDER.i3d"),
            new OakwoodPlayerModel("TommyGUN", "TommyGUN.i3d"),
            new OakwoodPlayerModel("TommyHAT", "TommyHAT.i3d"),
            new OakwoodPlayerModel("TommyNAHAC", "TommyNAHAC.i3d"),
            new OakwoodPlayerModel("TommyOLD", "TommyOLD.i3d"),
            new OakwoodPlayerModel("TommyPYTEL", "TommyPYTEL.i3d"),
            new OakwoodPlayerModel("TommyRACER", "TommyRACER.i3d"),
            new OakwoodPlayerModel("TommyRACER2", "TommyRACER2.i3d"),
            new OakwoodPlayerModel("TommyRUKAV", "TommyRUKAV.i3d"),
            new OakwoodPlayerModel("TommySAILOR", "TommySAILOR.i3d"),
            new OakwoodPlayerModel("TommyTAXIDRIVER", "TommyTAXIDRIVER.i3d"),
            new OakwoodPlayerModel("AsisPZ1", "AsisPZ1.i3d"),
            new OakwoodPlayerModel("Barman01", "Barman01.i3d"),
            new OakwoodPlayerModel("Bclerk01", "Bclerk01.i3d"),
            new OakwoodPlayerModel("Bclerk02", "Bclerk02.i3d"),
            new OakwoodPlayerModel("Bguard01", "Bguard01.i3d"),
            new OakwoodPlayerModel("Bguard02", "Bguard02.i3d"),
            new OakwoodPlayerModel("Bguard03", "Bguard03.i3d"),
            new OakwoodPlayerModel("Biff", "Biff.i3d"),
            new OakwoodPlayerModel("BigDig", "BigDig.i3d"),
            new OakwoodPlayerModel("BnkO01", "BnkO01.i3d"),
            new OakwoodPlayerModel("BnkO02", "BnkO02.i3d"),
            new OakwoodPlayerModel("BnkO03", "BnkO03.i3d"),
            new OakwoodPlayerModel("BobAut01", "BobAut01.i3d"),
            new OakwoodPlayerModel("Bookmaker01", "Bookmaker01.i3d"),
            new OakwoodPlayerModel("Bookmaker02", "Bookmaker02.i3d"),
            new OakwoodPlayerModel("Boxer01", "Boxer01.i3d"),
            new OakwoodPlayerModel("Boxer02", "Boxer02.i3d"),
            new OakwoodPlayerModel("Boxer03", "Boxer03.i3d"),
            new OakwoodPlayerModel("Boxer04", "Boxer04.i3d"),
            new OakwoodPlayerModel("Carlo", "Carlo.i3d"),
            new OakwoodPlayerModel("Chulig1", "Chulig1.i3d"),
            new OakwoodPlayerModel("Chulig1b", "Chulig1b.i3d"),
            new OakwoodPlayerModel("David", "David.i3d"),
            new OakwoodPlayerModel("Delnik01", "Delnik01.i3d"),
            new OakwoodPlayerModel("Delnik02", "Delnik02.i3d"),
            new OakwoodPlayerModel("Delnik03", "Delnik03.i3d"),
            new OakwoodPlayerModel("Detektiv01", "Detektiv01.i3d"),
            new OakwoodPlayerModel("Detektiv02", "Detektiv02.i3d"),
            new OakwoodPlayerModel("Detektiv03", "Detektiv03.i3d"),
            new OakwoodPlayerModel("Enemy01+", "Enemy01+.i3d"),
            new OakwoodPlayerModel("Enemy02+", "Enemy02+.i3d"),
            new OakwoodPlayerModel("Enemy03+", "Enemy03+.i3d"),
            new OakwoodPlayerModel("Enemy04", "Enemy04.i3d"),
            new OakwoodPlayerModel("Enemy04K", "Enemy04K.i3d"),
            new OakwoodPlayerModel("Enemy05", "Enemy05.i3d"),
            new OakwoodPlayerModel("Enemy07+", "Enemy07+.i3d"),
            new OakwoodPlayerModel("Enemy08+", "Enemy08+.i3d"),
            new OakwoodPlayerModel("Enemy08", "Enemy08.i3d"),
            new OakwoodPlayerModel("Enemy08K", "Enemy08K.i3d"),
            new OakwoodPlayerModel("Enemy09+", "Enemy09+.i3d"),
            new OakwoodPlayerModel("Enemy09", "Enemy09.i3d"),
            new OakwoodPlayerModel("Enemy09K", "Enemy09K.i3d"),
            new OakwoodPlayerModel("Enemy10+", "Enemy10+.i3d"),
            new OakwoodPlayerModel("Enemy10", "Enemy10.i3d"),
            new OakwoodPlayerModel("Enemy10K", "Enemy10K.i3d"),
            new OakwoodPlayerModel("Enemy11K", "Enemy11K.i3d"),
            new OakwoodPlayerModel("Enemy12", "Enemy12.i3d"),
            new OakwoodPlayerModel("Enemy12K", "Enemy12K.i3d"),
            new OakwoodPlayerModel("Enemy13C", "Enemy13C.i3d"),
            new OakwoodPlayerModel("Enemy91", "Enemy91.i3d"),
            new OakwoodPlayerModel("Enemy92", "Enemy92.i3d"),
            new OakwoodPlayerModel("FMVENemy11K", "FMVENemy11K.i3d"),
            new OakwoodPlayerModel("FREEgang01", "FREEgang01.i3d"),
            new OakwoodPlayerModel("FREEgang02", "FREEgang02.i3d"),
            new OakwoodPlayerModel("Friend1", "Friend1.i3d"),
            new OakwoodPlayerModel("Gangster03", "Gangster03.i3d"),
            new OakwoodPlayerModel("GodzMan1", "GodzMan1.i3d"),
            new OakwoodPlayerModel("Guard01", "Guard01.i3d"),
            new OakwoodPlayerModel("Guard02", "Guard02.i3d"),
            new OakwoodPlayerModel("Hasic01", "Hasic01.i3d"),
            new OakwoodPlayerModel("HighCivil", "HighCivil.i3d"),
            new OakwoodPlayerModel("HighCivilBLOOD", "HighCivilBLOOD.i3d"),
            new OakwoodPlayerModel("Homeless01", "Homeless01.i3d"),
            new OakwoodPlayerModel("Hoolig01", "Hoolig01.i3d"),
            new OakwoodPlayerModel("Hoolig02", "Hoolig02.i3d"),
            new OakwoodPlayerModel("Hoolig03", "Hoolig03.i3d"),
            new OakwoodPlayerModel("Hoolig04", "Hoolig04.i3d"),
            new OakwoodPlayerModel("Hoolig05", "Hoolig05.i3d"),
            new OakwoodPlayerModel("Hoolig06", "Hoolig06.i3d"),
            new OakwoodPlayerModel("I04Delnik01", "I04Delnik01.i3d"),
            new OakwoodPlayerModel("Joe", "Joe.i3d"),
            new OakwoodPlayerModel("Kasar", "Kasar.i3d"),
            new OakwoodPlayerModel("Knez", "Knez.i3d"),
            new OakwoodPlayerModel("LifeG01", "LifeG01.i3d"),
            new OakwoodPlayerModel("Lucas", "Lucas.i3d"),
            new OakwoodPlayerModel("Luigi", "Luigi.i3d"),
            new OakwoodPlayerModel("Malticka1", "Malticka1.i3d"),
            new OakwoodPlayerModel("MorelloLOW", "MorelloLOW.i3d"),
            new OakwoodPlayerModel("NormanHIGH", "NormanHIGH.i3d"),
            new OakwoodPlayerModel("Organizator01", "Organizator01.i3d"),
            new OakwoodPlayerModel("Paulie", "Paulie.i3d"),
            new OakwoodPlayerModel("PaulieCOATHAT", "PaulieCOATHAT.i3d"),
            new OakwoodPlayerModel("Pepe", "Pepe.i3d"),
            new OakwoodPlayerModel("PoliceMan01", "PoliceMan01.i3d"),
            new OakwoodPlayerModel("PoliceMan02", "PoliceMan02.i3d"),
            new OakwoodPlayerModel("Politik", "Politik.i3d"),
            new OakwoodPlayerModel("PortGuard01", "PortGuard01.i3d"),
            new OakwoodPlayerModel("PortGuard02", "PortGuard02.i3d"),
            new OakwoodPlayerModel("Prokur", "Prokur.i3d"),
            new OakwoodPlayerModel("Radni01", "Radni01.i3d"),
            new OakwoodPlayerModel("Radni02", "Radni02.i3d"),
            new OakwoodPlayerModel("Ralph", "Ralph.i3d"),
            new OakwoodPlayerModel("RalphHIGH", "RalphHIGH.i3d"),
            new OakwoodPlayerModel("ReditelB", "ReditelB.i3d"),
            new OakwoodPlayerModel("ReditelH", "ReditelH.i3d"),
            new OakwoodPlayerModel("RidicNakladaku", "RidicNakladaku.i3d"),
            new OakwoodPlayerModel("SalMan01K", "SalMan01K.i3d"),
            new OakwoodPlayerModel("SalMan02K", "SalMan02K.i3d"),
            new OakwoodPlayerModel("SalMan03", "SalMan03.i3d"),
            new OakwoodPlayerModel("SalMan03K", "SalMan03K.i3d"),
            new OakwoodPlayerModel("SalMan04", "SalMan04.i3d"),
            new OakwoodPlayerModel("SalMan05", "SalMan05.i3d"),
            new OakwoodPlayerModel("SalMan05K", "SalMan05K.i3d"),
            new OakwoodPlayerModel("Salieri2", "Salieri2.i3d"),
            new OakwoodPlayerModel("SalieriLOW", "SalieriLOW.i3d"),
            new OakwoodPlayerModel("Sam", "Sam.i3d"),
            new OakwoodPlayerModel("SamCOATHAT", "SamCOATHAT.i3d"),
            new OakwoodPlayerModel("Samblood1", "Samblood1.i3d"),
            new OakwoodPlayerModel("Sergio", "Sergio.i3d"),
            new OakwoodPlayerModel("SergioBLOOD", "SergioBLOOD.i3d"),
            new OakwoodPlayerModel("SynRad1", "SynRad1.i3d"),
            new OakwoodPlayerModel("SynRad1BLOOD", "SynRad1BLOOD.i3d"),
            new OakwoodPlayerModel("SynRad1DEAD", "SynRad1DEAD.i3d"),
            new OakwoodPlayerModel("Tony", "Tony.i3d"),
            new OakwoodPlayerModel("VincenzoLOW", "VincenzoLOW.i3d"),
            new OakwoodPlayerModel("Vrabec", "Vrabec.i3d"),
            new OakwoodPlayerModel("Vratny1", "Vratny1.i3d"),
            new OakwoodPlayerModel("Vypravci", "Vypravci.i3d"),
            new OakwoodPlayerModel("Vypravci2", "Vypravci2.i3d"),
            new OakwoodPlayerModel("WillG2", "WillG2.i3d"),
            new OakwoodPlayerModel("WillMan01", "WillMan01.i3d"),
            new OakwoodPlayerModel("WillMan02", "WillMan02.i3d"),
            new OakwoodPlayerModel("Zavod1", "Zavod1.i3d"),
            new OakwoodPlayerModel("Zavod2", "Zavod2.i3d"),
            new OakwoodPlayerModel("Zavod3", "Zavod3.i3d"),
            new OakwoodPlayerModel("ZavodFMV1", "ZavodFMV1.i3d"),
            new OakwoodPlayerModel("ZavodFMV2", "ZavodFMV2.i3d"),
            new OakwoodPlayerModel("civil02", "civil02.i3d"),
            new OakwoodPlayerModel("civil03", "civil03.i3d"),
            new OakwoodPlayerModel("civil04", "civil04.i3d"),
            new OakwoodPlayerModel("civil05", "civil05.i3d"),
            new OakwoodPlayerModel("civil06", "civil06.i3d"),
            new OakwoodPlayerModel("civil11", "civil11.i3d"),
            new OakwoodPlayerModel("civil12", "civil12.i3d"),
            new OakwoodPlayerModel("civil13", "civil13.i3d"),
            new OakwoodPlayerModel("civil14", "civil14.i3d"),
            new OakwoodPlayerModel("civil15", "civil15.i3d"),
            new OakwoodPlayerModel("civil16", "civil16.i3d"),
            new OakwoodPlayerModel("civil17", "civil17.i3d"),
            new OakwoodPlayerModel("civil18", "civil18.i3d"),
            new OakwoodPlayerModel("civil19", "civil19.i3d"),
            new OakwoodPlayerModel("civil21", "civil21.i3d"),
            new OakwoodPlayerModel("civil22", "civil22.i3d"),
            new OakwoodPlayerModel("civil31", "civil31.i3d"),
            new OakwoodPlayerModel("civil32", "civil32.i3d"),
            new OakwoodPlayerModel("civil33", "civil33.i3d"),
            new OakwoodPlayerModel("civil34", "civil34.i3d"),
            new OakwoodPlayerModel("civil35", "civil35.i3d"),
            new OakwoodPlayerModel("civil36", "civil36.i3d"),
            new OakwoodPlayerModel("civil37", "civil37.i3d"),
            new OakwoodPlayerModel("civil38", "civil38.i3d"),
            new OakwoodPlayerModel("civil39", "civil39.i3d"),
            new OakwoodPlayerModel("civil40", "civil40.i3d"),
            new OakwoodPlayerModel("civil41", "civil41.i3d"),
            new OakwoodPlayerModel("civil42", "civil42.i3d"),
            new OakwoodPlayerModel("civil43", "civil43.i3d"),
            new OakwoodPlayerModel("civil44", "civil44.i3d"),
            new OakwoodPlayerModel("civil51", "civil51.i3d"),
            new OakwoodPlayerModel("civil52", "civil52.i3d"),
            new OakwoodPlayerModel("civil53", "civil53.i3d"),
            new OakwoodPlayerModel("civil54", "civil54.i3d"),
            new OakwoodPlayerModel("civil55", "civil55.i3d"),
            new OakwoodPlayerModel("civil56", "civil56.i3d"),
            new OakwoodPlayerModel("civil57", "civil57.i3d"),
            new OakwoodPlayerModel("civil60", "civil60.i3d"),
            new OakwoodPlayerModel("civil61", "civil61.i3d"),
            new OakwoodPlayerModel("civil62", "civil62.i3d"),
            new OakwoodPlayerModel("civil63", "civil63.i3d"),
            new OakwoodPlayerModel("civil70", "civil70.i3d"),
            new OakwoodPlayerModel("civil71", "civil71.i3d"),
            new OakwoodPlayerModel("civil72", "civil72.i3d"),
            new OakwoodPlayerModel("frank", "frank.i3d"),
            new OakwoodPlayerModel("ohorelec01", "ohorelec01.i3d"),
            new OakwoodPlayerModel("pianist1", "pianist1.i3d"),
            new OakwoodPlayerModel("pol01", "pol01.i3d"),
            new OakwoodPlayerModel("pol02", "pol02.i3d"),
            new OakwoodPlayerModel("pol03", "pol03.i3d"),
            new OakwoodPlayerModel("pol11", "pol11.i3d"),
            new OakwoodPlayerModel("pol12", "pol12.i3d"),
            new OakwoodPlayerModel("pol13", "pol13.i3d"),
            new OakwoodPlayerModel("polim62", "polim62.i3d"),
            new OakwoodPlayerModel("pumpar01", "pumpar01.i3d"),
            new OakwoodPlayerModel("recep", "recep.i3d"),
            new OakwoodPlayerModel("sailor01", "sailor01.i3d"),
            new OakwoodPlayerModel("sailor02", "sailor02.i3d"),
            new OakwoodPlayerModel("sailor03", "sailor03.i3d"),
            new OakwoodPlayerModel("waiter01", "waiter01.i3d"),
            new OakwoodPlayerModel("waiter02", "waiter02.i3d"),
            new OakwoodPlayerModel("waiter03", "waiter03.i3d"),
            new OakwoodPlayerModel("Alice1", "Alice1.i3d"),
            new OakwoodPlayerModel("Berta", "Berta.i3d"),
            new OakwoodPlayerModel("Bitch01", "Bitch01.i3d"),
            new OakwoodPlayerModel("Bitch02", "Bitch02.i3d"),
            new OakwoodPlayerModel("Bitch02Mask", "Bitch02Mask.i3d"),
            new OakwoodPlayerModel("Bitch03M", "Bitch03M.i3d"),
            new OakwoodPlayerModel("CarlZen1", "CarlZen1.i3d"),
            new OakwoodPlayerModel("Czena01", "Czena01.i3d"),
            new OakwoodPlayerModel("Czena02", "Czena02.i3d"),
            new OakwoodPlayerModel("Czena03", "Czena03.i3d"),
            new OakwoodPlayerModel("Czena04", "Czena04.i3d"),
            new OakwoodPlayerModel("Czena05", "Czena05.i3d"),
            new OakwoodPlayerModel("Czena06", "Czena06.i3d"),
            new OakwoodPlayerModel("Czena07", "Czena07.i3d"),
            new OakwoodPlayerModel("Czena08", "Czena08.i3d"),
            new OakwoodPlayerModel("Czena09", "Czena09.i3d"),
            new OakwoodPlayerModel("Czena10", "Czena10.i3d"),
            new OakwoodPlayerModel("Czena11", "Czena11.i3d"),
            new OakwoodPlayerModel("Czena12", "Czena12.i3d"),
            new OakwoodPlayerModel("Czena13", "Czena13.i3d"),
            new OakwoodPlayerModel("March1", "March1.i3d"),
            new OakwoodPlayerModel("Michelle", "Michelle.i3d"),
            new OakwoodPlayerModel("MichelleLOW", "MichelleLOW.i3d"),
            new OakwoodPlayerModel("Milenka1", "Milenka1.i3d"),
            new OakwoodPlayerModel("Sarah1", "Sarah1.i3d"),
            new OakwoodPlayerModel("Sarah1Obl", "Sarah1Obl.i3d"),
            new OakwoodPlayerModel("Sarah2", "Sarah2.i3d"),
            new OakwoodPlayerModel("ZenaRad01", "ZenaRad01.i3d")
        };
        #endregion

        #region Vehicle Models
        /// <summary>
        /// Vehicle Models
        /// </summary>
        public static OakwoodVehicleModel[] VehicleModels =
        {
            new OakwoodVehicleModel("Bolt Ace Coupe Blue", "fordtco00.i3d"),
            new OakwoodVehicleModel("Bolt Ace Coupe Dark Blue", "fordtco01.i3d"),
            new OakwoodVehicleModel("Bolt Ace Coupe Brown", "fordtco02.i3d"),
            new OakwoodVehicleModel("Bolt Ace Coupe Green", "fordtco03.i3d"),
            new OakwoodVehicleModel("Bolt Ace Coupe Red", "fordtco04.i3d"),

            new OakwoodVehicleModel("Bolt Ace Fordor Blue", "fordtFor00.i3d"),
            new OakwoodVehicleModel("Bolt Ace Fordor Dark Blue", "fordtFor01.i3d"),
            new OakwoodVehicleModel("Bolt Ace Fordor Brown", "fordtFor02.i3d"),
            new OakwoodVehicleModel("Bolt Ace Fordor Green", "fordtFor03.i3d"),
            new OakwoodVehicleModel("Bolt Ace Fordor Red", "fordtFor04.i3d"),

            new OakwoodVehicleModel("Bolt Ace Pickup Blue", "fordtpi00.i3d"),
            new OakwoodVehicleModel("Bolt Ace Pickup Dark Blue", "fordtpi01.i3d"),
            new OakwoodVehicleModel("Bolt Ace Pickup Brown", "fordtpi02.i3d"),
            new OakwoodVehicleModel("Bolt Ace Pickup Green", "fordtpi03.i3d"),
            new OakwoodVehicleModel("Bolt Ace Pickup Red", "fordtpi04.i3d"),

            new OakwoodVehicleModel("Bolt Ace Runabout Blue", "forttru00.i3d"),
            new OakwoodVehicleModel("Bolt Ace Runabout Dark Blue", "forttru01.i3d"),
            new OakwoodVehicleModel("Bolt Ace Runabout Brown", "forttru02.i3d"),
            new OakwoodVehicleModel("Bolt Ace Runabout Green", "forttru03.i3d"),
            new OakwoodVehicleModel("Bolt Ace Runabout Red", "forttru04.i3d"),

            new OakwoodVehicleModel("Bolt Ace Touring Blue", "forttto00.i3d"),
            new OakwoodVehicleModel("Bolt Ace Touring Dark Blue", "forttto01.i3d"),
            new OakwoodVehicleModel("Bolt Ace Touring Brown", "forttto02.i3d"),
            new OakwoodVehicleModel("Bolt Ace Touring Green", "forttto03.i3d"),
            new OakwoodVehicleModel("Bolt Ace Touring Red", "forttto04.i3d"),

            new OakwoodVehicleModel("Bolt Ace Tudor Blue", "fordttud00.i3d"),
            new OakwoodVehicleModel("Bolt Ace Tudor Dark Blue", "fordttud01.i3d"),
            new OakwoodVehicleModel("Bolt Ace Tudor Brown", "fordttud02.i3d"),
            new OakwoodVehicleModel("Bolt Ace Tudor Green", "fordttud03.i3d"),
            new OakwoodVehicleModel("Bolt Ace Tudor Red", "fordttud04.i3d"),

            new OakwoodVehicleModel("Bolt Model B Cabriolet Brown", "ForAca00.i3d"),
            new OakwoodVehicleModel("Bolt Model B Cabriolet Red", "ForAca01.i3d"),
            new OakwoodVehicleModel("Bolt Model B Cabriolet Green", "ForAca02.i3d"),
            new OakwoodVehicleModel("Bolt Model B Cabriolet Dark Blue", "ForAca03.i3d"),

            new OakwoodVehicleModel("Bolt Model B Coupe Brown", "ForAcou00.i3d"),
            new OakwoodVehicleModel("Bolt Model B Coupe Red", "ForAcou01.i3d"),
            new OakwoodVehicleModel("Bolt Model B Coupe Green", "ForAcou02.i3d"),
            new OakwoodVehicleModel("Bolt Model B Coupe Dark Blue", "ForAcou03.i3d"),

            new OakwoodVehicleModel("Bolt Model B Delivery Brown", "ForAde00.i3d"),
            new OakwoodVehicleModel("Bolt Model B Delivery Red", "ForAde01.i3d"),
            new OakwoodVehicleModel("Bolt Model B Delivery Green", "ForAde02.i3d"),
            new OakwoodVehicleModel("Bolt Model B Delivery Dark Blue", "ForAde03.i3d"),

            new OakwoodVehicleModel("Bolt Model B Fordor Brown", "ForAfo00.i3d"),
            new OakwoodVehicleModel("Bolt Model B Fordor Red", "ForAfo01.i3d"),
            new OakwoodVehicleModel("Bolt Model B Fordor Green", "ForAfo02.i3d"),
            new OakwoodVehicleModel("Bolt Model B Fordor Dark Blue", "ForAfo03.i3d"),

            new OakwoodVehicleModel("Bolt Model B Pickup Brown", "ForApic00.i3d"),
            new OakwoodVehicleModel("Bolt Model B Pickup Red", "ForApic01.i3d"),
            new OakwoodVehicleModel("Bolt Model B Pickup Green", "ForApic02.i3d"),
            new OakwoodVehicleModel("Bolt Model B Pickup Dark Blue", "ForApic03.i3d"),

            new OakwoodVehicleModel("Bolt Model B Roadster Brown", "ForAro00.i3d"),
            new OakwoodVehicleModel("Bolt Model B Roadster Red", "ForAro01.i3d"),
            new OakwoodVehicleModel("Bolt Model B Roadster Green", "ForAro02.i3d"),
            new OakwoodVehicleModel("Bolt Model B Roadster Dark Blue", "ForAro03.i3d"),

            new OakwoodVehicleModel("Bolt Model B Tudor Brown", "ForAtu00.i3d"),
            new OakwoodVehicleModel("Bolt Model B Tudor Red", "ForAtu01.i3d"),
            new OakwoodVehicleModel("Bolt Model B Tudor Green", "ForAtu02.i3d"),
            new OakwoodVehicleModel("Bolt Model B Tudor Dark Blue", "ForAtu03.i3d"),

            new OakwoodVehicleModel("Bolt V8 Coupe Green", "forVco00.i3d"),
            new OakwoodVehicleModel("Bolt V8 Coupe Red", "forVco01.i3d"),
            new OakwoodVehicleModel("Bolt V8 Coupe Blue", "forVco02.i3d"),
            new OakwoodVehicleModel("Bolt V8 Coupe Grey", "forVco03.i3d"),

            new OakwoodVehicleModel("Bolt V8 Fordor Green", "forVfor00.i3d"),
            new OakwoodVehicleModel("Bolt V8 Fordor Red", "forVfor01.i3d"),
            new OakwoodVehicleModel("Bolt V8 Fordor Blue", "forVfor02.i3d"),
            new OakwoodVehicleModel("Bolt V8 Fordor Grey", "forVfor03.i3d"),

            new OakwoodVehicleModel("Bolt V8 Roadster Green", "forVro00.i3d"),
            new OakwoodVehicleModel("Bolt V8 Roadster Red", "forVro01.i3d"),
            new OakwoodVehicleModel("Bolt V8 Roadster Blue", "forVro02.i3d"),
            new OakwoodVehicleModel("Bolt V8 Roadster Grey", "forVro03.i3d"),

            new OakwoodVehicleModel("Bolt V8 Touring Green", "forVto00.i3d"),
            new OakwoodVehicleModel("Bolt V8 Touring Red", "forVto01.i3d"),
            new OakwoodVehicleModel("Bolt V8 Touring Blue", "forVto02.i3d"),
            new OakwoodVehicleModel("Bolt V8 Touring Grey", "forVto03.i3d"),

            new OakwoodVehicleModel("Bolt V8 Tudor Green", "forVtud00.i3d"),
            new OakwoodVehicleModel("Bolt V8 Tudor Red", "forVtud01.i3d"),
            new OakwoodVehicleModel("Bolt V8 Tudor Blue", "forVtud02.i3d"),
            new OakwoodVehicleModel("Bolt V8 Tudor Grey", "forVtud03.i3d"),

            new OakwoodVehicleModel("Brubaker 4WD", "miller00.i3d"),

            new OakwoodVehicleModel("Bruno Speedster 851 Silver", "speedster00.i3d"),
            new OakwoodVehicleModel("Bruno Speedster 851 Red", "speedster01.i3d"),
            new OakwoodVehicleModel("Bruno Speedster 851 Green", "speedster02.i3d"),

            new OakwoodVehicleModel("Caesar 8C 2300 Racing", "alfa00.i3d"),
            new OakwoodVehicleModel("Caesar 8C Mostro Red", "alfa8C00.i3d"),
            new OakwoodVehicleModel("Caesar 8C Mostro Black", "alfa8C01.i3d"),

            new OakwoodVehicleModel("Celeste Marque 500 White", "merced500K00.i3d"),
            new OakwoodVehicleModel("Celeste Marque 500 Brown", "merced500K01.i3d"),

            new OakwoodVehicleModel("Corrozella C-Otto 4WD Blue", "bugatti00.i3d"),
            new OakwoodVehicleModel("Corrozella C-Otto 4WD Green", "bugatti01.i3d"),

            new OakwoodVehicleModel("Crusader Chromim Fordor Blue", "pontFor00.i3d"),
            new OakwoodVehicleModel("Crusader Chromim Fordor Pink", "pontFor01.i3d"),
            new OakwoodVehicleModel("Crusader Chromim Tudor Blue", "pontTud00.i3d"),
            new OakwoodVehicleModel("Crusader Chromim Tudor Pink", "pontTud01.i3d"),

            new OakwoodVehicleModel("Falconer Blue", "blackha00.i3d"),
            new OakwoodVehicleModel("Falconer Red", "blackha01.i3d"),
            new OakwoodVehicleModel("Falconer Gangster", "black00.i3d"),
            new OakwoodVehicleModel("Falconer Yellowcar", "taxi00.i3d"),

            new OakwoodVehicleModel("Guardian Terraplane Coupe Pink", "hudcou00.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Coupe Beige", "hudcou01.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Coupe Black", "hudcou02.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Fordor Pink", "hudfor00.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Fordor Beige", "hudfor01.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Fordor Black", "hudfor02.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Tudor Pink", "hudtu00.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Tudor Beige", "hudtu01.i3d"),
            new OakwoodVehicleModel("Guardian Terraplane Tudor Black", "hudtu02.i3d"),

            new OakwoodVehicleModel("Lassister V16 Appolon", "hartmann00.i3d"),
            new OakwoodVehicleModel("Lassister V16 Charron", "hearseCa00.i3d"),
            new OakwoodVehicleModel("Lassister V16 Fordor", "cad_ford00.i3d"),
            new OakwoodVehicleModel("Lassister V16 Phaeton", "cad_phaeton00.i3d"),
            new OakwoodVehicleModel("Lassister V16 Police", "polCad00.i3d"),
            new OakwoodVehicleModel("Lassister V16 Roadster", "cad_road00.i3d"),

            new OakwoodVehicleModel("Schubert Extra Six Fordor Green", "chemaFor00.i3d"),
            new OakwoodVehicleModel("Schubert Extra Six Fordor Blue", "chemaFor01.i3d"),
            new OakwoodVehicleModel("Schubert Extra Six Fordor White", "chemaFor02.i3d"),
            new OakwoodVehicleModel("Schubert Extra Six Fordor Police", "polimFor00.i3d"),
            new OakwoodVehicleModel("Schubert Extra Six Tudor Green", "chematud00.i3d"),
            new OakwoodVehicleModel("Schubert Extra Six Tudor Blue", "chematud01.i3d"),
            new OakwoodVehicleModel("Schubert Extra Six Tudor White", "chematud02.i3d"),
            new OakwoodVehicleModel("Schubert Extra Six Tudor Police", "polimTud00.i3d"),

            new OakwoodVehicleModel("Schubert Six Red", "chev00.i3d"),
            new OakwoodVehicleModel("Schubert Six White", "chev01.i3d"),
            new OakwoodVehicleModel("Schubert Six Black", "chev02.i3d"),
            new OakwoodVehicleModel("Schubert Six Police", "poli00.i3d"),

            new OakwoodVehicleModel("Silver Fletcher", "arrow00.i3d"),

            new OakwoodVehicleModel("Thor 810 Phaeton FWD Orange", "cordph00.i3d"),
            new OakwoodVehicleModel("Thor 810 Phaeton FWD Black", "cordph01.i3d"),
            new OakwoodVehicleModel("Thor 810 Sedan FWD Orange", "cordse00.i3d"),
            new OakwoodVehicleModel("Thor 810 Sedan FWD Black", "cordse01.i3d"),
            new OakwoodVehicleModel("Thor 810 Cabriolet FWD Orange", "cordca00.i3d"),
            new OakwoodVehicleModel("Thor 810 Cabriolet FWD Black", "cordca01.i3d"),

            new OakwoodVehicleModel("Trautenberg Model J", "deuseJco00.i3d"),
            new OakwoodVehicleModel("Trautenberg Racer 4WD", "duesenberg00.i3d"),

            new OakwoodVehicleModel("Ulver Airstream Fordor Yellow", "airflFor00.i3d"),
            new OakwoodVehicleModel("Ulver Airstream Fordor Blue", "airflFor01.i3d"),
            new OakwoodVehicleModel("Ulver Airstream Tudor Yellow", "airfltud00.i3d"),
            new OakwoodVehicleModel("Ulver Airstream Tudor Blue", "airfltud01.i3d"),

            new OakwoodVehicleModel("Wright Coupe Blue", "buiCou00.i3d"),
            new OakwoodVehicleModel("Wright Coupe Red", "buiCou01.i3d"),
            new OakwoodVehicleModel("Wright Coupe Gangster", "buigang00.i3d"),
            new OakwoodVehicleModel("Wright Fordor Blue", "buikFor00.i3d"),
            new OakwoodVehicleModel("Wright Fordor Red", "buikFor01.i3d"),

            new OakwoodVehicleModel("Bolt Ambulance", "ambulance00.i3d"),
            new OakwoodVehicleModel("Bolt Firetruck", "fire00.i3d"),
            new OakwoodVehicleModel("Bolt Hearse", "hearseA00.i3d"),
            new OakwoodVehicleModel("Bolt Truck Flatbed", "truckA00.i3d"),
            new OakwoodVehicleModel("Bolt Truck Covered", "truckB00.i3d"),
            new OakwoodVehicleModel("Bolt Truck", "truckBx00.i3d"),
            
            new OakwoodVehicleModel("Manta Prototype", "phantom00.i3d"),
            new OakwoodVehicleModel("Black Dragon 4WD", "blackdragon00.i3d"),
            new OakwoodVehicleModel("Black Metal 4WD", "chevroletm6H00.i3d"),
            new OakwoodVehicleModel("Bob Mylan 4WD", "hotrodp200.i3d"),
            new OakwoodVehicleModel("Bolt-Thrower", "Thunderbird00.i3d"),
            new OakwoodVehicleModel("Crazy Horse", "FThot00.i3d"),
            new OakwoodVehicleModel("Demoniac", "fordTH00.i3d"),
            new OakwoodVehicleModel("Discorder 4WD", "hodrodp300.i3d"),
            new OakwoodVehicleModel("Flame Spear 4WD", "hodrodp600.i3d"),
            new OakwoodVehicleModel("Flamer", "Flamer00.i3d"),
            new OakwoodVehicleModel("Flower Power", "fordAdelH00.i3d"),
            new OakwoodVehicleModel("Hillbilly 5.1 FWD", "Tbirdold00.i3d"),
            new OakwoodVehicleModel("HotRod", "FordHOT00.i3d"),
            new OakwoodVehicleModel("Luciferon FWD", "hotrodp500.i3d"),
            new OakwoodVehicleModel("Manta Taxi FWD", "phantomtaxi00.i3d"),
            new OakwoodVehicleModel("Masseur", "fordApick00.i3d"),
            new OakwoodVehicleModel("Masseur Taxi", "fordApickTaxi00.i3d"),
            new OakwoodVehicleModel("Mutagen FWD", "cord_sedanH00.i3d"),
            new OakwoodVehicleModel("Speedee 4WD", "hotrodp400.i3d"),

            new OakwoodVehicleModel("Beatle Custom", "brouk00.i3d"),
            new OakwoodVehicleModel("Bulldozer", "Bull00.i3d"),
            new OakwoodVehicleModel("Bus", "bus00.i3d"),
            new OakwoodVehicleModel("Monster Truck", "Bigfoot00.i3d"),
            new OakwoodVehicleModel("Monster Truck American Flag", "Bigfoot.i3d"),
            new OakwoodVehicleModel("Moto", "9motorka.i3d"),
            new OakwoodVehicleModel("Toyota 4x4 Concept Car", "toyota00.i3d"),
            new OakwoodVehicleModel("Traktor", "traktor.i3d"),
            new OakwoodVehicleModel("Xeon", "xedos00.i3d"),
        };
        #endregion
    }
}
