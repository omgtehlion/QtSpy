/*
Copyright (c) 2009, Anton A. Drachev

Some rights reserved ;)

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

 * Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.
 * Redistributions in binary form should reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the author nor the names of other contributors may be used to endorse
or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY 
OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY 
WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

A fresh version of this file you can found at: http://drachev.com/winapi/
*/

/*
Revision History:
 *
 * 2014.03.11:  Removed old (NET20) crap
 * 2014.03.13:  Added
                    GetForegroundWindow
                    GetWindowThreadId
                    AttachThreadInput
                    SetFocus
                    ClientToScreen
                    ScreenToClient
 * 2014.03.14:  Added
                    SetWindowSize
                    SetWindowClientSize
 * 2014.03.17:  Added
                    PostThreadMessage
 * 2014.03.17:  Added
                    RemoveProp
 * 2014.04.09:  Added
                    CreateEvent
                    SetEvent
                    ResetEvent
                    DuplicateHandle
                    GetCurrentProcess
                    WaitForSingleObject
 * 2014.04.10:  Fixed keystroke simulation
                    - KeyUpDown
                    - KeyStroke
                    + MakeKeyLParam
                    + PostKeyStroke
                    + PostKeyChar
 * 2014.04.10:  Added
                    WindowFromPoint
 * 2014.04.28:  Added
                    BITMAPINFOHEADER
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

public static class Winapi
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
        LVM_SETITEMSTATE = LVM_FIRST + 43,
        LVM_SETSELECTIONMARK = LVM_FIRST + 67,
        LVM_ENSUREVISIBLE = LVM_FIRST + 19,
        LVM_REDRAWITEMS = LVM_FIRST + 21,
        LVM_GETITEMRECT = LVM_FIRST + 14,
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
        MEM_IMAGE = SEC_IMAGE,
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
        WM_XBUTTONUP = 0x20C,
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
        SC_SEPARATOR = 0xF00F,
    }

    public enum ShowWindowCommands : uint
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
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
        SW_MAX = 11,
    }

    public enum GetWindowCommands : uint
    {
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6,
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
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
        HTHELP = 21,
    }

    public enum ScrollBarConstants : uint
    {
        SB_HORZ = 0,
        SB_VERT = 1,
        SB_CTL = 2,
        SB_BOTH = 3,
    }

    public enum WaitResult : uint
    {
        WAIT_ABANDONED = 0x00000080,
        WAIT_OBJECT_0 = 0x00000000,
        WAIT_TIMEOUT = 0x00000102,
        WAIT_FAILED = 0xFFFFFFFF,
    }

    public enum BitmapCompressionMode : uint
    {
        BI_RGB = 0,
        BI_RLE8 = 1,
        BI_RLE4 = 2,
        BI_BITFIELDS = 3,
        BI_JPEG = 4,
        BI_PNG = 5,
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
        WS_CHILDWINDOW = WS_CHILD
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
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST
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
        MK_MBUTTON = 0x0010
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
        PAGE_WRITECOMBINE = 0x400
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
        GENERIC_ALL = 0x10000000
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
        PROCESS_ALL_ACCESS = AccessFlags.STANDARD_RIGHTS_REQUIRED | AccessFlags.SYNCHRONIZE | 0xFFFF
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
        LVIF_DI_SETITEM = 0x1000
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
        LVIS_STATEIMAGEMASK = 0xF000
    }

    [Flags]
    public enum SendMessageTimeoutFlags : uint
    {
        SMTO_NORMAL = 0x0000,
        SMTO_BLOCK = 0x0001,
        SMTO_ABORTIFHUNG = 0x0002,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
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
        RDW_NOFRAME = 0x0800
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

    [Flags]
    public enum DuplicateHandleOptions : uint
    {
        None = 0,
        DUPLICATE_CLOSE_SOURCE = 0x00000001,
        DUPLICATE_SAME_ACCESS = 0x00000002,
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

        public bool Contains(POINT pt)
        {
            return Left <= pt.X && pt.X <= Right && Top <= pt.Y && pt.Y <= Bottom;
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
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public BitmapCompressionMode biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public static readonly int SizeOf = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
    }

    #endregion

    #region callback delegates

    private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

    public delegate bool EnumThreadDelegate(IntPtr hwnd, IntPtr lParam);

    #endregion

    #region imports

    /// <summary>
    /// Creates a rectangular region with rounded corners. 
    /// </summary>
    /// <param name="x1">x-coordinate of upper-left corner</param>
    /// <param name="y1">y-coordinate of upper-left corner</param>
    /// <param name="x2">x-coordinate of lower-right corner</param>
    /// <param name="y2">y-coordinate of lower-right corner</param>
    /// <param name="cx">height of ellipse</param>
    /// <param name="cy">width of ellipse</param>
    /// <returns>
    /// If the function succeeds, the return value is the handle to the region.
    /// If the function fails, the return value is NULL. 
    /// </returns>
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

    /// <summary>
    /// Reserves or commits a region of memory within the virtual address space of a specified process. 
    /// The function initializes the memory it allocates to zero, unless MEM_RESET is used.
    /// </summary>
    /// <param name="hProcess">The handle to a process. The function allocates memory within the virtual address space of this process</param>
    /// <param name="lpAddress">The pointer that specifies a desired starting address for the region of pages that you want to allocate</param>
    /// <param name="dwSize">The size of the region of memory to allocate, in bytes</param>
    /// <param name="flAllocationType">The type of memory allocation. This parameter must contain one of the following values</param>
    /// <param name="flProtect">The memory protection for the region of pages to be allocated</param>
    /// <returns>
    /// If the function succeeds, the return value is the base address of the allocated region of pages.
    /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
       uint dwSize, VirtAllocTypes flAllocationType, AccessProtectFlags flProtect);

    /// <summary>
    /// Opens an existing local process object.
    /// </summary>
    /// <param name="dwDesiredAccess">The access to the process object. This access right is checked against the security descriptor for the process</param>
    /// <param name="bInheritHandle">If this value is TRUE, processes created by this process will inherit the handle</param>
    /// <param name="dwProcessId">The identifier of the local process to be opened</param>
    /// <returns>
    /// If the function succeeds, the return value is an open handle to the specified process.
    /// If the function fails, the return value is NULL. To get extended error information, call GetLastError
    /// </returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
       bool bInheritHandle, uint dwProcessId);

    /// <summary>
    /// The SendMessage function sends the specified message to a window or windows. 
    /// It calls the window procedure for the specified window and does not return until 
    /// the window procedure has processed the message. 
    /// </summary>
    /// <param name="hWnd">Handle to the window whose window procedure will receive the message</param>
    /// <param name="Msg">Specifies the message to be sent</param>
    /// <param name="wParam">Specifies additional message-specific information</param>
    /// <param name="lParam">Specifies additional message-specific information</param>
    /// <returns>The return value specifies the result of the message processing; it depends on the message sent</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    public static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Retrieves the identifier of the thread that created the specified window 
    /// and, optionally, the identifier of the process that created the window
    /// </summary>
    /// <param name="hWnd">Handle to the window</param>
    /// <param name="lpdwProcessId">Pointer to a variable that receives the process identifier</param>
    /// <returns>The return value is the identifier of the thread that created the window</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    /// <summary>
    /// Reads data from an area of memory in a specified process
    /// </summary>
    /// <param name="hProcess">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process from which to read</param>
    /// <param name="lpBuffer">A pointer to a buffer that receives the contents from the address space of the specified process</param>
    /// <param name="dwSize">The number of bytes to be read from the specified process</param>
    /// <param name="lpNumberOfBytesRead">
    /// A pointer to a variable that receives the number of bytes transferred into the specified buffer. 
    /// If lpNumberOfBytesRead is NULL, the parameter is ignored
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
        [In, Out] byte[] lpBuffer, uint dwSize, out uint lpNumberOfBytesRead);

    /// <summary>
    /// Reads data from an area of memory in a specified process
    /// </summary>
    /// <param name="hProcess">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process from which to read</param>
    /// <param name="lpBuffer">A pointer to a buffer that receives the contents from the address space of the specified process</param>
    /// <param name="dwSize">The number of bytes to be read from the specified process</param>
    /// <param name="lpNumberOfBytesRead">
    /// A pointer to a variable that receives the number of bytes transferred into the specified buffer. 
    /// If lpNumberOfBytesRead is NULL, the parameter is ignored
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
        [In, Out] int[] lpBuffer, uint dwSize, out uint lpNumberOfBytesRead);

    /// <summary>
    /// Reads data from an area of memory in a specified process
    /// </summary>
    /// <param name="hProcess">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process from which to read</param>
    /// <param name="lpBuffer">A pointer to a buffer that receives the contents from the address space of the specified process</param>
    /// <param name="dwSize">The number of bytes to be read from the specified process</param>
    /// <param name="lpNumberOfBytesRead">
    /// A pointer to a variable that receives the number of bytes transferred into the specified buffer. 
    /// If lpNumberOfBytesRead is NULL, the parameter is ignored
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
        IntPtr lpBuffer, uint dwSize, out uint lpNumberOfBytesRead);

    /// <summary>
    /// Writes data to an area of memory in a specified process
    /// </summary>
    /// <param name="hProcess">A handle to the process memory to be modified</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process to which data is written</param>
    /// <param name="lpBuffer">A pointer to the buffer that contains data to be written in the address space of the specified process</param>
    /// <param name="nSize">The number of bytes to be written to the specified process</param>
    /// <param name="lpNumberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
       [In, Out] byte[] lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

    /// <summary>
    /// Writes data to an area of memory in a specified process
    /// </summary>
    /// <param name="hProcess">A handle to the process memory to be modified</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process to which data is written</param>
    /// <param name="lpBuffer">A pointer to the buffer that contains data to be written in the address space of the specified process</param>
    /// <param name="nSize">The number of bytes to be written to the specified process</param>
    /// <param name="lpNumberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
       [In, Out] int[] lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

    /// <summary>
    /// Writes data to an area of memory in a specified process
    /// </summary>
    /// <param name="hProcess">A handle to the process memory to be modified</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process to which data is written</param>
    /// <param name="lpBuffer">A pointer to the buffer that contains data to be written in the address space of the specified process</param>
    /// <param name="nSize">The number of bytes to be written to the specified process</param>
    /// <param name="lpNumberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
       IntPtr lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

    /// <summary>
    /// Closes an open object handle
    /// </summary>
    /// <param name="hObject">A valid handle to an open object</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);

    /// <summary>
    /// Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process
    /// </summary>
    /// <param name="hProcess">A handle to a process</param>
    /// <param name="lpAddress">A pointer to the starting address of the region of memory to be freed</param>
    /// <param name="dwSize">The size of the region of memory to free, in bytes</param>
    /// <param name="dwFreeType">The type of free operation</param>
    /// <returns>If the function succeeds, the return value is a nonzero value</returns>
    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
       UIntPtr dwSize, VirtAllocTypes dwFreeType);

    /// <summary>
    /// Fills a block of memory with zeros
    /// </summary>
    /// <param name="dest">A pointer to the starting address of the block of memory to fill with zeros</param>
    /// <param name="size">The size of the block of memory to fill with zeros, in bytes</param>
    [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
    public static extern void ZeroMemory(IntPtr dest, uint size);

    /// <summary>
    /// Defines a new window message that is guaranteed to be unique throughout the system.
    /// The message value can be used when sending or posting messages
    /// </summary>
    /// <param name="lpString">Pointer to a null-terminated string that specifies the message to be registered</param>
    /// <returns>
    /// If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern uint RegisterWindowMessage(string lpString);

    /// <summary>
    /// Sends the specified message to one of more windows
    /// </summary>
    /// <param name="hWnd">
    /// Handle to the window whose window procedure will receive the message.
    /// If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system,
    /// including disabled or invisible unowned windows. The function does not return until each window has timed out. 
    /// Therefore, the total wait time can be up to the value of uTimeout multiplied by the number of top-level windows.
    /// </param>
    /// <param name="Msg">Specifies the message to be sent</param>
    /// <param name="wParam">Specifies additional message-specific information</param>
    /// <param name="lParam">Specifies additional message-specific information</param>
    /// <param name="fuFlags">Specifies how to send the message</param>
    /// <param name="uTimeout">Specifies the duration, in milliseconds, of the time-out period</param>
    /// <param name="lpdwResult">Receives the result of the message processing</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam,
        SendMessageTimeoutFlags fuFlags, uint uTimeout, out UIntPtr lpdwResult);

    /// <summary>
    /// Enumerates all nonchild windows associated with a thread by passing the handle to each window, 
    /// in turn, to an application-defined callback function
    /// </summary>
    /// <param name="dwThreadId">Identifies the thread whose windows are to be enumerated</param>
    /// <param name="lpfn">Pointer to an application-defined callback function</param>
    /// <param name="lParam">Specifies an application-defined value to be passed to the callback function</param>
    /// <returns>
    /// If the callback function returns TRUE for all windows in the thread specified by dwThreadId, the return value is TRUE.
    /// If the callback function returns FALSE on any enumerated window, or if there are no windows found in the thread 
    /// specified by dwThreadId, the return value is FALSE.
    /// </returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

    /// <summary>
    /// Retrieves the length, in characters, of the specified window's title bar text (if the window has a title bar)
    /// </summary>
    /// <param name="hWnd">Handle to the window or control</param>
    /// <returns>
    /// If the function succeeds, the return value is the length, in characters, of the text. 
    /// Under certain conditions, this value may actually be greater than the length of the text. 
    /// For more information, see the following Remarks section.If the window has no text, the return value is zero.
    /// To get extended error information, call GetLastError. 
    /// </returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    /// <summary>
    /// Copies the text of the specified window's title bar (if it has one) into a buffer
    /// </summary>
    /// <param name="hWnd">Handle to the window or control containing the text</param>
    /// <param name="lpString">Pointer to the buffer that will receive the text</param>
    /// <param name="nMaxCount">Specifies the maximum number of characters to copy to the buffer, including the NULL character</param>
    /// <returns>If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating NULL character</returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    /// <summary>
    /// Changes the text of the specified window's title bar (if it has one)
    /// </summary>
    /// <param name="hWnd">Handle to the window or control whose text is to be changed</param>
    /// <param name="lpString">Pointer to a null-terminated string to be used as the new title or control text</param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    public static bool SetWindowText(IntPtr hWnd, string lpString)
    {
        //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern bool SetWindowText(IntPtr hWnd, string lpString);
        // SetWindowText does not work in some cases, direct WM_SETTEXT works better
        IntPtr strPtr = IntPtr.Zero;
        try {
            strPtr = Marshal.StringToHGlobalUni(lpString);
            return IntPtr.Zero != SendMessage(hWnd, WindowsMessages.WM_SETTEXT, IntPtr.Zero, strPtr);
        } finally {
            if (strPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(strPtr);
        }
    }

    /// <summary>
    /// Retrieves the name of the class to which the specified window belongs
    /// </summary>
    /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs</param>
    /// <param name="lpClassName">Pointer to the buffer that is to receive the class name string</param>
    /// <param name="nMaxCount">Specifies the length, in TCHAR, of the buffer pointed to by the lpClassName parameter</param>
    /// <returns>
    /// If the function succeeds, the return value is the number of TCHAR copied to the specified buffer.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
    /// </returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    /// <summary>
    /// Determines whether the specified window handle identifies an existing window
    /// </summary>
    /// <param name="hWnd">Handle to the window to test</param>
    /// <returns>
    /// If the window handle identifies an existing window, the return value is nonzero.
    /// If the window handle does not identify an existing window, the return value is zero. 
    /// </returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindow(IntPtr hWnd);

    /// <summary>
    /// Retrieves the visibility state of the specified window
    /// </summary>
    /// <param name="hWnd">Handle to the window to test</param>
    /// <returns>If the specified window, its parent window, its parent's parent window, and so forth, 
    /// have the WS_VISIBLE style, the return value is nonzero. Otherwise, the return value is zero</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    /// <summary>
    /// Retrieves a handle to a window whose class name and window name match the specified strings.
    /// The function searches child windows, beginning with the one following the specified child window.
    /// This function does not perform a case-sensitive search
    /// </summary>
    /// <param name="hwndParent">Handle to the parent window whose child windows are to be searched</param>
    /// <param name="hwndChildAfter">Handle to a child window</param>
    /// <param name="lpszClass">
    /// Pointer to a null-terminated string that specifies the class name or a class atom
    /// created by a previous call to the RegisterClass or RegisterClassEx function
    /// </param>
    /// <param name="lpszWindow">
    /// Pointer to a null-terminated string that specifies the window name (the window's title). 
    /// If this parameter is NULL, all window names match
    /// </param>
    /// <returns>If the function succeeds, the return value is a handle to the window that has the specified class and window names</returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    /// <summary>
    /// Enumerates the child windows that belong to the specified parent window
    /// </summary>
    /// <param name="hWndParent">
    /// A handle to the parent window whose child windows are to be enumerated.
    /// If this parameter is NULL, this function is equivalent to EnumWindows
    /// </param>
    /// <param name="lpEnumFunc">A pointer to an application-defined callback function</param>
    /// <param name="lParam">An application-defined value to be passed to the callback function</param>
    /// <returns>The return value is not used</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowProc lpEnumFunc, IntPtr lParam);

    /// <summary>
    /// Performs a bit-block transfer of the color data corresponding to a rectangle of pixels 
    /// from the specified source device context into a destination device context
    /// </summary>
    /// <param name="hObject">Handle to the destination device context</param>
    /// <param name="nXDest">x-coord of destination upper-left corner</param>
    /// <param name="nYDest">y-coord of destination upper-left corner</param>
    /// <param name="nWidth">width of destination rectangle</param>
    /// <param name="nHeight">height of destination rectangle</param>
    /// <param name="hObjectSource">handle to source DC</param>
    /// <param name="nXSrc">x-coordinate of source upper-left corner</param>
    /// <param name="nYSrc">y-coordinate of source upper-left corner</param>
    /// <param name="dwRop">raster operation code</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("gdi32.dll")]
    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
        int nWidth, int nHeight, IntPtr hObjectSource,
        int nXSrc, int nYSrc, int dwRop);

    /// <summary>
    /// Creates a bitmap compatible with the device that is associated with the specified device context
    /// </summary>
    /// <param name="hDC">Handle to a device context</param>
    /// <param name="nWidth">width of bitmap, in pixels</param>
    /// <param name="nHeight">height of bitmap, in pixels</param>
    /// <returns>If the function succeeds, the return value is a handle to the compatible bitmap (DDB).</returns>
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

    /// <summary>
    /// Creates a memory device context (DC) compatible with the specified device
    /// </summary>
    /// <param name="hDC">
    /// Handle to an existing DC. 
    /// If this handle is NULL, the function creates a memory DC compatible with the application's current screen
    /// </param>
    /// <returns>If the function succeeds, the return value is the handle to a memory DC</returns>
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

    /// <summary>
    /// Deletes the specified device context (DC)
    /// </summary>
    /// <param name="hDC">Handle to the device context</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hDC);

    /// <summary>
    /// Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object
    /// </summary>
    /// <param name="hObject">Handle to a logical pen, brush, font, bitmap, region, or palette</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    /// <summary>
    /// Selects an object into the specified device context (DC). The new object replaces the previous object of the same type
    /// </summary>
    /// <param name="hDC">Handle to the DC</param>
    /// <param name="hObject">Handle to the object to be selected</param>
    /// <returns>If the selected object is not a region and the function succeeds, the return value is a handle to the object being replaced</returns>
    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    /// <summary>
    /// Places (posts) a message in the message queue associated with the thread that created the specified window 
    /// and returns without waiting for the thread to process the message
    /// </summary>
    /// <param name="hWnd">Handle to the window whose window procedure is to receive the message</param>
    /// <param name="Msg">Specifies the message to be posted</param>
    /// <param name="wParam">Specifies additional message-specific information</param>
    /// <param name="lParam">Specifies additional message-specific information</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Updates the client area of the specified window by sending a WM_PAINT message to the window
    /// if the window's update region is not empty
    /// </summary>
    /// <param name="hWnd">Handle to the window to be updated</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool UpdateWindow(IntPtr hWnd);

    /// <summary>
    /// Updates the specified rectangle or region in a window's client area
    /// </summary>
    /// <param name="hWnd">Handle to the window to be redrawn. If this parameter is NULL, the desktop window is updated</param>
    /// <param name="lprcUpdate">
    /// Pointer to a RECT structure containing the coordinates, in device units, of the update rectangle.
    /// This parameter is ignored if the hrgnUpdate parameter identifies a region
    /// </param>
    /// <param name="hrgnUpdate">
    /// Handle to the update region. If both the hrgnUpdate and lprcUpdate parameters are NULL,
    /// the entire client area is added to the update region
    /// </param>
    /// <param name="flags">Specifies one or more redraw flags</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawFlags flags);

    /// <summary>
    /// Sets the specified window's show state
    /// </summary>
    /// <param name="hWnd">Handle to the window</param>
    /// <param name="nCmdShow">Specifies how the window is to be shown</param>
    /// <returns>
    /// If the window was previously visible, the return value is nonzero. 
    /// If the window was previously hidden, the return value is zero. 
    /// </returns>
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

    /// <summary>
    /// Determines whether the specified window is minimized (iconic).
    /// </summary>
    /// <param name="hWnd">Handle to the window to test</param>
    /// <returns>
    /// If the window is iconic, the return value is nonzero.
    /// If the window is not iconic, the return value is zero.
    /// </returns>
    [DllImport("user32.dll")]
    public static extern bool IsIconic(IntPtr hWnd);

    /// <summary>
    /// Changes an attribute of the specified window. 
    /// The function also sets the 32-bit (long) value at the specified offset into the extra window memory
    /// </summary>
    /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs</param>
    /// <param name="nIndex">Specifies the zero-based offset to the value to be set</param>
    /// <param name="dwNewLong">Specifies the replacement value</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, WindowLongIndex nIndex, int dwNewLong);

    /// <summary>
    /// Retrieves information about the specified window.
    /// The function also retrieves the 32-bit (long) value at the specified offset into the extra window memory
    /// </summary>
    /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs</param>
    /// <param name="nIndex">Specifies the zero-based offset to the value to be retrieved</param>
    /// <returns>If the function succeeds, the return value is the requested 32-bit value</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, WindowLongIndex nIndex);

    /// <summary>
    /// Sets the opacity and transparency color key of a layered window
    /// </summary>
    /// <param name="hwnd">Handle to the layered window</param>
    /// <param name="crKey">COLORREF structure that specifies the transparency color key to be used when composing the layered window</param>
    /// <param name="bAlpha">Alpha value used to describe the opacity of the layered window</param>
    /// <param name="dwFlags">Specifies an action to take.</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    /// <summary>
    /// Converts the client-area coordinates of a specified point to screen coordinates
    /// </summary>
    /// <param name="hWnd">Handle to the window whose client area is used for the conversion</param>
    /// <param name="lpPoint">Pointer to a POINT structure that contains the client coordinates to be converted</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

    /// <summary>
    /// Converts the screen coordinates of a specified point on the screen to client-area coordinates
    /// </summary>
    /// <param name="hWnd">Handle to the window whose client area is used for the conversion</param>
    /// <param name="lpPoint">Pointer to a POINT structure that specifies the screen coordinates to be converted</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

    /// <summary>
    /// Retrieves the device context (DC) for the entire window, including title bar, menus, and scroll bars
    /// </summary>
    /// <param name="hWnd">
    /// Handle to the window with a device context that is to be retrieved. 
    /// If this value is NULL, GetWindowDC retrieves the device context for the entire screen
    /// </param>
    /// <returns>If the function succeeds, the return value is a handle to a device context for the specified window</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowDC(IntPtr hWnd);

    /// <summary>
    /// Retrieves a handle to a display device context (DC) for the client area of a specified window or for the entire screen
    /// </summary>
    /// <param name="hWnd">
    /// Handle to the window with a device context that is to be retrieved. 
    /// If this value is NULL, GetWindowDC retrieves the device context for the entire screen
    /// </param>
    /// <returns>If the function succeeds, the return value is a handle to the DC for the specified window's client area</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    /// <summary>
    /// Releases a device context (DC), freeing it for use by other applications
    /// </summary>
    /// <param name="hWnd">Handle to the window whose DC is to be released</param>
    /// <param name="hDC">Handle to the DC to be released</param>
    /// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

    /// <summary>
    /// Retrieves the dimensions of the bounding rectangle of the specified window
    /// </summary>
    /// <param name="hWnd">Handle to the window</param>
    /// <param name="lpRect">
    /// Pointer to a structure that receives the screen coordinates of the upper-left and 
    /// lower-right corners of the window
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    /// <summary>
    /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to a window</param>
    /// <param name="uCmd">The relationship between the specified window and the window whose handle is to be retrieved</param>
    /// <returns>If the function succeeds, the return value is a window handle</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCommands uCmd);

    /// <summary>
    /// Retrieves the coordinates of a window's client area. 
    /// The client coordinates specify the upper-left and lower-right corners of the client area. 
    /// Because client coordinates are relative to the upper-left corner of a window's client area, 
    /// the coordinates of the upper-left corner are (0,0). 
    /// </summary>
    /// <param name="hWnd">Handle to the window whose client coordinates are to be retrieved</param>
    /// <param name="lpRect">Pointer to a RECT structure that receives the client coordinates</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    /// <summary>
    /// Plays a waveform sound
    /// </summary>
    /// <param name="uType">The sound type</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool MessageBeep(uint uType);

    /// <summary>
    /// Puts the thread that created the specified window into the foreground and activates the window
    /// </summary>
    /// <param name="hWnd">Handle to the window that should be activated and brought to the foreground</param>
    /// <returns>If the window was brought to the foreground, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// Updates the position, size, shape, content, and translucency of a layered window
    /// </summary>
    /// <param name="hwnd">Handle to a layered window</param>
    /// <param name="hdcDst">Handle to a device context (DC) for the screen. This handle is obtained by specifying NULL when calling the function</param>
    /// <param name="pptDst">
    /// Pointer to a POINT structure that specifies the new screen position of the layered window. 
    /// If the current position is not changing, pptDst can be NULL
    /// </param>
    /// <param name="psize">
    /// Pointer to a SIZE structure that specifies the new size of the layered window. 
    /// If the size of the window is not changing, psize can be NULL. 
    /// If hdcSrc is NULL, psize must be NULL
    /// </param>
    /// <param name="hdcSrc">Handle to a DC for the surface that defines the layered window.</param>
    /// <param name="pprSrc">
    /// Pointer to a POINT structure that specifies the location of the layer in the device context.
    /// If hdcSrc is NULL, pptSrc should be NULL.
    /// </param>
    /// <param name="crKey">COLORREF structure that specifies the color key to be used when composing the layered window</param>
    /// <param name="pblend">Pointer to a BLENDFUNCTION structure that specifies the transparency value to be used when composing the layered window</param>
    /// <param name="dwFlags">Operation flags</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize,
        IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, UpdateLayeredFlags dwFlags);

    /// <summary>
    /// Retrieves a handle to the specified window's parent or owner
    /// </summary>
    /// <param name="hWnd">A handle to the window whose parent window handle is to be retrieved</param>
    /// <returns>
    /// If the window is a child window, the return value is a handle to the parent window.
    /// If the window is a top-level window with the WS_POPUP style, the return value is a handle to the owner window
    /// </returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetParent(IntPtr hWnd);

    /// <summary>
    /// Changes the parent window of the specified child window
    /// </summary>
    /// <param name="hWndChild">Handle to the child window</param>
    /// <param name="hWndNewParent">Handle to the new parent window. If this parameter is NULL, the desktop window becomes the new parent window</param>
    /// <returns>If the function succeeds, the return value is a handle to the previous parent window</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    /// <summary>
    /// Changes the size, position, and Z order of a child, pop-up, or top-level window
    /// </summary>
    /// <param name="hWnd">A handle to the window</param>
    /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order</param>
    /// <param name="X">Specifies the new position of the left side of the window, in client coordinates</param>
    /// <param name="Y">Specifies the new position of the top of the window, in client coordinates</param>
    /// <param name="cx">Specifies the new width of the window, in pixels</param>
    /// <param name="cy">Specifies the new height of the window, in pixels</param>
    /// <param name="uFlags">Specifies the window sizing and positioning flags</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
         int X, int Y, int cx, int cy, WindowPos uFlags);

    /// <summary>
    /// Retrieves a data handle from the property list of the specified window
    /// </summary>
    /// <param name="hWnd">Handle to the window whose property list is to be searched</param>
    /// <param name="lpString">Pointer to a null-terminated character string or contains an atom that identifies a string</param>
    /// <returns>
    /// If the property list contains the string, the return value is the associated data handle.
    /// Otherwise, the return value is NULL
    /// </returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetProp(IntPtr hWnd, string lpString);

    /// <summary>
    /// Adds a new entry or changes an existing entry in the property list of the specified window
    /// </summary>
    /// <param name="hWnd">Handle to the window whose property list receives the new entry</param>
    /// <param name="lpString">Pointer to a null-terminated string or contains an atom that identifies a string</param>
    /// <param name="hData">Handle to the data to be copied to the property list</param>
    /// <returns>If the data handle and string are added to the property list, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);

    /// <summary>
    /// This function removes an entry identified by a specified character string from the property list of a specified window
    /// </summary>
    /// <param name="hWnd">Handle to the window for which you want to change the property list</param>
    /// <param name="lpString">Pointer to a null-terminated string or an atom that identifies the string</param>
    /// <returns>
    /// The data handle associated with the specified string indicates that the property list contained the string,
    /// and that RemoveProp removed the association between the property and the window.
    /// NULL indicates that the specified string was not found in the specified property list
    /// </returns>
    [DllImport("user32.dll")]
    public static extern IntPtr RemoveProp(IntPtr hWnd, string lpString);

    /// <summary>
    /// Calculates the required size of the window rectangle, based on the desired size of the client rectangle
    /// </summary>
    /// <param name="lpRect">
    /// Pointer to a RECT structure that contains the coordinates of the top-left and bottom-right corners 
    /// of the desired client area. When the function returns, the structure contains the coordinates of the top-left 
    /// and bottom-right corners of the window to accommodate the desired client area. 
    /// </param>
    /// <param name="dwStyle">Specifies the window style of the window whose required size is to be calculated</param>
    /// <param name="bMenu">Specifies whether the window has a menu</param>
    /// <param name="dwExStyle">Specifies the extended window style of the window whose required size is to be calculated</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool AdjustWindowRectEx(ref RECT lpRect, WindowStyles dwStyle, bool bMenu, WindowExStyles dwExStyle);

    /// <summary>
    /// Enables or disables one or both scroll bar arrows
    /// </summary>
    /// <param name="hWnd">Handle to a window or a scroll bar control, depending on the value of the wSBflags parameter</param>
    /// <param name="wSBflags">Specifies the scroll bar type</param>
    /// <param name="wArrows">Specifies whether the scroll bar arrows are enabled or disabled and indicates which arrows are enabled or disabled</param>
    /// <returns>If the arrows are enabled or disabled as specified, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    public static extern bool EnableScrollBar(IntPtr hWnd, ScrollBarConstants wSBflags, EnableScrollBarFlags wArrows);

    /// <summary>
    /// Maps OEMASCII codes 0 through 0x0FF into the OEM scan codes and shift states
    /// </summary>
    /// <param name="wOemChar">The ASCII value of the OEM character</param>
    /// <returns>The low-order word of the return value contains the scan code of the OEM character, and the high-order word contains the shift state</returns>
    [DllImport("user32.dll")]
    public static extern uint OemKeyScan(ushort wOemChar);

    /// <summary>
    /// Retrieves a handle to the foreground window (the window with which the user is currently working)
    /// </summary>
    /// <returns>The return value is a handle to the foreground window</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    /// <summary>
    /// Attaches or detaches the input processing mechanism of one thread to that of another thread
    /// </summary>
    /// <param name="idAttach">The identifier of the thread to be attached to another thread</param>
    /// <param name="idAttachTo">The identifier of the thread to which idAttach will be attached</param>
    /// <param name="fAttach">If this parameter is TRUE, the two threads are attached. If the parameter is FALSE, the threads are detached</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

    /// <summary>
    /// Sets the keyboard focus to the specified window. The window must be attached to the calling thread's message queue
    /// </summary>
    /// <param name="hWnd">A handle to the window that will receive the keyboard input. If this parameter is NULL, keystrokes are ignored</param>
    /// <returns>If the function succeeds, the return value is the handle to the window that previously had the keyboard focus</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr SetFocus(IntPtr hWnd);

    /// <summary>
    /// Posts a message to the message queue of the specified thread.
    /// It returns without waiting for the thread to process the message
    /// </summary>
    /// <param name="threadId">The identifier of the thread to which the message is to be posted</param>
    /// <param name="msg">The type of message to be posted</param>
    /// <param name="wParam">Additional message-specific information</param>
    /// <param name="lParam">Additional message-specific information</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostThreadMessage(uint threadId, WindowsMessages msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Creates or opens a named or unnamed event object
    /// </summary>
    /// <param name="lpEventAttributes">A pointer to a SECURITY_ATTRIBUTES structure</param>
    /// <param name="bManualReset">
    /// If this parameter is TRUE, the function creates a manual-reset event object,
    /// which requires the use of the ResetEvent function to set the event state to nonsignaled
    /// </param>
    /// <param name="bInitialState">
    /// If this parameter is TRUE, the initial state of the event object is signaled; otherwise, it is nonsignaled
    /// </param>
    /// <param name="lpName">The name of the event object. The name is limited to MAX_PATH characters</param>
    /// <returns>If the function succeeds, the return value is a handle to the event object</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

    /// <summary>
    /// Sets the specified event object to the signaled state
    /// </summary>
    /// <param name="hEvent">A handle to the event object</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll")]
    public static extern bool SetEvent(IntPtr hEvent);

    /// <summary>
    /// Sets the specified event object to the nonsignaled state
    /// </summary>
    /// <param name="hEvent">A handle to the event object</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll")]
    public static extern bool ResetEvent(IntPtr hEvent);

    /// <summary>
    /// Duplicates an object handle
    /// </summary>
    /// <param name="hSourceProcessHandle">A handle to the process with the handle to be duplicated</param>
    /// <param name="hSourceHandle">The handle to be duplicated</param>
    /// <param name="hTargetProcessHandle">
    /// A handle to the process that is to receive the duplicated handle.
    /// The handle must have the PROCESS_DUP_HANDLE access right
    /// </param>
    /// <param name="lpTargetHandle">A pointer to a variable that receives the duplicate handle</param>
    /// <param name="dwDesiredAccess">The access requested for the new handle</param>
    /// <param name="bInheritHandle">A variable that indicates whether the handle is inheritable</param>
    /// <param name="dwOptions">Optional actions</param>
    /// <returns>If the function succeeds, the return value is nonzero</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle,
        IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
        uint dwDesiredAccess, bool bInheritHandle, DuplicateHandleOptions dwOptions);

    /// <summary>
    /// Retrieves a pseudo handle for the current process
    /// </summary>
    /// <returns>The return value is a pseudo handle to the current process</returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetCurrentProcess();

    /// <summary>
    /// Waits until the specified object is in the signaled state or the time-out interval elapses
    /// </summary>
    /// <param name="hHandle">A handle to the object</param>
    /// <param name="dwMilliseconds">
    /// The time-out interval, in milliseconds.
    /// If a nonzero value is specified, the function waits until the object is signaled or the interval elapses
    /// </param>
    /// <returns>If the function succeeds, the return value indicates the event that caused the function to return</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern WaitResult WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

    /// <summary>
    /// Retrieves a handle to the window that contains the specified point
    /// </summary>
    /// <param name="Point">The point to be checked</param>
    /// <returns>The return value is a handle to the window that contains the point</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(POINT Point);

    #endregion

    public static Point PointFromLParam(IntPtr value)
    {
        var val = value.ToInt32();
        return new Point {
            X = (short)(val & 0xffff),
            Y = (short)(val >> 16),
        };
    }

    public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

    public const byte AC_SRC_OVER = 0x00;
    public const byte AC_SRC_ALPHA = 0x01;

    public const int LWA_ALPHA = 0x2;
    public const int LWA_COLORKEY = 0x1;

    public static RECT GetClientRect(IntPtr hWnd)
    {
        RECT result;
        GetClientRect(hWnd, out result);
        return result;
    }

    public static RECT GetWindowRect(IntPtr hWnd)
    {
        RECT result;
        GetWindowRect(hWnd, out result);
        return result;
    }

    public static void MakeLayered(IntPtr hWnd)
    {
        var exStyle = GetWindowExStyle(hWnd);
        if ((exStyle & WindowExStyles.WS_EX_LAYERED) != WindowExStyles.WS_EX_LAYERED)
            return;
        SetWindowExStyle(hWnd, exStyle | WindowExStyles.WS_EX_LAYERED);
        SetLayeredWindowAttributes(hWnd, 0, 255, LWA_ALPHA);
    }

    public static IntPtr FindForeignWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow)
    {
        var result = hwndChildAfter;
        do {
            result = FindWindowEx(hwndParent, result, lpszClass, null);
            if (GetWndText(result) == lpszWindow)
                return result;
        } while (result != IntPtr.Zero);
        return result;
    }

    public static IntPtr FindWindowByHotPointDeep(IntPtr hwndParent, string lpszClass,
        int pointX, int pointY, bool findInvisible)
    {
        var pp = new POINT(pointX, pointY);
        ClientToScreen(hwndParent, ref pp);

        var result = new[] { IntPtr.Zero };
        EnumChildWindows(hwndParent, (hWnd, lParam) => {
            if (!findInvisible && !IsWindowVisible(hWnd))
                return true;
            if (GetClassName(hWnd) != lpszClass)
                return true;
            if (GetWindowRect(hWnd).Contains(pp)) {
                result[0] = hWnd;
                return false;
            }
            return true;
        }, IntPtr.Zero);
        return result[0];
    }

    public static IntPtr FindWindowByHotPoint(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
        int pointX, int pointY, bool findInvisible)
    {
        var pp = new POINT(pointX, pointY);
        ClientToScreen(hwndParent, ref pp);

        var result = hwndChildAfter;
        do {
            result = FindWindowEx(hwndParent, result, lpszClass, null);
            if (!findInvisible && !IsWindowVisible(result))
                continue;
            if (GetWindowRect(result).Contains(pp))
                return result;
        } while (result != IntPtr.Zero);
        return result;
    }

    public static IntPtr FindWindowByHotPoint(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
        int pointX, int pointY)
    {
        return FindWindowByHotPoint(hwndParent, hwndChildAfter, lpszClass, pointX, pointY, false);
    }

    public static string GetWndText(IntPtr hWnd)
    {
        // Allocate correct string length first
        var length = GetWindowTextLength(hWnd);
        var sb = new StringBuilder(length + 1);
        GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }

    public static string GetClassName(IntPtr hWnd)
    {
        var sb = new StringBuilder(512);
        GetClassName(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }

    public static IntPtr MakeLParam(int LoWord, int HiWord)
    {
        return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
    }

    public static IntPtr MakeKeyLParam(int keyCode, int oemCode, bool keyDown)
    {
        // lParam =
        // 0-15     The repeat count for the current message.
        // 16-23    The scan code. The value depends on the OEM.
        // 24       Indicates whether the key is an extended key, such as the right-hand ALT and CTRL
        // 25-28    Reserved; do not use.
        // 29       The context code. The value is always 0 for a WM_KEYDOWN message.
        // 30       The previous key state. The value is 1 if the key is down before the message is sent, or it is zero if the key is up.
        // 31       The transition state. The value is always 0 for a WM_KEYDOWN message.
        var lParam = 1 | (oemCode << 16) | (2 << 29);
        if (!keyDown) {
            // 29       The context code. The value is always 0 for a WM_KEYUP message.
            // 30       The previous key state. The value is always 1 for a WM_KEYUP message.
            // 31       The transition state. The value is always 1 for a WM_KEYUP message.
            lParam |= 6 << 29;
        }
        return (IntPtr)lParam;
    }

    public static void PostKeyStroke(IntPtr hWnd, int keyCode)
    {
        var oemCode = (int)(OemKeyScan((ushort)(0xFF & keyCode)) & 0xFFFF);
        // wParam = The virtual-key code of the nonsystem key. See Virtual-Key Codes.
        PostMessage(hWnd, WindowsMessages.WM_KEYDOWN, (IntPtr)keyCode, MakeKeyLParam(keyCode, oemCode, true));
        PostMessage(hWnd, WindowsMessages.WM_KEYUP, (IntPtr)keyCode, MakeKeyLParam(keyCode, oemCode, false));
    }

    public static void PostKeyChar(IntPtr hWnd, char keyChar)
    {
        var keyCode = char.ToUpperInvariant(keyChar) & 0xFF;
        var oemCode = (int)(OemKeyScan((ushort)keyCode) & 0xFFFF);
        PostMessage(hWnd, WindowsMessages.WM_CHAR, (IntPtr)keyChar, MakeKeyLParam(keyCode, oemCode, true));
    }

    private static readonly BLENDFUNCTION PredefinedBlend = new BLENDFUNCTION {
        BlendOp = AC_SRC_OVER,
        BlendFlags = 0,
        SourceConstantAlpha = 255,
        AlphaFormat = AC_SRC_ALPHA
    };

    private static readonly Color ZeroColor = Color.FromArgb(0);

    /// <summary>
    /// Updates shape and content of a layered window
    /// </summary>
    /// <param name="handle">Handle to a layered window</param>
    /// <param name="bitmap">New content of the window. Must be 32bppArgb bitmap</param>
    public static void UpdateLayeredWindow(IntPtr handle, Bitmap bitmap)
    {
        var thisDc = GetWindowDC(handle);
        var bmpDc = CreateCompatibleDC(thisDc);
        var hBitmap = IntPtr.Zero;
        var oldBitmap = IntPtr.Zero;

        try {
            hBitmap = bitmap.GetHbitmap(ZeroColor);  // grab a GDI handle from this GDI+ bitmap
            oldBitmap = SelectObject(bmpDc, hBitmap);

            var size = new SIZE(bitmap.Size);
            var pointSource = new POINT(0, 0);
            var topPos = new POINT(GetWindowRect(handle).Location);

            var blendFunc = PredefinedBlend; // copy blendfunction

            UpdateLayeredWindow(handle, IntPtr.Zero, ref topPos, ref size, bmpDc, ref pointSource, 0,
                ref blendFunc, UpdateLayeredFlags.ULW_ALPHA);
        } finally {
            ReleaseDC(handle, thisDc);
            if (hBitmap != IntPtr.Zero) {
                SelectObject(bmpDc, oldBitmap);
                DeleteObject(hBitmap);
            }
            DeleteDC(bmpDc);
        }
    }

    /// <summary>
    /// Changes the owner window of the specified child(owned) window. 
    /// DO NOT confuse with PARENT which is set by <see cref="SetParent"/>!
    /// </summary>
    /// <param name="hWnd">Handle to the owned window</param>
    /// <param name="hWndNewOwner">Handle to the new owner window.</param>
    public static void SetWindowOwner(IntPtr hWnd, IntPtr hWndNewOwner)
    {
        SetWindowLong(hWnd, WindowLongIndex.GWL_HWNDPARENT, (int)hWndNewOwner);
    }

    public static WindowStyles GetWindowStyle(IntPtr hWnd)
    {
        return (WindowStyles)GetWindowLong(hWnd, WindowLongIndex.GWL_STYLE);
    }

    public static WindowExStyles GetWindowExStyle(IntPtr hWnd)
    {
        return (WindowExStyles)GetWindowLong(hWnd, WindowLongIndex.GWL_EXSTYLE);
    }

    public static WindowStyles SetWindowStyle(IntPtr hWnd, WindowStyles newStyle)
    {
        return (WindowStyles)SetWindowLong(hWnd, WindowLongIndex.GWL_STYLE, (int)newStyle);
    }

    public static WindowExStyles SetWindowExStyle(IntPtr hWnd, WindowExStyles newStyle)
    {
        return (WindowExStyles)SetWindowLong(hWnd, WindowLongIndex.GWL_EXSTYLE, (int)newStyle);
    }

    public static IntPtr CreateRoundRectRgn(Rectangle rect, int rx, int ry)
    {
        return CreateRoundRectRgn(rect.Left, rect.Top, rect.Right, rect.Bottom, rx, ry);
    }

    public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Rectangle rect, WindowPos uFlags)
    {
        return SetWindowPos(hWnd, hWndInsertAfter, rect.X, rect.Y, rect.Width, rect.Height, uFlags);
    }

    public static void ForeachWindow(this Process process, Func<IntPtr, bool> callback)
    {
        foreach (ProcessThread th in process.Threads)
            EnumThreadWindows((uint)th.Id, (IntPtr hwnd, IntPtr lParam) => callback(hwnd), IntPtr.Zero);
    }

    public static IntPtr FindWindow(this Process process, Func<IntPtr, bool> callback)
    {
        var result = new[] { IntPtr.Zero };
        foreach (ProcessThread th in process.Threads)
            EnumThreadWindows((uint)th.Id, (IntPtr hwnd, IntPtr lParam) => {
                if (callback(hwnd)) {
                    result[0] = hwnd;
                    return false;
                }
                return true;
            }, IntPtr.Zero);
        return result[0];
    }

    public static IEnumerable<IntPtr> EnumerateWindows(Process process)
    {
        var result = new List<IntPtr>();
        foreach (ProcessThread th in process.Threads)
            EnumThreadWindows((uint)th.Id, (IntPtr hwnd, IntPtr lParam) => {
                result.Add(hwnd);
                return true;
            }, IntPtr.Zero);
        return result;
    }

    /// <summary>
    /// Simplified version of GetWindowThreadProcessId
    /// </summary>
    /// <param name="hWnd">Handle to the window</param>
    /// <returns>The return value is the identifier of the thread that created the window</returns>
    public static uint GetWindowThreadId(IntPtr hWnd)
    {
        uint ignored;
        return GetWindowThreadProcessId(hWnd, out ignored);
    }

    public static Point ClientToScreen(IntPtr hWnd, Point point)
    {
        var tmp = (POINT)point;
        ClientToScreen(hWnd, ref tmp);
        return (Point)tmp;
    }

    public static Point ScreenToClient(IntPtr hWnd, Point point)
    {
        var tmp = (POINT)point;
        ScreenToClient(hWnd, ref tmp);
        return (Point)tmp;
    }

    public static void SetWindowSize(IntPtr hWnd, Size size)
    {
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, size.Width, size.Height,
            WindowPos.SWP_NOACTIVATE | WindowPos.SWP_NOMOVE | WindowPos.SWP_NOZORDER);
    }

    public static void SetWindowClientSize(IntPtr hWnd, Size clientSize)
    {
        var rect = new RECT(0, 0, clientSize.Width, clientSize.Height);
        AdjustWindowRectEx(ref rect, GetWindowStyle(hWnd), false, GetWindowExStyle(hWnd));
        var currRect = GetWindowRect(hWnd);
        if (currRect.Width == rect.Width && currRect.Height == rect.Height)
            return;
        SetWindowSize(hWnd, rect.Size);
        RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, RedrawFlags.RDW_INVALIDATE | RedrawFlags.RDW_UPDATENOW);
    }
}
