/*
This file is put in Public Domain by Anton A. Drachev, 2009

A fresh version of this file you can found at: http://drachev.com/winapi/
*/

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace Winapi
{
    using M = Methods;

    [DebuggerDisplay("Window #{Hex(handle),nq}")]
    public abstract class WindowBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected readonly IntPtr handle;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected string className;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int windowThreadProcessId;


        protected WindowBase(IntPtr handle)
        {
            this.handle = handle;
        }

        public IntPtr Handle
        {
            get { return handle; }
        }

        public WindowStyles Style
        {
            get { return (WindowStyles)GetWindowLong(handle, WindowLongIndex.GWL_STYLE); }
            set { SetWindowLong(handle, WindowLongIndex.GWL_STYLE, (int)value); }
        }

        public WindowExStyles ExStyle
        {
            get { return (WindowExStyles)GetWindowLong(handle, WindowLongIndex.GWL_EXSTYLE); }
            set { SetWindowLong(handle, WindowLongIndex.GWL_EXSTYLE, (int)value); }
        }

        public bool Layered
        {
            get { return (ExStyle & WindowExStyles.WS_EX_LAYERED) == WindowExStyles.WS_EX_LAYERED; }
            set
            {
                var exStyle = ExStyle;
                var layered = (ExStyle & WindowExStyles.WS_EX_LAYERED) == WindowExStyles.WS_EX_LAYERED;
                if (layered != value) {
                    if (value) {
                        ExStyle = exStyle | WindowExStyles.WS_EX_LAYERED;
                        M.SetLayeredWindowAttributes(handle, 0, 255, LWA_ALPHA);
                    } else {
                        ExStyle = exStyle & ~WindowExStyles.WS_EX_LAYERED;
                    }
                }
            }
        }

        public string Text
        {
            get { return GetText(0); }
            set { SendMessage(handle, WindowsMessages.WM_SETTEXT, IntPtr.Zero, value); }
        }

        public string ClassName
        {
            get
            {
                if (className == null) {
                    var sb = new StringBuilder(1024);
                    GetClassName(handle, sb, sb.Capacity);
                    className = sb.ToString();
                }
                return className;
            }
        }

        /// <summary>
        /// Changes the owner window of the specified child(owned) window. 
        /// DO NOT confuse with PARENT which is set by <see cref="Parent"/>!
        /// </summary>
        [DebuggerDisplay("0x{Hex(Owner),nq}")]
        public IntPtr Owner
        {
            get { return GetWindow(handle, GetWindowCmd.GW_OWNER); }
            set { SetWindowLong(handle, WindowLongIndex.GWL_HWNDPARENT, (int)value); }
        }

        [DebuggerDisplay("0x{Hex(Parent),nq}")]
        public IntPtr Parent
        {
            get
            {
                var ancestor = GetAncestor(handle, GetAncestorFlags.GA_PARENT);
                return ancestor == GetDesktopWindow() ? IntPtr.Zero : ancestor;
            }
            set { SetParent(handle, value); }
        }

        public int WindowThreadProcessId
        {
            get
            {
                if (windowThreadProcessId == 0) {
                    GetWindowThreadProcessId(handle, out windowThreadProcessId);
                }
                return windowThreadProcessId;
            }
        }

        public bool IsWindow
        {
            get { return IsWindow_Func(handle); }
        }

        public bool Visible
        {
            get { return IsWindowVisible(handle); }
            set { ShowWindow(handle, value ? ShowWindowCommands.SW_SHOW : ShowWindowCommands.SW_HIDE); }
        }

        public Rectangle ClientRect
        {
            get
            {
                RECT result;
                GetClientRect(handle, out result);
                return result;
            }
        }

        public Rectangle Rect
        {
            get
            {
                RECT result;
                GetWindowRect(handle, out result);
                return result;
            }
        }

        /// <summary>
        /// Properties, handled by SetProp/GetProp
        /// </summary>
        /// <param name="key">property name</param>
        /// <returns></returns>
        public IntPtr this[string key]
        {
            get { return GetProp(handle, key); }
            set { SetProp(handle, key, value); }
        }

        public FormWindowState WindowState
        {
            get
            {
                var placement = WINDOWPLACEMENT.Create();
                GetWindowPlacement(handle, ref placement);
                switch (placement.showCmd) {
                    case ShowWindowCommands.SW_RESTORE:
                    case ShowWindowCommands.SW_SHOW:
                    case ShowWindowCommands.SW_SHOWNA:
                    case ShowWindowCommands.SW_SHOWNOACTIVATE:
                    case ShowWindowCommands.SW_NORMAL:
                        return FormWindowState.Normal;
                    case ShowWindowCommands.SW_MAXIMIZE:
                        return FormWindowState.Maximized;
                    case ShowWindowCommands.SW_HIDE:
                    case ShowWindowCommands.SW_SHOWMINIMIZED:
                    case ShowWindowCommands.SW_MINIMIZE:
                    case ShowWindowCommands.SW_SHOWMINNOACTIVE:
                        return FormWindowState.Minimized;
                }
                return FormWindowState.Normal;
            }
            set
            {
                switch (value) {
                    case FormWindowState.Normal:
                        ShowWindow(ShowWindowCommands.SW_NORMAL);
                        break;
                    case FormWindowState.Maximized:
                        ShowWindow(ShowWindowCommands.SW_MAXIMIZE);
                        break;
                    case FormWindowState.Minimized:
                        ShowWindow(ShowWindowCommands.SW_MINIMIZE);
                        break;
                }
            }
        }

        public bool ShowWindow(ShowWindowCommands nCmdShow)
        {
            return ShowWindow(handle, nCmdShow);
        }

        public bool Move(Rectangle newRect, bool repaint)
        {
            return MoveWindow(handle, newRect.X, newRect.Y, newRect.Width, newRect.Height, repaint);
        }

        public string GetText(int expectedLength)
        {
            var length = Math.Max(GetWindowTextLength(handle), expectedLength);
            if (length == 0)
                return "";
            length++;
            var sb = new StringBuilder(length);
            SendMessage(handle, WindowsMessages.WM_GETTEXT, (IntPtr)sb.Capacity, sb);
            return sb.ToString();
        }

        public void Close()
        {
            M.SendMessage(handle, WindowsMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        #region layered window stuff

        protected const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

        protected const byte AC_SRC_OVER = 0x00;
        protected const byte AC_SRC_ALPHA = 0x01;

        protected const int LWA_ALPHA = 0x2;
        protected const int LWA_COLORKEY = 0x1;

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
        /// <param name="bitmap">New content of the window. Must be 32bppArgb bitmap</param>
        public void UpdateLayeredWindow(Bitmap bitmap)
        {
            var thisDc = M.GetWindowDC(handle);
            var bmpDc = M.CreateCompatibleDC(thisDc);
            var hBitmap = IntPtr.Zero;
            var oldBitmap = IntPtr.Zero;

            try {
                hBitmap = bitmap.GetHbitmap(ZeroColor);  // grab a GDI handle from this GDI+ bitmap
                oldBitmap = M.SelectObject(bmpDc, hBitmap);

                var size = new SIZE(bitmap.Size);
                var pointSource = new POINT(0, 0);
                var topPos = new POINT(Rect.Location);

                var blendFunc = PredefinedBlend; // copy blendfunction

                UpdateLayeredWindow(handle, IntPtr.Zero, ref topPos, ref size, bmpDc, ref pointSource, 0,
                    ref blendFunc, UpdateLayeredFlags.ULW_ALPHA);
            } finally {
                M.ReleaseDC(handle, thisDc);
                if (hBitmap != IntPtr.Zero) {
                    M.SelectObject(bmpDc, oldBitmap);
                    M.DeleteObject(hBitmap);
                }
                M.DeleteDC(bmpDc);
            }
        }

        #endregion //layered window stuff

        public void KeyStroke(Keys keyCode, char keyChar, int repeatCount)
        {
            var kc = (IntPtr)keyCode;
            //Bits  | Meaning 
            //0-15  | The repeat count for the current message, 1 for a WM_KEYUP 
            //16-23 | The scan code. The value depends on the OEM. 
            //24    | Indicates whether the key is an extended key 
            //24    | Indicates whether the key is an extended key 
            //29    | The context code. 0 for a WM_KEYDOWN
            //30    | The previous key state. 1 for a WM_KEYUP
            //31    | The transition state. 0 for a WM_KEYDOWN, 1 for a WM_KEYUP. 
            repeatCount = 0xFFFF & repeatCount;
            const int keyUpFlags = 1 | 1 << 30 | 1 << 31;
            M.SendMessage(handle, WindowsMessages.WM_KEYDOWN, kc, (IntPtr)repeatCount);
            if (keyChar != char.MinValue)
                M.SendMessage(handle, WindowsMessages.WM_CHAR, kc, (IntPtr)repeatCount);
            M.SendMessage(handle, WindowsMessages.WM_KEYUP, kc, (IntPtr)keyUpFlags);
        }


        public void KeyStroke(Keys keyCode, char keyChar)
        {
            KeyStroke(keyCode, keyChar, 1);
        }

        public void KeyStroke(Keys keyCode)
        {
            /*
send char: TAB,ESC,backsp...etc
doesn't send: caps, shift, ctrl, arrows, menu,win, control block, scroll, pause, Fkeys
sends only keyup: vk_snapshot
             */
            KeyStroke(keyCode, char.MinValue, 1);
        }

        #region Debugger support
        // ReSharper disable UnusedMember.Local
        private string Hex(IntPtr val)
        {
            return val.ToString("X8");
        }
        // ReSharper restore UnusedMember.Local
        #endregion

        #region imports
        /// <summary>
        /// Specialized version of SendMessage for text-related functions
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        protected static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        protected static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, string lParam);

        /// <summary>
        /// Retrieves the identifier of the thread that created the specified window 
        /// and, optionally, the identifier of the process that created the window
        /// </summary>
        /// <param name="hWnd">Handle to the window</param>
        /// <param name="lpdwProcessId">Pointer to a variable that receives the process identifier</param>
        /// <returns>The return value is the identifier of the thread that created the window</returns>
        [DllImport("user32.dll", SetLastError = true)]
        protected static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

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
        protected static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// Copies the text of the specified window's title bar (if it has one) into a buffer
        /// </summary>
        /// <param name="hWnd">Handle to the window or control containing the text</param>
        /// <param name="lpString">Pointer to the buffer that will receive the text</param>
        /// <param name="nMaxCount">Specifies the maximum number of characters to copy to the buffer, including the NULL character</param>
        /// <returns>If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating NULL character</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /*/// <summary>
        /// Changes the text of the specified window's title bar (if it has one)
        /// </summary>
        /// <param name="hWnd">Handle to the window or control whose text is to be changed</param>
        /// <param name="lpString">Pointer to a null-terminated string to be used as the new title or control text</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        protected static extern bool SetWindowText(IntPtr hWnd, string lpString);*/

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
        protected static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// Determines whether the specified window handle identifies an existing window
        /// </summary>
        /// <param name="hWnd">Handle to the window to test</param>
        /// <returns>
        /// If the window handle identifies an existing window, the return value is nonzero.
        /// If the window handle does not identify an existing window, the return value is zero. 
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow_Func(IntPtr hWnd);

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
        /// Sets the specified window's show state
        /// </summary>
        /// <param name="hWnd">Handle to the window</param>
        /// <param name="nCmdShow">Specifies how the window is to be shown</param>
        /// <returns>
        /// If the window was previously visible, the return value is nonzero. 
        /// If the window was previously hidden, the return value is zero. 
        /// </returns>
        [DllImport("user32.dll")]
        protected static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

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
        /// Changes the parent window of the specified child window
        /// </summary>
        /// <param name="hWndChild">Handle to the child window</param>
        /// <param name="hWndNewParent">Handle to the new parent window. If this parameter is NULL, the desktop window becomes the new parent window</param>
        /// <returns>If the function succeeds, the return value is a handle to the previous parent window</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>
        /// Retrieves a handle to the specified window's parent or owner
        /// </summary>
        /// <param name="hWnd">A handle to the window whose parent window handle is to be retrieved</param>
        /// <returns>
        /// If the window is a child window, the return value is a handle to the parent window. 
        /// If the window is a top-level window, the return value is a handle to the owner window.
        /// If the window is a top-level unowned window or if the function fails, the return value is NULL.
        /// </returns>
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// Retrieves the handle to the ancestor of the specified window
        /// </summary>
        /// <param name="hwnd">A handle to the window whose ancestor is to be retrieved. If this parameter is the desktop window, the function returns NULL</param>
        /// <param name="gaFlags">The ancestor to be retrieved</param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags gaFlags);

        /// <summary>
        /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window
        /// </summary>
        /// <param name="hWnd">A handle to a window</param>
        /// <param name="uCmd">The relationship between the specified window and the window whose handle is to be retrieved</param>
        /// <returns>If the function succeeds, the return value is a window handle</returns>
        [DllImport("user32.dll", SetLastError = true)]
        protected static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        [DllImport("user32.dll", SetLastError = false)]
        protected static extern IntPtr GetDesktopWindow();

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
        /// Changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area. 
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="X">Specifies the new position of the left side of the window.</param>
        /// <param name="Y">Specifies the new position of the top of the window.</param>
        /// <param name="nWidth">Specifies the new width of the window.</param>
        /// <param name="nHeight">Specifies the new height of the window.</param>
        /// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

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
        private static extern IntPtr GetProp(IntPtr hWnd, string lpString);

        /// <summary>
        /// Adds a new entry or changes an existing entry in the property list of the specified window
        /// </summary>
        /// <param name="hWnd">Handle to the window whose property list receives the new entry</param>
        /// <param name="lpString">Pointer to a null-terminated string or contains an atom that identifies a string</param>
        /// <param name="hData">Handle to the data to be copied to the property list</param>
        /// <returns>If the data handle and string are added to the property list, the return value is nonzero</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);

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
        private static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize,
            IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, UpdateLayeredFlags dwFlags);

        /// <summary>
        /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="lpwndpl">
        /// A pointer to the WINDOWPLACEMENT structure that receives the show state and position information.
        /// Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-> length is not set correctly.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        #endregion

        public Point ClientToScreen(Point point)
        {
            var tmp = (POINT)point;
            M.ClientToScreen(handle, ref tmp);
            return (Point)tmp;
        }

        public Point ScreenToClient(Point point)
        {
            var tmp = (POINT)point;
            M.ScreenToClient(handle, ref tmp);
            return (Point)tmp;
        }
    }
}