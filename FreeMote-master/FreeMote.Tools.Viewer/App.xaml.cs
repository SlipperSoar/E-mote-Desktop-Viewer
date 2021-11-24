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
            #region console method
            /*
            var app = new CommandLineApplication();
            app.OptionsComparison = StringComparison.OrdinalIgnoreCase;

            //help
            app.HelpOption("-?|--help"); //do not inherit
            app.ExtendedHelpText = PrintHelp();

            //options
            var optWidth = app.Option<uint>("-w|--width", "Set Window width", CommandOptionType.SingleValue);
            var optHeight = app.Option<uint>("-h|--height", "Set Window height", CommandOptionType.SingleValue);
            var optDirectLoad = app.Option("-d|--direct", "Just load with EMT driver, don't try parsing with FreeMote first", CommandOptionType.NoValue);
            var optFixMetadata = app.Option("-nf|--no-fix", "Don't try to apply metadata fix (for partial exported PSBs). Can't work together with `-d`", CommandOptionType.NoValue);

            //args
            var argPath = app.Argument("Files", "File paths", multipleValues: true);

            app.OnExecute(() =>
            {
                if (argPath.Values.Count == 0)
                {
                    app.ShowHelp();
                    return;
                }

                Core.PsbPaths = argPath.Values.ToList();
                Core.PsbPaths.RemoveAll(f => !File.Exists(f));

                if (Core.PsbPaths.Count == 0)
                {
                    Console.WriteLine("No file specified.");
                    return;
                }
                
                if (optWidth.HasValue())
                {
                    Core.Width = optWidth.ParsedValue;
                }

                if (optHeight.HasValue())
                {
                    Core.Height = optHeight.ParsedValue;
                }

                if (!optDirectLoad.HasValue())
                {
                    try
                    {
                        //Consts.FastMode = false;
                        FreeMount.Init();
                        var ctx = FreeMount.CreateContext();
                        for (int i = 0; i < Core.PsbPaths.Count; i++)
                        {
                            var oriPath = Core.PsbPaths[i];
                            using var fs = File.OpenRead(oriPath);
                            string currentType = null;
                            using var ms = ctx.OpenFromShell(fs, ref currentType);
                            var psb = ms != null ? new PSB(ms) : new PSB(fs);

                            if (psb.Platform == PsbSpec.krkr)
                            {
                                psb.SwitchSpec(PsbSpec.win, PsbSpec.win.DefaultPixelFormat());
                            }

                            if (!optFixMetadata.HasValue())
                            {
                                psb.FixMotionMetadata();
                            }

                            psb.Merge();
                            //File.WriteAllText("output.json", PsbDecompiler.Decompile(psb));
                            var tempFile = Path.GetTempFileName();
                            File.WriteAllBytes(tempFile, psb.Build());
                            Core.PsbPaths[i] = tempFile;
                            Core.NeedRemoveTempFile = true;
                        }
                        
                        GC.Collect(); //Can save memory from 700MB to 400MB
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
                else
                {
                    Core.DirectLoad = true;
                }

                FreeConsole();
                App wpf = new App();
                MainWindow main = new MainWindow();
                wpf.Run(main);
            });

            try
            {
                app.ExecuteAsync(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            */
            #endregion

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
