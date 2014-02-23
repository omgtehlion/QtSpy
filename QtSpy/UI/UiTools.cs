using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Winapi;

namespace QtSpy.UI
{
    class UiTools
    {
        const int INVERT_BORDER = 3;

        [DllImport("user32.dll")]
        static extern bool InvertRect(IntPtr hDC, [In] ref RECT lprc);

        [DllImport("user32.dll")]
        static extern bool OffsetRect(ref RECT lprc, int dx, int dy);

        [DllImport("user32.dll")]
        static extern bool SetRect(out RECT lprc, int xLeft, int yTop, int xRight, int yBottom);

        public static void InvertWindow(WindowBase hwnd, bool fShowHidden)
        {
            RECT rect2;

            var border = INVERT_BORDER;

            //window rectangle (screen coords)
            RECT rect = hwnd.Rect;

            //client rectangle (screen coords)
            RECT rectc = hwnd.ClientRect;

            POINT tmp = rectc.Location;
            Methods.ClientToScreen(hwnd.Handle, ref tmp);
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
                hwnd = new Window(IntPtr.Zero);

            var hdc = Methods.GetWindowDC(hwnd.Handle);

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


            Methods.ReleaseDC(hwnd.Handle, hdc);
        }

        public static void InvertScreenRect(WindowBase hwnd, Rectangle rect)
        {
            RECT rect2;

            var loc = hwnd.Rect.Location;
            rect.Offset(-loc.X, -loc.Y);
            var hdc = Methods.GetWindowDC(hwnd.Handle);
            //top edge
            rect2 = new RECT(rect.Left, rect.Top, rect.Right, rect.Top + INVERT_BORDER);
            InvertRect(hdc, ref rect2);

            //left edge
            rect2 = new RECT(rect.Left, rect.Top + INVERT_BORDER, rect.Left + INVERT_BORDER, rect.Bottom - INVERT_BORDER);
            InvertRect(hdc, ref rect2);

            //right edge
            rect2 = new RECT(rect.Right - INVERT_BORDER, rect.Top + INVERT_BORDER, rect.Right, rect.Bottom - INVERT_BORDER);
            InvertRect(hdc, ref rect2);

            //bottom edge
            rect2 = new RECT(rect.Left, rect.Bottom - INVERT_BORDER, rect.Right, rect.Bottom);
            InvertRect(hdc, ref rect2);

            Methods.ReleaseDC(hwnd.Handle, hdc);
        }
    }
}
