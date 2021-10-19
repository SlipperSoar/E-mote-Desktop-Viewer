using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class WinAPI
{
    #region const

    public const int GWL_WNDPROC = -4;
    public const int GWL_HINSTANCE = -6;
    public const int GWL_HWNDPARENT = -8;
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;
    public const int GWL_USERDATA = -21;
    public const int GWL_ID = -12;

    public const uint SWP_SHOWWINDOW = 0x0040;

    public const int WS_BORDER = 1;
    public const int WS_THICKFRAME = 0x00040000;
    public const int WS_VISIBLE = 0x10000000;
    public const int WS_POPUP = 0x800000;
    public const int WS_EX_LAYERED = 0x00080000;
    public const int WS_CAPTION = 0x00C00000;
    public const int WS_EX_TRANSPARENT = 0x20;

    public const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}
    public const int SW_SHOWMAXIMIZED = 3; //{最大化, 激活} 

    private const int LWA_COLORKEY = 0x00000001;
    private const int LWA_ALPHA = 0x00000002;

    public const int ULW_COLORKEY = 0x00000001;
    public const int ULW_ALPHA = 0x00000002;
    public const int ULW_OPAQUE = 0x00000004;
    public const int ULW_EX_NORESIZE = 0x00000008;

    #endregion

    #region inner struct/class

    public struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    #endregion

    #region properties

    public static IntPtr ThisWindowHandle;

    #endregion

    #region Public Dll Method

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    #endregion

    #region Private Dll Method

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    #endregion

    #region Public Method

    public static void InitWindowHandle()
    {
        ThisWindowHandle = FindWindowA(null, "E-mote Desktop Viewer");
    }

    public static void HideWindowTitleBar()
    {
        var margins = new MARGINS() { cxLeftWidth = -1 };

        var lastStyle = GetWindowLong(ThisWindowHandle, GWL_STYLE);
        // SetWindowLong(ThisWindowHandle, GWL_STYLE, lastStyle & ~WS_BORDER & ~WS_THICKFRAME);
        // Set properties of the window
        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591%28v=vs.85%29.aspx
        SetWindowLong(ThisWindowHandle, GWL_STYLE, lastStyle & WS_POPUP | WS_VISIBLE);

        // Extend the window into the client area
        //See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa969512%28v=vs.85%29.aspx 
        DwmExtendFrameIntoClientArea(ThisWindowHandle, ref margins);
    }

    /// <summary>
    /// 窗口最大化
    /// </summary>
    public static void MaximizeWindow()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
    }

    /// <summary>
    /// 窗口最小化
    /// </summary>
    public static void MinimizeWindow()
    {
        ShowWindow(GetForegroundWindow(), WinAPI.SW_SHOWMINIMIZED);
    }

    public static void RemoveFrame()
    {
        SetWindowLong(ThisWindowHandle, GWL_STYLE, WS_POPUP);

        SetWindowPos(ThisWindowHandle, -1, 0, 0, 1024, 768, SWP_SHOWWINDOW);
    }

    public static void RemoveFrameWithAlpha()
    {
        // SetWindowLong(ThisWindowHandle, GWL_EXSTYLE, WS_EX_LAYERED);
        SetWindowLong(ThisWindowHandle, GWL_STYLE, GetWindowLong(ThisWindowHandle, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION);
        SetWindowPos(ThisWindowHandle, -1, 0, 0, 1024, 768, SWP_SHOWWINDOW);

        var margins = new MARGINS() { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(ThisWindowHandle, ref margins);
    }

    /// <summary>
    /// 穿透窗口
    /// </summary>
    public static void PenetrateWindow()
    {
        var lastStyle = GetWindowLong(ThisWindowHandle, GWL_EXSTYLE);
        SetWindowLong(ThisWindowHandle, GWL_EXSTYLE, lastStyle | WS_EX_TRANSPARENT | WS_EX_LAYERED);
    }

    #endregion
}
