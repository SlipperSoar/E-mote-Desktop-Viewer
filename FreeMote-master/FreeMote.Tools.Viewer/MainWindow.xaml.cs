/*
 *  Project AZUSA © 2015-2018 ( https://github.com/Project-AZUSA )
 *  AUTHOR:	Ulysses (wdwxy12345@gmail.com)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace FreeMote.Tools.Viewer
{
    enum D3DResult : uint
    {
        DEVICE_LOST = 0x88760868,
        DEVICE_NOTRESET = 0x88760869,
        INVAILDCALL = 0x8876086C,
        OK = 0x0,
        UNKNOWN_ERROR = 0xFFFFFFFF
    }

    struct TimelineData
    {
        public string name;
        public TimelinePlayFlags flag;
    }

    public partial class MainWindow : Window
    {
        const string LastScaleKey = "LastEmoteScale";
        const string LastPositionKey = "LastPosition";
        const float RefreshRate = 1000.0f / 65.0f; // 1/n秒カウントをmsへ変換。
        private const int Movement = 10;

        private static double _lastX, _lastY;
        private static double _midX, _midY;
        private bool _mouseTrack = false;
        private D3DImage _di;
        private Emote _emote;
        private WindowInteropHelper _helper;
        private EmotePlayer _player;
        private IntPtr _scene;
        private string _psbPath;
        private PreciseTimer _timer;

        private double _deltaX, _deltaY;
        private double _elapsedTime;
        private bool _measureMode = false;

        private List<TimelineData> timelineDatas;

        public MainWindow()
        {
            _psbPath = Core.PsbPath;
            timelineDatas = new List<TimelineData>();

            _helper = new WindowInteropHelper(this);

            // create a D3DImage to host the scene and
            // monitor it for changes in front buffer availability
            _di = new D3DImage();
            _di.IsFrontBufferAvailableChanged
                += OnIsFrontBufferAvailableChanged;

            MouseRightButtonUp += MainWindow_MouseRightButtonUp;
            MouseMove += MainWindow_MouseMove;
            MouseWheel += MainWindow_MouseWheel;
            MouseDoubleClick += MainWindow_MouseDoubleClick;

            KeyDown += OnKeyDown;

            // make a brush of the scene available as a resource on the window
            Resources["NekoHacksScene"] = new ImageBrush(_di);

            //double x = SystemParameters.WorkArea.Width; //得到屏幕工作区域宽度
            //double y = SystemParameters.WorkArea.Height; //得到屏幕工作区域高度
            //double x1 = SystemParameters.PrimaryScreenWidth; //得到屏幕整体宽度
            //double y1 = SystemParameters.PrimaryScreenHeight; //得到屏幕整体高度

            //WindowStartupLocation = WindowStartupLocation.Manual;
            //Left = x1 - 800;
            //Top = y1 - 600;
            // parse the XAML
            InitializeComponent();
            Init();
            Width = Core.Width;
            Height = Core.Height;

            _midX = Width / 2;
            _midY = Height / 2;
            CenterMark.Visibility = Visibility.Hidden;
            CharaCenterMark.Visibility = Visibility.Hidden;

            _emote = new Emote(_helper.EnsureHandle(), (int)Width, (int)Height, true);
            _emote.EmoteInit();

            _player = _emote.CreatePlayer("Chara1", _psbPath);

            var offsetScaleStr = UserRegistryKey.GetString(LastScaleKey);
            _player.SetScale(1, 0, 0);
            if (!string.IsNullOrEmpty(offsetScaleStr))
            {
                var offsetScale = float.Parse(offsetScaleStr);
                _player.SetScale(offsetScale);
            }
            _player.SetCoord(0, 0);
            _player.SetVariable("fade_z", 256);
            _player.SetSmoothing(true);
            _player.Show();

            InitTimelines();

            if (Core.NeedRemoveTempFile)
            {
                File.Delete(_psbPath);
                Core.NeedRemoveTempFile = false;
            }

            // begin rendering the custom D3D scene into the D3DImage
            BeginRenderingScene();
        }

        #region Public Method

        public void Init()
        {
            // Init Position
            WindowStartupLocation = WindowStartupLocation.Manual;
            var posStr = UserRegistryKey.GetString(LastPositionKey);
            if (string.IsNullOrEmpty(posStr))
            {
                return;
            }

            var posStrs = posStr.Split(',');
            var left = int.Parse(posStrs[0]);
            var top = int.Parse(posStrs[1]);

            var mainWidth = (int) SystemParameters.WorkArea.Width;
            var mainHeight = (int) SystemParameters.WorkArea.Height;
            var width = (int) SystemParameters.VirtualScreenWidth; //得到全屏幕工作区域宽度
            var height = (int) SystemParameters.VirtualScreenHeight; //得到全屏幕工作区域高度
            Left = left >= width ? mainWidth / 2 : left;
            Top = top >= height ? mainHeight / 2 : top;
        }

        #endregion

        private void MainWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlayTimeline(new Random().Next(0, timelineDatas.Count));
        }

        private void LoadModel()
        {
            //TODO:
        }

        private void MainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            var p = e.GetPosition(this);
            Debug.WriteLine(WindowWorldToCharacterWorld(p.X, p.Y));
#endif
        }

        private async void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Up)
            {
                _player.OffsetCoord(0, Movement);
            }

            if (keyEventArgs.Key == Key.Down)
            {
                _player.OffsetCoord(0, -Movement);
            }

            if (keyEventArgs.Key == Key.Left)
            {
                _player.OffsetCoord(Movement, 0);
            }

            if (keyEventArgs.Key == Key.Right)
            {
                _player.OffsetCoord(-Movement, 0);
            }

            await Task.Delay(20);
            UpdatePosition();
        }

        void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_measureMode)
            {
                _player.SetScale(1f);
            }
            else
            {
                _player.OffsetScale(1 + ConvertDelta(e.Delta));
                UserRegistryKey.SetString(LastScaleKey, _player.GetScale().ToString());
            }
        }

        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // var ex = e.GetPosition(this);
                // _player.OffsetCoord((int) (ex.X - _lastX), (int) (ex.Y - _lastY));
                // _lastX = ex.X;
                // _lastY = ex.Y;
                DragMove();
                UserRegistryKey.SetString(LastPositionKey, $"{(int) Left},{(int) Top}");
            }
            else
            {
                var ex2 = e.GetPosition(this);
                _lastX = ex2.X;
                _lastY = ex2.Y;
                if (!_measureMode)
                {
                    if (_mouseTrack)
                    {
                        var ex = e.GetPosition(this);
                        _deltaX = (ex.X - _midX) / _midX * 64;
                        _deltaY = (ex.Y - _midY) / _midY * 64;

                        float frameCount = 0f;
                        //float frameCount = 50f;
                        float easing = 0f;
                        _player.SetVariable("head_UD", (float) _deltaY, frameCount, easing);
                        _player.SetVariable("head_LR", (float) _deltaX, frameCount, easing);
                        _player.SetVariable("body_UD", (float) _deltaY, frameCount, easing);
                        _player.SetVariable("body_LR", (float) _deltaX, frameCount, easing);
                        _player.SetVariable("face_eye_UD", (float) _deltaY, frameCount, easing);
                        _player.SetVariable("face_eye_LR", (float) _deltaX, frameCount, easing);
                    }
                }
            }

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            // var p = Mouse.GetPosition(this);
            // var (mx, my) = WindowWorldToCharacterWorld(p.X, p.Y);
            _player.GetCoord(out float cx, out float cy);
            var (wx, wy) = CharacterWorldToWindowWorld(cx, cy);
            UpdateCharaMark(wx, wy);
            // Title = $"Project AZUSA © FreeMote Viewer Powered by FreeMote - Center: {-cx:F2},{-cy:F2} Mouse: {mx:F2},{my:F2}";
        }

        private void UpdateCharaMark(double wx, double wy)
        {
            var width = CharaCenterMark.ActualWidth;
            var height = CharaCenterMark.ActualHeight;
            var margin = CharaCenterMark.Margin;
            margin.Left = wx - width / 2.0;
            margin.Top = wy - height / 2.0;
            CharaCenterMark.Margin = margin;
        }

        private (float x, float y) ConvertToEmoteWorld(double x, double y)
        {
            var centerX = Width / 2.0;
            var centerY = Height / 2.0;
            float ex = (float) (x - centerX);
            float ey = (float) (y - centerY);
            var scale = _player.GetScale();
            return (ex / scale, ey / scale);
        }

        private (float x, float y) WindowWorldToCharacterWorld(double x, double y)
        {
            var centerX = Width / 2.0;
            var centerY = Height / 2.0;
            float ex = (float) (x - centerX);
            float ey = (float) (y - centerY);
            var scale = _player.GetScale();
            _player.GetCoord(out float cx, out float cy);
            return (-(cx - ex / scale), -(cy - ey / scale));
        }

        private (double x, double y) CharacterWorldToWindowWorld(float x, float y)
        {
            _player.GetCoord(out float cx, out float cy);
            float ex = x - cx;
            float ey = y - cy;
            var centerX = Width / 2.0;
            var centerY = Height / 2.0;
            var scale = _player.GetScale();
            return (cx * scale + centerX + ex * scale, cy * scale + centerY + ey * scale);
        }

        private static float ConvertDelta(int delta)
        {
            return delta / 120.0f / 50.0f;
        }

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // if the front buffer is available, then WPF has just created a new
            // D3D device, so we need to start rendering our custom scene
            if (_di.IsFrontBufferAvailable)
            {
                BeginRenderingScene();
            }
            else
            {
                // If the front buffer is no longer available, then WPF has lost its
                // D3D device so there is no reason to waste cycles rendering our
                // custom scene until a new device is created.
                StopRenderingScene();
            }
        }

        private void BeginRenderingScene()
        {
            if (_di.IsFrontBufferAvailable)
            {
                // create a custom D3D scene and get a pointer to its surface
                _scene = new IntPtr(_emote.D3DSurface);

                // set the back buffer using the new scene pointer
                _di.Lock();
                _di.SetBackBuffer(D3DResourceType.IDirect3DSurface9, _scene);
                _di.Unlock();

                _timer = new PreciseTimer();
                // leverage the Rendering event of WPF's composition target to
                // update the custom D3D scene
                CompositionTarget.Rendering += OnRendering;
            }
        }

        private void StopRenderingScene()
        {
            // This method is called when WPF loses its D3D device.
            // In such a circumstance, it is very likely that we have lost 
            // our custom D3D device also, so we should just release the scene.
            // We will create a new scene when a D3D device becomes 
            // available again.
            CompositionTarget.Rendering -= OnRendering;
            _scene = IntPtr.Zero;

            _emote.OnDeviceLost();
            while (_emote.D3DTestCooperativeLevel() == (uint) D3DResult.DEVICE_LOST)
            {
                Thread.Sleep(5);
            }

            if (_emote.D3DTestCooperativeLevel() == (uint) D3DResult.DEVICE_NOTRESET)
            {
                _emote.D3DReset();
                _emote.OnDeviceReset();
                //_emote.D3DInitRenderState();
                //var hr = _emote.D3DTestCooperativeLevel();
            }
            else
            {
                Debug.WriteLine("{0:x8}", _emote.D3DTestCooperativeLevel());
            }
        }

        private void OnRendering(object sender, EventArgs e)
        {
            _elapsedTime += _timer.GetElaspedTime() * 1000;

            if (_elapsedTime < RefreshRate)
            {
                return;
            }

            // when WPF's composition target is about to render, we update our 
            // custom render target so that it can be blended with the WPF target
            UpdateScene(_elapsedTime);

            _elapsedTime = 0;
        }

        private void UpdateScene(double elasped)
        {
            if (_di.IsFrontBufferAvailable && _scene != IntPtr.Zero)
            {
                _emote.Update((float) elasped);
                // lock the D3DImage
                _di.Lock();
                // update the scene (via a call into our custom library)
                _emote.D3DBeginScene();
                _emote.Draw();
                _emote.D3DEndScene();
                // invalidate the updated region of the D3DImage (in this case, the whole image)
                _di.AddDirtyRect(new Int32Rect(0, 0, _emote.SurfaceWidth, _emote.SurfaceHeight));
                // unlock the D3DImage
                _di.Unlock();
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
        }

        private void InitTimelines()
        {
            var count = _player.CountMainTimelines();
            for (uint i = 0; i < count; i++)
            {
                timelineDatas.Add(new TimelineData()
                {
                    name = _player.GetMainTimelineLabelAt(i),
                    flag = TimelinePlayFlags.NONE
                });
            }

            count = _player.CountDiffTimelines();
            for (uint i = 0; i < count; i++)
            {
                timelineDatas.Add(new TimelineData()
                {
                    name = _player.GetDiffTimelineLabelAt(i),
                    flag = TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE
                });
            }
        }

        private void PlayTimeline(int index)
        {
            if (timelineDatas.Count <= index)
            {
                return;
            }
            var timeline = timelineDatas[index];
            _player.PlayTimeline(timeline.name, timeline.flag);
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            _player.StopTimeline("");
            _player.Skip();
            _player.SetVariable("fade_z", 256);
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            for (uint i = 0; i < _player.CountVariables(); i++)
            {
                _player.SetVariable(_player.GetVariableLabelAt(i), 0);
            }

            _player.SetVariable("fade_z", 256);
        }
    }
}