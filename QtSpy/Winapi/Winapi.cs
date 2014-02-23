/*
This file is put in Public Domain by Anton A. Drachev, 2009

A fresh version of this file you can found at: http://drachev.com/winapi/
*/

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Winapi
{
    #region enums

    public enum ListViewMessages : uint
    {
        LVM_FIRST = 0x1000,
        LVM_SCROLL = LVM_FIRST + 20,
        LVM_GETSELECTEDCOUNT = LVM_FIRST + 50,
        LVM_GETITEMSTATE = LVM_FIRST + 44,
        LVM_GETITEMTEXT = LVM_FIRST + 45,
        LVM_GETITEM = LVM_FIRST + 5,
        LVM_GETITEMCOUNT = LVM_FIRST + 4,
        LVM_GETITEMW = LVM_FIRST + 75,
        LVM_GETITEMTEXTW = LVM_FIRST + 115,
        LVM_GETCOLUMNORDERARRAY = LVM_FIRST + 59,
        LVM_SETHOTITEM = LVM_FIRST + 60,
        LVM_GETHOTITEM = LVM_FIRST + 61,
        LVM_SETHOTCURSOR = LVM_FIRST + 62,
        LVM_GETHOTCURSOR = LVM_FIRST + 63,
        LVM_SETITEMSTATE = LVM_FIRST + 43
    }

    public enum VirtAllocTypes : uint
    {
        WRITE_WATCH_FLAG_RESET = 0x00000001, // Win98 only
        MEM_COMMIT = 0x00001000,
        MEM_RESERVE = 0x00002000,
        MEM_COMMIT_OR_RESERVE = 0x00003000,
        MEM_DECOMMIT = 0x00004000,
        MEM_RELEASE = 0x00008000,
        MEM_FREE = 0x00010000,
        MEM_PUBLIC = 0x00020000,
        MEM_MAPPED = 0x00040000,
        MEM_RESET = 0x00080000, // Win2K only
        MEM_TOP_DOWN = 0x00100000,
        MEM_WRITE_WATCH = 0x00200000, // Win98 only
        MEM_PHYSICAL = 0x00400000, // Win2K only
        //MEM_4MB_PAGES    = 0x80000000, // ??
        SEC_IMAGE = 0x01000000,
        MEM_IMAGE = SEC_IMAGE
    }

    public enum WindowsMessages : uint
    {
        WM_ACTIVATE = 0x6,
        WM_ACTIVATEAPP = 0x1C,
        WM_AFXFIRST = 0x360,
        WM_AFXLAST = 0x37F,
        WM_APP = 0x8000,
        WM_ASKCBFORMATNAME = 0x30C,
        WM_CANCELJOURNAL = 0x4B,
        WM_CANCELMODE = 0x1F,
        WM_CAPTURECHANGED = 0x215,
        WM_CHANGECBCHAIN = 0x30D,
        WM_CHAR = 0x102,
        WM_CHARTOITEM = 0x2F,
        WM_CHILDACTIVATE = 0x22,
        WM_CLEAR = 0x303,
        WM_CLOSE = 0x10,
        WM_COMMAND = 0x111,
        WM_COMPACTING = 0x41,
        WM_COMPAREITEM = 0x39,
        WM_CONTEXTMENU = 0x7B,
        WM_COPY = 0x301,
        WM_COPYDATA = 0x4A,
        WM_CREATE = 0x1,
        WM_CTLCOLORBTN = 0x135,
        WM_CTLCOLORDLG = 0x136,
        WM_CTLCOLOREDIT = 0x133,
        WM_CTLCOLORLISTBOX = 0x134,
        WM_CTLCOLORMSGBOX = 0x132,
        WM_CTLCOLORSCROLLBAR = 0x137,
        WM_CTLCOLORSTATIC = 0x138,
        WM_CUT = 0x300,
        WM_DEADCHAR = 0x103,
        WM_DELETEITEM = 0x2D,
        WM_DESTROY = 0x2,
        WM_DESTROYCLIPBOARD = 0x307,
        WM_DEVICECHANGE = 0x219,
        WM_DEVMODECHANGE = 0x1B,
        WM_DISPLAYCHANGE = 0x7E,
        WM_DRAWCLIPBOARD = 0x308,
        WM_DRAWITEM = 0x2B,
        WM_DROPFILES = 0x233,
        WM_ENABLE = 0xA,
        WM_ENDSESSION = 0x16,
        WM_ENTERIDLE = 0x121,
        WM_ENTERMENULOOP = 0x211,
        WM_ENTERSIZEMOVE = 0x231,
        WM_ERASEBKGND = 0x14,
        WM_EXITMENULOOP = 0x212,
        WM_EXITSIZEMOVE = 0x232,
        WM_FONTCHANGE = 0x1D,
        WM_GETDLGCODE = 0x87,
        WM_GETFONT = 0x31,
        WM_GETHOTKEY = 0x33,
        WM_GETICON = 0x7F,
        WM_GETMINMAXINFO = 0x24,
        WM_GETOBJECT = 0x3D,
        WM_GETSYSMENU = 0x313,
        WM_GETTEXT = 0xD,
        WM_GETTEXTLENGTH = 0xE,
        WM_HANDHELDFIRST = 0x358,
        WM_HANDHELDLAST = 0x35F,
        WM_HELP = 0x53,
        WM_HOTKEY = 0x312,
        WM_HSCROLL = 0x114,
        WM_HSCROLLCLIPBOARD = 0x30E,
        WM_ICONERASEBKGND = 0x27,
        WM_IME_CHAR = 0x286,
        WM_IME_COMPOSITION = 0x10F,
        WM_IME_COMPOSITIONFULL = 0x284,
        WM_IME_CONTROL = 0x283,
        WM_IME_ENDCOMPOSITION = 0x10E,
        WM_IME_KEYDOWN = 0x290,
        WM_IME_KEYLAST = 0x10F,
        WM_IME_KEYUP = 0x291,
        WM_IME_NOTIFY = 0x282,
        WM_IME_REQUEST = 0x288,
        WM_IME_SELECT = 0x285,
        WM_IME_SETCONTEXT = 0x281,
        WM_IME_STARTCOMPOSITION = 0x10D,
        WM_INITDIALOG = 0x110,
        WM_INITMENU = 0x116,
        WM_INITMENUPOPUP = 0x117,
        WM_INPUT = 0x00FF,
        WM_INPUTLANGCHANGE = 0x51,
        WM_INPUTLANGCHANGEREQUEST = 0x50,
        WM_KEYDOWN = 0x100,
        WM_KEYFIRST = 0x100,
        WM_KEYLAST = 0x108,
        WM_KEYUP = 0x101,
        WM_KILLFOCUS = 0x8,
        WM_LBUTTONDBLCLK = 0x203,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_MBUTTONDBLCLK = 0x209,
        WM_MBUTTONDOWN = 0x207,
        WM_MBUTTONUP = 0x208,
        WM_MDIACTIVATE = 0x222,
        WM_MDICASCADE = 0x227,
        WM_MDICREATE = 0x220,
        WM_MDIDESTROY = 0x221,
        WM_MDIGETACTIVE = 0x229,
        WM_MDIICONARRANGE = 0x228,
        WM_MDIMAXIMIZE = 0x225,
        WM_MDINEXT = 0x224,
        WM_MDIREFRESHMENU = 0x234,
        WM_MDIRESTORE = 0x223,
        WM_MDISETMENU = 0x230,
        WM_MDITILE = 0x226,
        WM_MEASUREITEM = 0x2C,
        WM_MENUCHAR = 0x120,
        WM_MENUCOMMAND = 0x126,
        WM_MENUDRAG = 0x123,
        WM_MENUGETOBJECT = 0x124,
        WM_MENURBUTTONUP = 0x122,
        WM_MENUSELECT = 0x11F,
        WM_MOUSEACTIVATE = 0x21,
        WM_MOUSEFIRST = 0x200,
        WM_MOUSEHOVER = 0x2A1,
        WM_MOUSELAST = 0x20A,
        WM_MOUSELEAVE = 0x2A3,
        WM_MOUSEMOVE = 0x200,
        WM_MOUSEWHEEL = 0x20A,
        WM_MOUSEHWHEEL = 0x20E,
        WM_MOVE = 0x3,
        WM_MOVING = 0x216,
        WM_NCACTIVATE = 0x86,
        WM_NCCALCSIZE = 0x83,
        WM_NCCREATE = 0x81,
        WM_NCDESTROY = 0x82,
        WM_NCHITTEST = 0x84,
        WM_NCLBUTTONDBLCLK = 0xA3,
        WM_NCLBUTTONDOWN = 0xA1,
        WM_NCLBUTTONUP = 0xA2,
        WM_NCMBUTTONDBLCLK = 0xA9,
        WM_NCMBUTTONDOWN = 0xA7,
        WM_NCMBUTTONUP = 0xA8,
        WM_NCMOUSEHOVER = 0x2A0,
        WM_NCMOUSELEAVE = 0x2A2,
        WM_NCMOUSEMOVE = 0xA0,
        WM_NCPAINT = 0x85,
        WM_NCRBUTTONDBLCLK = 0xA6,
        WM_NCRBUTTONDOWN = 0xA4,
        WM_NCRBUTTONUP = 0xA5,
        WM_NEXTDLGCTL = 0x28,
        WM_NEXTMENU = 0x213,
        WM_NOTIFY = 0x4E,
        WM_NOTIFYFORMAT = 0x55,
        WM_NULL = 0x0,
        WM_PAINT = 0xF,
        WM_PAINTCLIPBOARD = 0x309,
        WM_PAINTICON = 0x26,
        WM_PALETTECHANGED = 0x311,
        WM_PALETTEISCHANGING = 0x310,
        WM_PARENTNOTIFY = 0x210,
        WM_PASTE = 0x302,
        WM_PENWINFIRST = 0x380,
        WM_PENWINLAST = 0x38F,
        WM_POWER = 0x48,
        WM_PRINT = 0x317,
        WM_PRINTCLIENT = 0x318,
        WM_QUERYDRAGICON = 0x37,
        WM_QUERYENDSESSION = 0x11,
        WM_QUERYNEWPALETTE = 0x30F,
        WM_QUERYOPEN = 0x13,
        WM_QUERYUISTATE = 0x129,
        WM_QUEUESYNC = 0x23,
        WM_QUIT = 0x12,
        WM_RBUTTONDBLCLK = 0x206,
        WM_RBUTTONDOWN = 0x204,
        WM_RBUTTONUP = 0x205,
        WM_RENDERALLFORMATS = 0x306,
        WM_RENDERFORMAT = 0x305,
        WM_SETCURSOR = 0x20,
        WM_SETFOCUS = 0x7,
        WM_SETFONT = 0x30,
        WM_SETHOTKEY = 0x32,
        WM_SETICON = 0x80,
        WM_SETREDRAW = 0xB,
        WM_SETTEXT = 0xC,
        WM_SETTINGCHANGE = 0x1A,
        WM_SHOWWINDOW = 0x18,
        WM_SIZE = 0x5,
        WM_SIZECLIPBOARD = 0x30B,
        WM_SIZING = 0x214,
        WM_SPOOLERSTATUS = 0x2A,
        WM_STYLECHANGED = 0x7D,
        WM_STYLECHANGING = 0x7C,
        WM_SYNCPAINT = 0x88,
        WM_SYSCHAR = 0x106,
        WM_SYSCOLORCHANGE = 0x15,
        WM_SYSCOMMAND = 0x112,
        WM_SYSDEADCHAR = 0x107,
        WM_SYSKEYDOWN = 0x104,
        WM_SYSKEYUP = 0x105,
        WM_SYSTIMER = 0x118,  // undocumented, see http://support.microsoft.com/?id=108938
        WM_TCARD = 0x52,
        WM_TIMECHANGE = 0x1E,
        WM_TIMER = 0x113,
        WM_UNDO = 0x304,
        WM_UNINITMENUPOPUP = 0x125,
        WM_USER = 0x400,
        WM_USERCHANGED = 0x54,
        WM_VKEYTOITEM = 0x2E,
        WM_VSCROLL = 0x115,
        WM_VSCROLLCLIPBOARD = 0x30A,
        WM_WINDOWPOSCHANGED = 0x47,
        WM_WINDOWPOSCHANGING = 0x46,
        WM_WININICHANGE = 0x1A,
        WM_XBUTTONDBLCLK = 0x20D,
        WM_XBUTTONDOWN = 0x20B,
        WM_XBUTTONUP = 0x20C
    }

    public enum SystemCommands : uint
    {
        SC_SIZE = 0xF000,
        SC_MOVE = 0xF010,
        SC_MOVE_INTERNAL = 0xF012,
        SC_MINIMIZE = 0xF020,
        SC_MAXIMIZE = 0xF030,
        SC_MAXIMIZE2 = 0xF032,
        SC_NEXTWINDOW = 0xF040,
        SC_PREVWINDOW = 0xF050,
        SC_CLOSE = 0xF060,
        SC_VSCROLL = 0xF070,
        SC_HSCROLL = 0xF080,
        SC_MOUSEMENU = 0xF090,
        SC_KEYMENU = 0xF100,
        SC_ARRANGE = 0xF110,
        SC_RESTORE = 0xF120,
        SC_RESTORE2 = 0xF122,
        SC_TASKLIST = 0xF130,
        SC_SCREENSAVE = 0xF140,
        SC_HOTKEY = 0xF150,
        SC_DEFAULT = 0xF160,
        SC_MONITORPOWER = 0xF170,
        SC_CONTEXTHELP = 0xF180,
        SC_SEPARATOR = 0xF00F
    }

    public enum ShowWindowCommands : uint
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11,
        SW_MAX = 11
    }

    public enum WindowLongIndex
    {
        GWL_WNDPROC = -4,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20,
        GWL_USERDATA = -21,
        GWL_ID = -12,
    }

    public enum WindowPosZ
    {
        HWND_TOP = 0,
        HWND_BOTTOM = 1,
        HWND_TOPMOST = -1,
        HWND_NOTOPMOST = -2
    }

    public enum HitTest
    {
        HTERROR = -2,
        HTTRANSPARENT = -1,
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTCAPTION = 2,
        HTSYSMENU = 3,
        HTGROWBOX = 4,
        HTSIZE = HTGROWBOX,
        HTMENU = 5,
        HTHSCROLL = 6,
        HTVSCROLL = 7,
        HTMINBUTTON = 8,
        HTMAXBUTTON = 9,
        HTLEFT = 10,
        HTRIGHT = 11,
        HTTOP = 12,
        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HTREDUCE = HTMINBUTTON,
        HTZOOM = HTMAXBUTTON,
        HTSIZEFIRST = HTLEFT,
        HTSIZELAST = HTBOTTOMRIGHT,
        HTOBJECT = 19,
        HTCLOSE = 20,
        HTHELP = 21
    }

    public enum ScrollBarConstants : uint
    {
        SB_HORZ = 0,
        SB_VERT = 1,
        SB_CTL = 2,
        SB_BOTH = 3,
    }

    public enum GetAncestorFlags
    {
        /// <summary>
        /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
        /// </summary>
        GA_PARENT = 1,
        /// <summary>
        /// Retrieves the root window by walking the chain of parent windows
        /// </summary>
        GA_ROOT = 2,
        /// <summary>
        /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent
        /// </summary>
        GA_ROOTOWNER = 3,
    }

    public enum GetWindowCmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6,
    }

    #endregion

    #region flag enums

    [Flags]
    public enum WindowStyles
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = unchecked((int)0x80000000),
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000, /* WS_BORDER | WS_DLGFRAME  */
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
        WS_CHILDWINDOW = WS_CHILD,

        DS_ABSALIGN = 0x1,
        DS_SYSMODAL = 0x2,
        DS_LOCALEDIT = 0x20,
        DS_SETFONT = 0x40,
        DS_MODALFRAME = 0x80,
        DS_NOIDLEMSG = 0x100,
        DS_SETFOREGROUND = 0x200,
        //#if(WINVER >= 0x0400)
        DS_3DLOOK = 0x0004,
        DS_FIXEDSYS = 0x0008,
        DS_NOFAILCREATE = 0x0010,
        DS_CONTROL = 0x0400,
        DS_CENTER = 0x0800,
        DS_CENTERMOUSE = 0x1000,
        DS_CONTEXTHELP = 0x2000,
    }

    [Flags]
    public enum WindowExStyles
    {
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_DRAGDETECT = 0x00000002,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,

        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,

        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,

        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_LAYERED = 0x00080000,

        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_LAYOUTRTL = 0x00400000,
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000,

        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
    }

    [Flags]
    public enum WindowPos
    {
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOREDRAW = 0x0008,
        SWP_NOACTIVATE = 0x0010,
        SWP_FRAMECHANGED = 0x0020,
        SWP_SHOWWINDOW = 0x0040,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOSENDCHANGING = 0x0400,
        SWP_DRAWFRAME = 0x0020,
        SWP_NOREPOSITION = 0x0200,
        SWP_RESERVED1 = 0x800,
        SWP_RESERVED2 = 0x1000,
        SWP_DEFERERASE = 0x2000,
        SWP_ASYNCWINDOWPOS = 0x4000,
    }

    [Flags]
    public enum KeyStateMasks : uint
    {
        MK_LBUTTON = 0x0001,
        MK_RBUTTON = 0x0002,
        MK_SHIFT = 0x0004,
        MK_CONTROL = 0x0008,
        MK_MBUTTON = 0x0010,
    }

    [Flags]
    public enum AccessProtectFlags : uint
    {
        PAGE_NOACCESS = 0x001,
        PAGE_READONLY = 0x002,
        PAGE_READWRITE = 0x004,
        PAGE_WRITECOPY = 0x008,
        PAGE_EXECUTE = 0x010,
        PAGE_EXECUTE_READ = 0x020,
        PAGE_EXECUTE_READWRITE = 0x040,
        PAGE_EXECUTE_WRITECOPY = 0x080,
        PAGE_GUARD = 0x100,
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400,
    }

    [Flags]
    public enum AccessFlags : uint
    {
        DELETE = 0x00010000,
        READ_CONTROL = 0x00020000,
        WRITE_DAC = 0x00040000,
        WRITE_OWNER = 0x00080000,
        SYNCHRONIZE = 0x00100000,
        STANDARD_RIGHTS_REQUIRED = 0x000F0000,
        STANDARD_RIGHTS_READ = READ_CONTROL,
        STANDARD_RIGHTS_WRITE = READ_CONTROL,
        STANDARD_RIGHTS_EXECUTE = READ_CONTROL,
        STANDARD_RIGHTS_ALL = 0x001F0000,
        SPECIFIC_RIGHTS_ALL = 0x0000FFFF,
        ACCESS_SYSTEM_SECURITY = 0x01000000,
        MAXIMUM_ALLOWED = 0x02000000,
        GENERIC_READ = 0x80000000,
        GENERIC_WRITE = 0x40000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_ALL = 0x10000000,
    }

    [Flags]
    public enum ProcessAccessFlags : uint
    {
        PROCESS_TERMINATE = 0x0001,
        PROCESS_CREATE_THREAD = 0x0002,
        PROCESS_SET_SESSIONID = 0x0004,
        PROCESS_VM_OPERATION = 0x0008,
        PROCESS_VM_READ = 0x0010,
        PROCESS_VM_WRITE = 0x0020,
        PROCESS_DUP_HANDLE = 0x0040,
        PROCESS_CREATE_PROCESS = 0x0080,
        PROCESS_SET_QUOTA = 0x0100,
        PROCESS_SET_INFORMATION = 0x0200,
        PROCESS_QUERY_INFORMATION = 0x0400,
        PROCESS_SUSPEND_RESUME = 0x0800,
        PROCESS_QUERY_LIMITED_INFORMATION = 0x1000,
        PROCESS_ALL_ACCESS = AccessFlags.STANDARD_RIGHTS_REQUIRED | AccessFlags.SYNCHRONIZE | 0xFFFF,
    }

    [Flags]
    public enum ListViewFlags : uint
    {
        LVIF_TEXT = 0x0001,
        LVIF_IMAGE = 0x0002,
        LVIF_PARAM = 0x0004,
        LVIF_STATE = 0x0008,
        LVIF_INDENT = 0x0010,
        LVIF_GROUPID = 0x0100,
        LVIF_COLUMNS = 0x0200,
        LVIF_NORECOMPUTE = 0x0800,
        LVIF_DI_SETITEM = 0x1000,
    }

    [Flags]
    public enum ListViewItemStyle
    {
        LVIS_FOCUSED = 0x0001,
        LVIS_SELECTED = 0x0002,
        LVIS_CUT = 0x0004,
        LVIS_DROPHILITED = 0x0008,
        LVIS_ACTIVATING = 0x0020,
        LVIS_OVERLAYMASK = 0x0F00,
        LVIS_STATEIMAGEMASK = 0xF000,
    }

    [Flags]
    public enum SendMessageTimeoutFlags : uint
    {
        SMTO_NORMAL = 0x0000,
        SMTO_BLOCK = 0x0001,
        SMTO_ABORTIFHUNG = 0x0002,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x0008,
    }

    [Flags]
    public enum UpdateLayeredFlags
    {
        ULW_COLORKEY = 0x00000001,
        ULW_ALPHA = 0x00000002,
        ULW_OPAQUE = 0x00000004,
    }

    [Flags]
    public enum RedrawFlags : uint
    {
        RDW_INVALIDATE = 0x0001,
        RDW_INTERNALPAINT = 0x0002,
        RDW_ERASE = 0x0004,
        RDW_VALIDATE = 0x0008,
        RDW_NOINTERNALPAINT = 0x0010,
        RDW_NOERASE = 0x0020,
        RDW_NOCHILDREN = 0x0040,
        RDW_ALLCHILDREN = 0x0080,
        RDW_UPDATENOW = 0x0100,
        RDW_ERASENOW = 0x0200,
        RDW_FRAME = 0x0400,
        RDW_NOFRAME = 0x0800,
    }

    [Flags]
    public enum EnableScrollBarFlags : uint
    {
        ESB_ENABLE_BOTH = 0x0000,
        ESB_DISABLE_BOTH = 0x0003,

        ESB_DISABLE_LEFT = 0x0001,
        ESB_DISABLE_RIGHT = 0x0002,

        ESB_DISABLE_UP = 0x0001,
        ESB_DISABLE_DOWN = 0x0002,

        ESB_DISABLE_LTUP = ESB_DISABLE_LEFT,
        ESB_DISABLE_RTDN = ESB_DISABLE_RIGHT,
    }

    #endregion

    #region structs

    [StructLayout(LayoutKind.Sequential)]
    public struct LVITEM
    {
        public ListViewFlags mask;
        public int iItem;
        public int iSubItem;
        public ListViewItemStyle state;
        public ListViewItemStyle stateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public int lParam;
        public int iIndent;
        public int iGroupId;
        public uint cColumns;
        public IntPtr puColumns;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMLISTVIEW
    {
        public NMHDR hdr;
        public int iItem;
        public int iSubItem;
        public uint uNewState;
        public uint uOldState;
        public uint uChanged;
        public POINT ptAction;
        public int lParam;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public UIntPtr idFrom;
        public uint code;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public POINT(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public static implicit operator Point(POINT p)
        {
            return new Point(p.X, p.Y);
        }

        public static implicit operator POINT(Point p)
        {
            return new POINT(p);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;

        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }

        public SIZE(Size size)
        {
            cx = size.Width;
            cy = size.Height;
        }

        public static implicit operator Size(SIZE p)
        {
            return new Size(p.cx, p.cy);
        }

        public static implicit operator SIZE(Size p)
        {
            return new SIZE(p);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMITEMACTIVATE
    {
        public NMHDR hdr;
        public int iItem;
        public int iSubItem;
        public uint uNewState;
        public uint uOldState;
        public uint uChanged;
        public POINT ptAction;
        public int lParam;
        public uint uKeyFlags;
    };

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = Top + value; }
        }
        public int Width
        {
            get { return Right - Left; }
            set { Right = Left + value; }
        }
        public Size Size { get { return new Size(Width, Height); } }

        public Point Location { get { return new Point(Left, Top); } }

        // Handy method for converting to a System.Drawing.Rectangle
        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(Left, Top, Right, Bottom);
        }

        public static RECT FromRectangle(Rectangle rectangle)
        {
            return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        public override int GetHashCode()
        {
            return Left ^ ((Top << 13) | (Top >> 0x13))
              ^ ((Width << 0x1a) | (Width >> 6))
              ^ ((Height << 7) | (Height >> 0x19));
        }

        #region Operator overloads

        public static implicit operator Rectangle(RECT rect)
        {
            return rect.ToRectangle();
        }

        public static implicit operator RECT(Rectangle rect)
        {
            return FromRectangle(rect);
        }

        #endregion

        public static RECT operator -(RECT a, RECT b)
        {
            return new RECT(a.Left - b.Left, a.Top - b.Top, a.Width - b.Width, a.Height - b.Height);
        }

        public static RECT operator +(RECT a, RECT b)
        {
            return new RECT(a.Left + b.Left, a.Top + b.Top, a.Width + b.Width, a.Height + b.Height);
        }

        public static bool operator ==(RECT a, RECT b)
        {
            return a.Left == b.Left && a.Top == b.Top && a.Right == b.Right && a.Bottom == b.Bottom;
        }

        public static bool operator !=(RECT a, RECT b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return this == (RECT)obj;
        }

        public override string ToString()
        {
            return string.Format("({0}; {1}), ({2}×{3})", Left, Top, Width, Height);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public WindowPos flags;

        public Rectangle Rectangle
        {
            get { return new Rectangle(x, y, cx, cy); }
            set
            {
                x = value.X;
                y = value.Y;
                cx = value.Width;
                cy = value.Height;
            }
        }

        public override string ToString()
        {
            return string.Format("({0}; {1}), ({2}×{3})", x, y, cx, cy);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public static WINDOWPLACEMENT Create()
        {
            var result = new WINDOWPLACEMENT();
            result.length = Marshal.SizeOf(result);
            return result;
        }

        public int length;
        public int flags;
        public ShowWindowCommands showCmd;
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
    }

    #endregion
}