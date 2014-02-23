/*
This file is put in Public Domain by Anton A. Drachev, 2009

A fresh version of this file you can found at: http://drachev.com/winapi/
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

#if (!NET_2_0)
//using System.Diagnostics;
#endif

#if (mshtml)
using mshtml;
#endif
namespace Winapi
{
    public static class Methods
    {
 

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
            ref LVITEM lpBuffer, uint dwSize, out uint lpNumberOfBytesRead);

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
           ref LVITEM lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

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
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings.
        /// This function does not search child windows. This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="lpClassName">
        /// The window class name. The class name can be any name registered with RegisterClass or RegisterClassEx,
        /// or any of the predefined control-class names. 
        /// If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter. 
        /// </param>
        /// <param name="lpWindowName">The window name (the window's title). If this parameter is NULL, all window names match.</param>
        /// <returns>If the function succeeds, the return value is a handle to the window that has the specified class name and window name</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

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

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

#if (mshtml)
        [DllImport("oleacc.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern object ObjectFromLresult(UIntPtr lResult,
             [MarshalAs(UnmanagedType.LPStruct)] Guid refiid, IntPtr wParam);
#endif

        #endregion

        public static Point PointFromLParam(IntPtr value)
        {
            var val = value.ToInt32();
            return new Point {
                X = (short)(val & 0xffff),
                Y = (short)(val >> 16),
            };
        }

        public delegate bool EnumThreadDelegate(IntPtr hwnd, IntPtr lParam);




        /*public static IntPtr FindForeignWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow)
        {
            var result = hwndChildAfter;
            do {
                result = FindWindowEx(hwndParent, result, lpszClass, null);
                if (lpszWindow == null || GetWindowText(result, lpszWindow.Length) == lpszWindow)
                    return result;
            } while (result != IntPtr.Zero);
            return result;
        }
        */
        public static IntPtr FindWindowByHotPoint(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            int pointX, int pointY, bool findInvisible)
        {
            var pp = new POINT(0, 0);
            ClientToScreen(hwndParent, ref pp);

            Window result = hwndChildAfter;
            do {
                result = FindWindowEx(hwndParent, result, lpszClass, null);
                if (!findInvisible && !WindowBase.IsWindowVisible(result))
                    continue;
                var r = result.Rect;
                r.X -= pp.X;
                r.Y -= pp.Y;
                if (r.Contains(pointX, pointY))
                    return result;
            } while (result != IntPtr.Zero);
            return result;
        }

        public static IntPtr FindWindowByHotPoint(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            int pointX, int pointY)
        {
            return FindWindowByHotPoint(hwndParent, hwndChildAfter, lpszClass, pointX, pointY, false);
        }

        public static IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        public static IntPtr CreateRoundRectRgn(Rectangle rect, int rx, int ry)
        {
            return CreateRoundRectRgn(rect.Left, rect.Top, rect.Right, rect.Bottom, rx, ry);
        }

        public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Rectangle rect, WindowPos uFlags)
        {
            return SetWindowPos(hWnd, hWndInsertAfter, rect.X, rect.Y, rect.Width, rect.Height, uFlags);
        }

#if (mshtml)

    public static IHTMLDocument2 GetIEDocumentFromWindowHandle(IntPtr hWnd)
    {
        var MSG = RegisterWindowMessage("WM_HTML_GETOBJECT");
        UIntPtr lRes;
        SendMessageTimeout(hWnd, MSG, UIntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out lRes);
        try {
            var doc = ObjectFromLresult(lRes, typeof(IHTMLDocument2).GUID, IntPtr.Zero) as IHTMLDocument2;
            return doc;
        } catch {
            return null;
        }
    }

#endif

#if (!NET_2_0)

        private sealed class ForeachWindowCallbackClass
        {
            readonly Process process;
            readonly Func<IntPtr, bool> callback;
            IntPtr result;
            public ForeachWindowCallbackClass(Process process, Func<IntPtr, bool> callback)
            {
                this.callback = callback;
                this.process = process;
            }

            private bool Callback(IntPtr hwnd, IntPtr lParam)
            {
                return callback(hwnd);
            }

            private bool CallbackFind(IntPtr hwnd, IntPtr lParam)
            {
                if (callback(hwnd)) {
                    result = hwnd;
                    return false;
                }
                return true;
            }

            public void Foreach()
            {
                foreach (ProcessThread th in process.Threads)
                    EnumThreadWindows((uint)th.Id, Callback, IntPtr.Zero);
            }

            public IntPtr Find()
            {
                foreach (ProcessThread th in process.Threads)
                    EnumThreadWindows((uint)th.Id, CallbackFind, IntPtr.Zero);
                return result;
            }
        }

        public static void ForeachWindow(this Process process, Func<IntPtr, bool> callback)
        {
            var c = new ForeachWindowCallbackClass(process, callback);
            c.Foreach();
        }

        public static IntPtr FindWindow(this Process process, Func<IntPtr, bool> callback)
        {
            var c = new ForeachWindowCallbackClass(process, callback);
            return c.Find();
        }

#endif

        private sealed class EnumerateWindowsCallbackClass
        {
            readonly List<IntPtr> result;

            internal bool Callback(IntPtr hwnd, IntPtr lParam)
            {
                result.Add(hwnd);
                return true;
            }

            internal EnumerateWindowsCallbackClass(List<IntPtr> result)
            {
                this.result = result;
            }
        }

        public static IEnumerable<IntPtr> EnumerateWindows(Process process)
        {
            var result = new List<IntPtr>();

            var cClass = new EnumerateWindowsCallbackClass(result);

            foreach (ProcessThread th in process.Threads)
                EnumThreadWindows((uint)th.Id, cClass.Callback, IntPtr.Zero);

            return result;
        }

        public static void SetWindowSize(IntPtr wnd, Size size)
        {
            SetWindowPos(wnd, IntPtr.Zero, 0, 0, size.Width, size.Height,
                WindowPos.SWP_NOACTIVATE | WindowPos.SWP_NOMOVE | WindowPos.SWP_NOZORDER);
        }
    }
}