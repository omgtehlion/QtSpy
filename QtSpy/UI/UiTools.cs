using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace QtSpy.UI
{
    class UiTools
    {
        const int INVERT_BORDER = 3;

        [DllImport("user32.dll")]
        static extern bool InvertRect(IntPtr hDC, [In] ref Winapi.RECT lprc);

        [DllImport("user32.dll")]
        static extern bool OffsetRect(ref Winapi.RECT lprc, int dx, int dy);

        [DllImport("user32.dll")]
        static extern bool SetRect(out Winapi.RECT lprc, int xLeft, int yTop, int xRight, int yBottom);

        public static void InvertWindow(IntPtr hwnd, bool fShowHidden)
        {
            Winapi.RECT rect2;

            var border = INVERT_BORDER;

            //window rectangle (screen coords)
            var rect = Winapi.GetWindowRect(hwnd);

            //client rectangle (screen coords)
            var rectc = Winapi.GetClientRect(hwnd);

            var tmp = Winapi.ClientToScreen(hwnd, rectc.Location);
            rectc.Left = tmp.X;
            rectc.Top = tmp.Y;

            var x1 = rect.Left;
            var y1 = rect.Top;
            OffsetRect(ref rect, -x1, -y1);
            OffsetRect(ref rectc, -x1, -y1);

            if (rect.Bottom - border * 2 < 0)
                border = 1;

            if (rect.Right - border * 2 < 0)
                border = 1;

            if (fShowHidden)
                hwnd = IntPtr.Zero;

            var hdc = Winapi.GetWindowDC(hwnd);

            //top edge
            SetRect(out rect2, 0, 0, rect.Right, border);
            if (fShowHidden)
                OffsetRect(ref rect2, x1, y1);
            InvertRect(hdc, ref rect2);

            //left edge
            SetRect(out rect2, 0, border, border, rect.Bottom);
            if (fShowHidden)
                OffsetRect(ref rect2, x1, y1);
            InvertRect(hdc, ref rect2);

            //right edge
            SetRect(out rect2, border, rect.Bottom - border, rect.Right, rect.Bottom);
            if (fShowHidden)
                OffsetRect(ref rect2, x1, y1);
            InvertRect(hdc, ref rect2);

            //bottom edge
            SetRect(out rect2, rect.Right - border, border, rect.Right, rect.Bottom - border);
            if (fShowHidden)
                OffsetRect(ref rect2, x1, y1);
            InvertRect(hdc, ref rect2);


            Winapi.ReleaseDC(hwnd, hdc);
        }

        public static void InvertScreenRect(IntPtr hwnd, Rectangle rect)
        {
            Winapi.RECT rect2;

            var loc = Winapi.GetWindowRect(hwnd).Location;
            rect.Offset(-loc.X, -loc.Y);
            var hdc = Winapi.GetWindowDC(hwnd);
            //top edge
            rect2 = new Winapi.RECT(rect.Left, rect.Top, rect.Right, rect.Top + INVERT_BORDER);
            InvertRect(hdc, ref rect2);

            //left edge
            rect2 = new Winapi.RECT(rect.Left, rect.Top + INVERT_BORDER, rect.Left + INVERT_BORDER, rect.Bottom - INVERT_BORDER);
            InvertRect(hdc, ref rect2);

            //right edge
            rect2 = new Winapi.RECT(rect.Right - INVERT_BORDER, rect.Top + INVERT_BORDER, rect.Right, rect.Bottom - INVERT_BORDER);
            InvertRect(hdc, ref rect2);

            //bottom edge
            rect2 = new Winapi.RECT(rect.Left, rect.Bottom - INVERT_BORDER, rect.Right, rect.Bottom);
            InvertRect(hdc, ref rect2);

            Winapi.ReleaseDC(hwnd, hdc);
        }
    }
}
