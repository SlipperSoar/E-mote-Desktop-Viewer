//#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace FreeMote.Tools.Viewer
{
    public static class Core
    {
        public static uint Width { get; set; } = 480;
        public static uint Height { get; set; } = 720;
        public static string PsbPath { get; set; }
        internal static bool NeedRemoveTempFile { get; set; } = false;
    }

    class Program
    {
        // [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        // private static extern bool FreeConsole();
        private static EmoteModelSetting settingWindow;
        private static NotifyIcon notifyIcon;
        private static App app;

        [STAThread]
        static void Main(string[] args)
        {
            InitIcon();
            app = new App();
            settingWindow = new EmoteModelSetting();
            settingWindow.AddMainWindowRunAction(() =>
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                return mainWindow;
            });
            app.Run(settingWindow);
        }

        private static void InitIcon()
        {
            var components = new Container();
            notifyIcon = new(components);
            notifyIcon.Icon = new Icon("fure-zu.ico");
            notifyIcon.Text = "E-mote 桌面精灵/宠物/老婆/老公\nPowered By FreeMote";
            notifyIcon.Visible = true;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("Powered By FreeMote", OnPoweredTagClick),
                new MenuItem("设置", OnSettingMenu),
                new MenuItem("退出", OnExit)
            });

            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        private static void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            OnSettingMenu(sender, e);
        }

        private static void OnPoweredTagClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/UlyssesWu/FreeMote");
        }

        private static void OnSettingMenu(object sender, EventArgs e)
        {
            settingWindow.Visibility = Visibility.Visible;
        }

        private static void OnExit(object sender, EventArgs e)
        {
            UserRegistryKey.OnApplicationExit();
            app.Shutdown();
        }
    }

    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {

        }
    }
}
