using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.CharSet;
using static System.Runtime.InteropServices.UnmanagedType;

#region Enum
public enum IDC : int
{
    ARROW = 32512,
    IBEAM = 32513,
    WAIT = 32514,
    CROSS = 32515,
    UPARROW = 32516,
    SIZE = 32640,
    ICON = 32641,
    SIZENWSE = 32642,
    SIZENESW = 32643,
    SIZEWE = 32644,
    SIZENS = 32645,
    SIZEALL = 32646,
    NO = 32648,
    HAND = 32649,
    APPSTARTING = 32650,
    HELP = 32651
}
public enum WM : uint
{
    ACTIVATE = 0x0006,
    ACTIVATEAPP = 0x001C,
    AFXFIRST = 0x0360,
    AFXLAST = 0x037F,
    APP = 0x8000,
    ASKCBFORMATNAME = 0x030C,
    CANCELJOURNAL = 0x004B,
    CANCELMODE = 0x001F,
    CAPTURECHANGED = 0x0215,
    CHANGECBCHAIN = 0x030D,
    CHANGEUISTATE = 0x0127,
    CHAR = 0x0102,
    CHARTOITEM = 0x002F,
    CHILDACTIVATE = 0x0022,
    CLEAR = 0x0303,
    CLOSE = 0x0010,
    CLIPBOARDUPDATE = 0x031D,
    COMMAND = 0x0111,
    COMPACTING = 0x0041,
    COMPAREITEM = 0x0039,
    CONTEXTMENU = 0x007B,
    COPY = 0x0301,
    COPYDATA = 0x004A,
    CREATE = 0x0001,
    CTLCOLORBTN = 0x0135,
    CTLCOLORDLG = 0x0136,
    CTLCOLOREDIT = 0x0133,
    CTLCOLORLISTBOX = 0x0134,
    CTLCOLORMSGBOX = 0x0132,
    CTLCOLORSCROLLBAR = 0x0137,
    CTLCOLORSTATIC = 0x0138,
    CUT = 0x0300,
    DEADCHAR = 0x0103,
    DELETEITEM = 0x002D,
    DESTROY = 0x0002,
    DESTROYCLIPBOARD = 0x0307,
    DEVICECHANGE = 0x0219,
    DEVMODECHANGE = 0x001B,
    DISPLAYCHANGE = 0x007E,
    DRAWCLIPBOARD = 0x0308,
    DRAWITEM = 0x002B,
    DROPFILES = 0x0233,
    ENABLE = 0x000A,
    ENDSESSION = 0x0016,
    ENTERIDLE = 0x0121,
    ENTERMENULOOP = 0x0211,
    ENTERSIZEMOVE = 0x0231,
    ERASEBKGND = 0x0014,
    EXITMENULOOP = 0x0212,
    EXITSIZEMOVE = 0x0232,
    FONTCHANGE = 0x001D,
    GETDLGCODE = 0x0087,
    GETFONT = 0x0031,
    GETHOTKEY = 0x0033,
    GETICON = 0x007F,
    GETMINMAXINFO = 0x0024,
    GETOBJECT = 0x003D,
    GETTEXT = 0x000D,
    GETTEXTLENGTH = 0x000E,
    HANDHELDFIRST = 0x0358,
    HANDHELDLAST = 0x035F,
    HELP = 0x0053,
    HOTKEY = 0x0312,
    HSCROLL = 0x0114,
    HSCROLLCLIPBOARD = 0x030E,
    ICONERASEBKGND = 0x0027,
    IME_CHAR = 0x0286,
    IME_COMPOSITION = 0x010F,
    IME_COMPOSITIONFULL = 0x0284,
    IME_CONTROL = 0x0283,
    IME_ENDCOMPOSITION = 0x010E,
    IME_KEYDOWN = 0x0290,
    IME_KEYLAST = 0x010F,
    IME_KEYUP = 0x0291,
    IME_NOTIFY = 0x0282,
    IME_REQUEST = 0x0288,
    IME_SELECT = 0x0285,
    IME_SETCONTEXT = 0x0281,
    IME_STARTCOMPOSITION = 0x010D,
    INITDIALOG = 0x0110,
    INITMENU = 0x0116,
    INITMENUPOPUP = 0x0117,
    INPUTLANGCHANGE = 0x0051,
    INPUTLANGCHANGEREQUEST = 0x0050,
    KEYDOWN = 0x0100,
    KEYFIRST = 0x0100,
    KEYLAST = 0x0108,
    KEYUP = 0x0101,
    KILLFOCUS = 0x0008,
    LBUTTONDBLCLK = 0x0203,
    LBUTTONDOWN = 0x0201,
    LBUTTONUP = 0x0202,
    MBUTTONDBLCLK = 0x0209,
    MBUTTONDOWN = 0x0207,
    MBUTTONUP = 0x0208,
    MDIACTIVATE = 0x0222,
    MDICASCADE = 0x0227,
    MDICREATE = 0x0220,
    MDIDESTROY = 0x0221,
    MDIGETACTIVE = 0x0229,
    MDIICONARRANGE = 0x0228,
    MDIMAXIMIZE = 0x0225,
    MDINEXT = 0x0224,
    MDIREFRESHMENU = 0x0234,
    MDIRESTORE = 0x0223,
    MDISETMENU = 0x0230,
    MDITILE = 0x0226,
    MEASUREITEM = 0x002C,
    MENUCHAR = 0x0120,
    MENUCOMMAND = 0x0126,
    MENUDRAG = 0x0123,
    MENUGETOBJECT = 0x0124,
    MENURBUTTONUP = 0x0122,
    MENUSELECT = 0x011F,
    MOUSEACTIVATE = 0x0021,
    MOUSEFIRST = 0x0200,
    MOUSEHOVER = 0x02A1,
    MOUSELAST = 0x020D,
    MOUSELEAVE = 0x02A3,
    MOUSEMOVE = 0x0200,
    MOUSEWHEEL = 0x020A,
    MOUSEHWHEEL = 0x020E,
    MOVE = 0x0003,
    MOVING = 0x0216,
    NCACTIVATE = 0x0086,
    NCCALCSIZE = 0x0083,
    NCCREATE = 0x0081,
    NCDESTROY = 0x0082,
    NCHITTEST = 0x0084,
    NCLBUTTONDBLCLK = 0x00A3,
    NCLBUTTONDOWN = 0x00A1,
    NCLBUTTONUP = 0x00A2,
    NCMBUTTONDBLCLK = 0x00A9,
    NCMBUTTONDOWN = 0x00A7,
    NCMBUTTONUP = 0x00A8,
    NCMOUSEHOVER = 0x02A0,
    NCMOUSELEAVE = 0x02A2,
    NCMOUSEMOVE = 0x00A0,
    NCPAINT = 0x0085,
    NCRBUTTONDBLCLK = 0x00A6,
    NCRBUTTONDOWN = 0x00A4,
    NCRBUTTONUP = 0x00A5,
    NCXBUTTONDBLCLK = 0x00AD,
    NCXBUTTONDOWN = 0x00AB,
    NCXBUTTONUP = 0x00AC,
    NCUAHDRAWCAPTION = 0x00AE,
    NCUAHDRAWFRAME = 0x00AF,
    NEXTDLGCTL = 0x0028,
    NEXTMENU = 0x0213,
    NOTIFY = 0x004E,
    NOTIFYFORMAT = 0x0055,
    NULL = 0x0000,
    PAINT = 0x000F,
    PAINTCLIPBOARD = 0x0309,
    PAINTICON = 0x0026,
    PALETTECHANGED = 0x0311,
    PALETTEISCHANGING = 0x0310,
    PARENTNOTIFY = 0x0210,
    PASTE = 0x0302,
    PENWINFIRST = 0x0380,
    PENWINLAST = 0x038F,
    POWER = 0x0048,
    POWERBROADCAST = 0x0218,
    PRINT = 0x0317,
    PRINTCLIENT = 0x0318,
    QUERYDRAGICON = 0x0037,
    QUERYENDSESSION = 0x0011,
    QUERYNEWPALETTE = 0x030F,
    QUERYOPEN = 0x0013,
    QUEUESYNC = 0x0023,
    QUIT = 0x0012,
    RBUTTONDBLCLK = 0x0206,
    RBUTTONDOWN = 0x0204,
    RBUTTONUP = 0x0205,
    RENDERALLFORMATS = 0x0306,
    RENDERFORMAT = 0x0305,
    SETCURSOR = 0x0020,
    SETFOCUS = 0x0007,
    SETFONT = 0x0030,
    SETHOTKEY = 0x0032,
    SETICON = 0x0080,
    SETREDRAW = 0x000B,
    SETTEXT = 0x000C,
    SETTINGCHANGE = 0x001A,
    SHOWWINDOW = 0x0018,
    SIZE = 0x0005,
    SIZECLIPBOARD = 0x030B,
    SIZING = 0x0214,
    SPOOLERSTATUS = 0x002A,
    STYLECHANGED = 0x007D,
    STYLECHANGING = 0x007C,
    SYNCPAINT = 0x0088,
    SYSCHAR = 0x0106,
    SYSCOLORCHANGE = 0x0015,
    SYSCOMMAND = 0x0112,
    SYSDEADCHAR = 0x0107,
    SYSKEYDOWN = 0x0104,
    SYSKEYUP = 0x0105,
    TCARD = 0x0052,
    TIMECHANGE = 0x001E,
    TIMER = 0x0113,
    UNDO = 0x0304,
    UNINITMENUPOPUP = 0x0125,
    USER = 0x0400,
    USERCHANGED = 0x0054,
    VKEYTOITEM = 0x002E,
    VSCROLL = 0x0115,
    VSCROLLCLIPBOARD = 0x030A,
    WINDOWPOSCHANGED = 0x0047,
    WINDOWPOSCHANGING = 0x0046,
    WININICHANGE = 0x001A,
    XBUTTONDBLCLK = 0x020D,
    XBUTTONDOWN = 0x020B,
    XBUTTONUP = 0x020C,
}
public enum WM_SIZE : int
{
    RESTORE = 0,
    MIN = 1,
    MAX = 2,
    MAXSHOW = 3,
    MAXHIDE = 4
}
public enum HT : int
{
    ERROR = -2,
    TRANSPARENT = -1,
    NOWHERE = 0,
    CLIENT = 1,
    CAPTION = 2,
    SYSMENU = 3,
    GROWBOX = 4,
    SIZE = GROWBOX,
    MENU = 5,
    HSCROLL = 6,
    VSCROLL = 7,
    MINBUTTON = 8,
    MAXBUTTON = 9,
    LEFT = 10,
    RIGHT = 11,
    TOP = 12,
    TOPLEFT = 13,
    TOPRIGHT = 14,
    BOTTOM = 15,
    BOTTOMLEFT = 16,
    BOTTOMRIGHT = 17,
    BORDER = 18,
    REDUCE = MINBUTTON,
    ZOOM = MAXBUTTON,
    SIZEFIRST = LEFT,
    SIZELAST = BOTTOMRIGHT,
    OBJECT = 19,
    CLOSE = 20,
    HELP = 21
}
public enum SM : int
{
    CXSCREEN = 0,  // 0x00
    CYSCREEN = 1,  // 0x01
    CXVSCROLL = 2,  // 0x02
    CYHSCROLL = 3,  // 0x03
    CYCAPTION = 4,  // 0x04
    CXBORDER = 5,  // 0x05
    CYBORDER = 6,  // 0x06
    CXDLGFRAME = 7,  // 0x07
    CXFIXEDFRAME = 7,  // 0x07
    CYDLGFRAME = 8,  // 0x08
    CYFIXEDFRAME = 8,  // 0x08
    CYVTHUMB = 9,  // 0x09
    CXHTHUMB = 10, // 0x0A
    CXICON = 11, // 0x0B
    CYICON = 12, // 0x0C
    CXCURSOR = 13, // 0x0D
    CYCURSOR = 14, // 0x0E
    CYMENU = 15, // 0x0F
    CXFULLSCREEN = 16, // 0x10
    CYFULLSCREEN = 17, // 0x11
    CYKANJIWINDOW = 18, // 0x12
    MOUSEPRESENT = 19, // 0x13
    CYVSCROLL = 20, // 0x14
    CXHSCROLL = 21, // 0x15
    DEBUG = 22, // 0x16
    SWAPBUTTON = 23, // 0x17
    CXMIN = 28, // 0x1C
    CYMIN = 29, // 0x1D
    CXSIZE = 30, // 0x1E
    CYSIZE = 31, // 0x1F
    CXSIZEFRAME = 32, // 0x20
    CXFRAME = 32, // 0x20
    CYSIZEFRAME = 33, // 0x21
    CYFRAME = 33, // 0x21
    CXMINTRACK = 34, // 0x22
    CYMINTRACK = 35, // 0x23
    CXDOUBLECLK = 36, // 0x24
    CYDOUBLECLK = 37, // 0x25
    CXICONSPACING = 38, // 0x26
    CYICONSPACING = 39, // 0x27
    MENUDROPALIGNMENT = 40, // 0x28
    PENWINDOWS = 41, // 0x29
    DBCSENABLED = 42, // 0x2A
    CMOUSEBUTTONS = 43, // 0x2B
    SECURE = 44, // 0x2C
    CXEDGE = 45, // 0x2D
    CYEDGE = 46, // 0x2E
    CXMINSPACING = 47, // 0x2F
    CYMINSPACING = 48, // 0x30
    CXSMICON = 49, // 0x31
    CYSMICON = 50, // 0x32
    CYSMCAPTION = 51, // 0x33
    CXSMSIZE = 52, // 0x34
    CYSMSIZE = 53, // 0x35
    CXMENUSIZE = 54, // 0x36
    CYMENUSIZE = 55, // 0x37
    ARRANGE = 56, // 0x38
    CXMINIMIZED = 57, // 0x39
    CYMINIMIZED = 58, // 0x3A
    CXMAXTRACK = 59, // 0x3B
    CYMAXTRACK = 60, // 0x3C
    CXMAXIMIZED = 61, // 0x3D
    CYMAXIMIZED = 62, // 0x3E
    NETWORK = 63, // 0x3F
    CLEANBOOT = 67, // 0x43
    CXDRAG = 68, // 0x44
    CYDRAG = 69, // 0x45
    SHOWSOUNDS = 70, // 0x46
    CXMENUCHECK = 71, // 0x47
    CYMENUCHECK = 72, // 0x48
    SLOWMACHINE = 73, // 0x49
    MIDEASTENABLED = 74, // 0x4A
    MOUSEWHEELPRESENT = 75, // 0x4B
    XVIRTUALSCREEN = 76, // 0x4C
    YVIRTUALSCREEN = 77, // 0x4D
    CXVIRTUALSCREEN = 78, // 0x4E
    CYVIRTUALSCREEN = 79, // 0x4F
    CMONITORS = 80, // 0x50
    SAMEDISPLAYFORMAT = 81, // 0x51
    IMMENABLED = 82, // 0x52
    CXFOCUSBORDER = 83, // 0x53
    CYFOCUSBORDER = 84, // 0x54
    TABLETPC = 86, // 0x56
    MEDIACENTER = 87, // 0x57
    STARTER = 88, // 0x58
    SERVERR2 = 89, // 0x59
    MOUSEHORIZONTALWHEELPRESENT = 91, // 0x5B
    CXPADDEDBORDER = 92, // 0x5C
    DIGITIZER = 94, // 0x5E
    MAXIMUMTOUCHES = 95, // 0x5F

    REMOTESESSION = 0x1000, // 0x1000
    SHUTTINGDOWN = 0x2000, // 0x2000
    REMOTECONTROL = 0x2001, // 0x2001


    CONVERTIBLESLATEMODE = 0x2003,
    SYSTEMDOCKED = 0x2004,
}
public enum Keys
{
    KeyCode = 0x0000FFFF,
    Modifiers = unchecked((int)0xFFFF0000),
    None = 0x00,
    LButton = 0x01,
    RButton = 0x02,
    Cancel = 0x03,
    MButton = 0x04,
    XButton1 = 0x05,
    XButton2 = 0x06,
    Back = 0x08,
    Tab = 0x09,
    LineFeed = 0x0A,
    Clear = 0x0C,
    Return = 0x0D,
    Enter = Return,
    ShiftKey = 0x10,
    ControlKey = 0x11,
    Menu = 0x12,
    Pause = 0x13,
    Capital = 0x14,
    CapsLock = 0x14,
    KanaMode = 0x15,
    HanguelMode = 0x15,
    HangulMode = 0x15,
    JunjaMode = 0x17,
    FinalMode = 0x18,
    HanjaMode = 0x19,
    KanjiMode = 0x19,
    Escape = 0x1B,
    IMEConvert = 0x1C,
    IMENonconvert = 0x1D,
    IMEAccept = 0x1E,
    IMEAceept = IMEAccept,
    IMEModeChange = 0x1F,
    Space = 0x20,
    Prior = 0x21,
    PageUp = Prior,
    Next = 0x22,
    PageDown = Next,
    End = 0x23,
    Home = 0x24,
    Left = 0x25,
    Up = 0x26,
    Right = 0x27,
    Down = 0x28,
    Select = 0x29,
    Print = 0x2A,
    Execute = 0x2B,
    Snapshot = 0x2C,
    PrintScreen = Snapshot,
    Insert = 0x2D,
    Delete = 0x2E,
    Help = 0x2F,
    D0 = 0x30,
    D1 = 0x31,
    D2 = 0x32,
    D3 = 0x33,
    D4 = 0x34,
    D5 = 0x35,
    D6 = 0x36,
    D7 = 0x37,
    D8 = 0x38,
    D9 = 0x39,
    A = 0x41,
    B = 0x42,
    C = 0x43,
    D = 0x44,
    E = 0x45,
    F = 0x46,
    G = 0x47,
    H = 0x48,
    I = 0x49,
    J = 0x4A,
    K = 0x4B,
    L = 0x4C,
    M = 0x4D,
    N = 0x4E,
    O = 0x4F,
    P = 0x50,
    Q = 0x51,
    R = 0x52,
    S = 0x53,
    T = 0x54,
    U = 0x55,
    V = 0x56,
    W = 0x57,
    X = 0x58,
    Y = 0x59,
    Z = 0x5A,
    LWin = 0x5B,
    RWin = 0x5C,
    Apps = 0x5D,
    Sleep = 0x5F,
    NumPad0 = 0x60,
    NumPad1 = 0x61,
    NumPad2 = 0x62,
    NumPad3 = 0x63,
    NumPad4 = 0x64,
    NumPad5 = 0x65,
    NumPad6 = 0x66,
    NumPad7 = 0x67,
    NumPad8 = 0x68,
    NumPad9 = 0x69,
    Multiply = 0x6A,
    Add = 0x6B,
    Separator = 0x6C,
    Subtract = 0x6D,
    Decimal = 0x6E,
    Divide = 0x6F,
    F1 = 0x70,
    F2 = 0x71,
    F3 = 0x72,
    F4 = 0x73,
    F5 = 0x74,
    F6 = 0x75,
    F7 = 0x76,
    F8 = 0x77,
    F9 = 0x78,
    F10 = 0x79,
    F11 = 0x7A,
    F12 = 0x7B,
    F13 = 0x7C,
    F14 = 0x7D,
    F15 = 0x7E,
    F16 = 0x7F,
    F17 = 0x80,
    F18 = 0x81,
    F19 = 0x82,
    F20 = 0x83,
    F21 = 0x84,
    F22 = 0x85,
    F23 = 0x86,
    F24 = 0x87,
    NumLock = 0x90,
    Scroll = 0x91,
    LShiftKey = 0xA0,
    RShiftKey = 0xA1,
    LControlKey = 0xA2,
    RControlKey = 0xA3,
    LMenu = 0xA4,
    RMenu = 0xA5,
    BrowserBack = 0xA6,
    BrowserForward = 0xA7,
    BrowserRefresh = 0xA8,
    BrowserStop = 0xA9,
    BrowserSearch = 0xAA,
    BrowserFavorites = 0xAB,
    BrowserHome = 0xAC,
    VolumeMute = 0xAD,
    VolumeDown = 0xAE,
    VolumeUp = 0xAF,
    MediaNextTrack = 0xB0,
    MediaPreviousTrack = 0xB1,
    MediaStop = 0xB2,
    MediaPlayPause = 0xB3,
    LaunchMail = 0xB4,
    SelectMedia = 0xB5,
    LaunchApplication1 = 0xB6,
    LaunchApplication2 = 0xB7,
    OemSemicolon = 0xBA,
    Oem1 = OemSemicolon,
    Oemplus = 0xBB,
    Oemcomma = 0xBC,
    OemMinus = 0xBD,
    OemPeriod = 0xBE,
    OemQuestion = 0xBF,
    Oem2 = OemQuestion,
    Oemtilde = 0xC0,
    Oem3 = Oemtilde,
    OemOpenBrackets = 0xDB,
    Oem4 = OemOpenBrackets,
    OemPipe = 0xDC,
    Oem5 = OemPipe,
    OemCloseBrackets = 0xDD,
    Oem6 = OemCloseBrackets,
    OemQuotes = 0xDE,
    Oem7 = OemQuotes,
    Oem8 = 0xDF,
    OemBackslash = 0xE2,
    Oem102 = OemBackslash,
    ProcessKey = 0xE5,
    Packet = 0xE7,
    Attn = 0xF6,
    Crsel = 0xF7,
    Exsel = 0xF8,
    EraseEof = 0xF9,
    Play = 0xFA,
    Zoom = 0xFB,
    NoName = 0xFC,
    Pa1 = 0xFD,
    OemClear = 0xFE,
    Shift = 0x00010000,
    Control = 0x00020000,
    Alt = 0x00040000,
}
#endregion
public unsafe class Interop
{
    #region Struct
    [StructLayout(LayoutKind.Sequential)]
    public record struct CURSORINFO(int cbSize, int flags, nint hCursor, Point ptScreenPos);
    [StructLayout(LayoutKind.Sequential)]
    public record struct RECT(int Left, int Top, int Right, int Bottom);
    [StructLayout(LayoutKind.Sequential)]
    public record struct BITMAPINFOHEADER(uint Size, int Width, int Height, ushort Planes, ushort BitCount, uint Compression, uint SizeImage, int XPelsPerMeter, int YPelsPerMeter, uint ClrUsed, uint ClrImportant);
    [StructLayout(LayoutKind.Sequential)]
    public record struct BITMAPINFO(BITMAPINFOHEADER Header, uint Colors);
    [StructLayout(LayoutKind.Sequential)]
    public struct RGB
    {
        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        //public byte A;
        public byte B;
        public byte G;
        public byte R;

        public override string ToString() => $"{R} {G} {B}";

        public static bool operator ==(RGB left, RGB right) => left.R == right.R && left.G == right.G && left.B == right.B;
        public static bool operator !=(RGB left, RGB right) => !(left == right);

        public static RGB White = new RGB(255, 255, 255);
    }
    #endregion

    #region Func
    const string user = "user32";
    const string kernel = "kernel32";
    const string gdi = "gdi32";

    [DllImport(user, CharSet = Auto)]
    public static extern
        nint SendMessage(nint hWnd, uint msg, nuint wParam, StringBuilder lParam);

    [DllImport(user, CharSet = Auto)]
    public static extern
        nint SendMessage(nint hWnd, uint msg, nuint wParam, [MarshalAs(LPWStr)] string lParam);

    [DllImport(user, CharSet = Auto)]
    public static extern
        nint SendMessage(nint hWnd, uint msg, nuint wParam, ref nint lParam);

    [DllImport(user, CharSet = Auto)]
    public static extern
        nint SendMessage(nint hWnd, uint msg, nuint wParam, nint lParam);

    [DllImport(user, CharSet = Auto)]
    public static extern
        nint SendMessage(nint hWnd, WM msg, nuint wParam, nint lParam);

    [DllImport(user)]
    public static extern
        nint SetCursor(nint hCursor);

    [DllImport(user)]
    public static extern
        nint LoadCursorA(nint hCursor, IDC str);

    [DllImport(user, CharSet = Auto)]
    public static extern
        int MessageBox(nint hWnd, string text, string caption, uint type);

    [DllImport(user)]
    public static extern
        short GetAsyncKeyState(Keys vKey);

    [DllImport(user)]
    public static extern
        short GetKeyState(Keys vKey);

    [DllImport(user)]
    public static extern
        bool GetCursorInfo(ref CURSORINFO pci);

    [DllImport(user)]
    public static extern
        nint GetForegroundWindow();

    [DllImport(user)]
    public static extern
        nint CallWindowProc(nint prevWndFunc, nint hWnd, WM msg, nint wParam, nint lParam);

    [DllImport(user)]
    public static extern
        int SetWindowLong(nint hWnd, int index, uint newLong);

    [DllImport(user)]
    public static extern
        nint SetWindowLongPtr(nint hWnd, int nIndex, nint dwNewLong);

    [DllImport(user)]
    public static extern
        nint GetWindowLong(nint hWnd, int nIndex);

    [DllImport(user)]
    [return: MarshalAs(Bool)]
    public static extern
        bool SetCursorPos(int x, int y);

    [DllImport(user)]
    public static extern
        int GetSystemMetrics(SM smIndex);

    [DllImport(kernel)]
    public static extern
        uint GetLastError();

    [DllImport(kernel, CharSet = Auto)]
    public static extern
        uint CreateThread(uint* threadAttb, uint stackSize, ThreadStart startAddr, uint* param, uint creationFlags, out uint threadId);

    [DllImport(kernel, CharSet = Unicode)]
    public static extern
        nint GetModuleHandle([MarshalAs(LPWStr)] string moduleName);

    [DllImport(kernel, CharSet = Ansi, ExactSpelling = true)]
    public static extern
        nint GetProcAddress(nint hModule, string procName);

    [DllImport(kernel, CharSet = Ansi)]
    public static extern
        nint LoadLibrary([MarshalAs(LPStr)] string fileName);

    [DllImport(kernel)]
    public static extern
        nint GetCurrentThread();

    [DllImport(kernel)]
    public static extern
        uint GetTickCount();

    [DllImport(gdi)]
    public static extern
        nint SelectObject(nint hdc, nint obj);

    [DllImport(gdi)]
    [return: MarshalAs(Bool)]
    public static extern
        bool DeleteObject(nint obj);

    [DllImport(user)] public static extern 
        nint GetWindowDC(nint hWnd);

    [DllImport(user)] public static extern 
        nint GetDC(nint hWnd);

    [DllImport(user)] public static extern 
        int ReleaseDC(nint hWnd, nint hdc);

    [DllImport(gdi)] public static extern
        nint CreateDIBSection(nint hdc, BITMAPINFO* pbmi, uint usage, RGB** ppvBits, nint hSection, uint offset);

    [DllImport(gdi)] public static extern
        nint CreateCompatibleDC(nint hdc);

    [DllImport(gdi)] public static extern
        bool DeleteDC(nint hdc);

    [DllImport(gdi)] public static extern
        bool BitBlt(nint hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, nint hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);
    #endregion

    #region Method
    public static uint StartThread(ThreadStart ThreadFunc)
    {
        uint i = 0;
        uint lpThreadID = 0;
        uint dwHandle = CreateThread(null, 0, ThreadFunc, &i, 0, out lpThreadID);
        return dwHandle;
    }

    public static void* GetProcAddressPtr(nint hModule, [MarshalAs(LPWStr)] string lpModuleName) => GetProcAddress(hModule, lpModuleName).ToPointer();

    public static int MessageBox(object obj) => MessageBox(0, obj.ToString() ?? "null", "", 0);

    public static nint lwjgl64Handle = GetModuleHandle("lwjgl64");
    public static nint openglHandle = GetModuleHandle("opengl32");
    public static void* GetNGLFunc(string func) => GetProcAddressPtr(lwjgl64Handle, "Java_org_lwjgl_opengl_GL11_ngl" + func);
    public static void* GetGLFunc(string func) => GetProcAddressPtr(openglHandle, func);

    public static bool IsKeyDown(Keys key) => GetAsyncKeyState(key) != 0;

    public static bool IsCursorHide()
    {
        CURSORINFO cur = new();
        cur.cbSize = sizeof(CURSORINFO);
        GetCursorInfo(ref cur);

        int realFlag = cur.hCursor.ToInt32();
        return realFlag > 66000 || realFlag < 65000;
    }

    public static bool IsWindowActive()
    {
        nint activeWindow = GetForegroundWindow();
        nint procWindow = Process.GetCurrentProcess().MainWindowHandle;
        return activeWindow == procWindow;
    }

    public static nint SetCursor(IDC cursor) => SetCursor(LoadCursorA(nint.Zero, cursor));
    #endregion
}

public static class KeysHelper
{
    private static Keys[] keys = Enum.GetValues<Keys>();
    private static string[] names = Enum.GetNames<Keys>().Select(n => n.ToLower()).ToArray();
    public static Keys GetKey(string name)
    {
        name = name.ToLower();

        for (int i = 0; i < keys.Length; i++)
            if (names[i] == name)
                return keys[i];

        return Keys.Modifiers;
    }
}