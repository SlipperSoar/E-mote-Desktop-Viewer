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

namespace FreeMote.Tools.Viewer
{
    /// <summary>
    /// EmoteModelSetting.xaml 的交互逻辑
    /// </summary>
    public partial class EmoteModelSetting : Window
    {
        private System.Windows.Forms.FolderBrowserDialog dialog;
        private string folderPath;
        private Action runMainWindow;

        public EmoteModelSetting()
        {
            InitializeComponent();
        }

        #region Public Method

        public void AddMainWindowRunAction(Action action)
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
            dialog = new System.Windows.Forms.FolderBrowserDialog();
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
                Core.PsbPaths.Add(fullPath);
                var button = new Button();
                button.Content = path.Name;
                button.Click += (sender, e) => {
                    LoadBtn_Click(sender, e, fullPath);
                };
                PsbFilePanel.Children.Add(button);
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e, string fullPath)
        {
            try
            {
                var psbFile = new PsbFile(fullPath);
                var headerValid = psbFile.TestHeaderEncrypted();
                var bodyValid = psbFile.TestBodyEncrypted();
                if (headerValid && bodyValid)
                {
                    Core.isSingle = true;
                    Core.NeedRemoveTempFile = false;
                    Core.PsbPath = fullPath;
                    ShowModel();
                }
                else
                {
                    App.LoadEmotePSB(fullPath);
                    var text = new TextBlock();
                    text.Text = $"path: {Core.PsbPath}";
                    PsbFilePanel.Children.Add(text);
                    ShowModel();
                }
            }
            catch (Exception exception)
            {
                var text = new TextBlock();
                text.Text = $"{exception.Message}\n{exception.StackTrace}";
                PsbFilePanel.Children.Add(text);
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

        private void ShowModel()
        {
            this.Visibility = Visibility.Hidden;
            runMainWindow?.Invoke();
        }
    }
}
