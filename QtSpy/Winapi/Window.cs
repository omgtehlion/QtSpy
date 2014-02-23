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


namespace Winapi
{
    using M = Methods;

    [DebuggerDisplay("Window #{Hex(handle),nq}")]
    public sealed class Window : WindowBase
    {
        public Window(IntPtr handle)
            : base(handle)
        {
        }

        public static IEnumerable<IntPtr> Find(string lpClassName, string lpWindowName)
        {
            return FindEx(IntPtr.Zero, IntPtr.Zero, lpClassName, lpWindowName);
        }

        public static IEnumerable<IntPtr> FindEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName)
        {
            var last = hwndChildAfter;
            do {
                last = M.FindWindowEx(hwndParent, last, lpClassName, lpWindowName);
                if (last != IntPtr.Zero)
                    yield return last;
            } while (last != IntPtr.Zero);
        }

        public static IEnumerable<Window> FindWindows(string lpClassName, string lpWindowName)
        {
            return FindWindowsEx(IntPtr.Zero, IntPtr.Zero, lpClassName, lpWindowName);
        }

        public static IEnumerable<Window> FindWindowsEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName)
        {
            var last = hwndChildAfter;
            do {
                last = M.FindWindowEx(hwndParent, last, lpClassName, lpWindowName);
                if (last != IntPtr.Zero)
                    yield return last;
            } while (last != IntPtr.Zero);
        }

        public static implicit operator Window(IntPtr handle) { return new Window(handle); }
        public static implicit operator IntPtr(Window value) { return value.handle; }
    }
}