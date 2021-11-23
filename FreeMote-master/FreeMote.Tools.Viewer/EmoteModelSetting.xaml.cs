using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using FreeMote.Plugins;
using FreeMote.Psb;
using FreeMote.PsBuild;

namespace FreeMote.Tools.Viewer
{
    /// <summary>
    /// EmoteModelSetting.xaml 的交互逻辑
    /// </summary>
    public partial class EmoteModelSetting : Window
    {
        private string folderPath;
        private Func<MainWindow> runMainWindow;
        private static FreeMountContext ctx;

        public EmoteModelSetting()
        {
            InitializeComponent();
            FreeMount.Init();
            ctx = FreeMount.CreateContext();
        }

        #region Public Method

        public void AddMainWindowRunAction(Func<MainWindow> action)
        {
            runMainWindow += action;
        }

        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void FileFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            FolderPathText.Text = folderPath = dialog.SelectedPath;
            LoadPsbPaths();
        }

        private void LoadPsbPaths()
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            DirectoryInfo directory = new DirectoryInfo(folderPath);
            foreach (var path in directory.GetFiles())
            {
                if (!path.Extension.ToLower().Equals(".psb"))
                {
                    continue;
                }
                var fullPath = path.FullName;
                var panel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Height = 25,
                };
                
                var button = new Button()
                {
                    Content = path.Name,
                };
                panel.Children.Add(button);
                button.Click += (sender, e) => {
                    var window = LoadBtn_Click(sender, e, fullPath);
                    if (window != null)
                    {
                        var btn = new Button()
                        {
                            Content = "Close This Window"
                        };
                        panel.Children.Add(btn);
                        btn.Click += (sender, e) =>
                        {
                            window.Close();
                            panel.Children.Remove(btn);
                        };
                    }
                };
                PsbFilePanel.Children.Add(panel);
            }
        }

        private MainWindow LoadBtn_Click(object sender, RoutedEventArgs e, string fullPath)
        {
            try
            {
                LoadEmotePSB(fullPath);
                Visibility = Visibility.Hidden;
                return runMainWindow?.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.Message}\n{exception.StackTrace}\n line: 97");
                return null;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Normal:
                    break;
                case WindowState.Minimized:
                    this.Visibility = Visibility.Hidden;
                    break;
                case WindowState.Maximized:
                    break;
                default:
                    break;
            }
        }

        public static void LoadEmotePSB(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                using var fs = File.OpenRead(path);
                string currentType = null;
                using var ms = ctx.OpenFromShell(fs, ref currentType);
                var psb = ms != null ? new PSB(ms) : new PSB(fs);

                if (psb.Platform == PsbSpec.krkr)
                {
                    psb.SwitchSpec(PsbSpec.win, PsbSpec.win.DefaultPixelFormat());
                }

                psb.FixMotionMetadata();

                psb.Merge();
                var tempFile = Path.GetTempFileName();
                File.WriteAllBytes(tempFile, psb.Build());
                Core.PsbPath = tempFile;
                Core.NeedRemoveTempFile = true;

                GC.Collect(); //Can save memory from 700MB to 400MB
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}\n line: 154");
            }
        }
    }
}
