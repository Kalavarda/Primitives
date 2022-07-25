using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Kalavarda.Primitives.WPF.Controllers
{
    public static class WindowPositionController
    {
        private static readonly IDictionary<Window, string> _keys = new Dictionary<Window, string>();
        private static readonly IDictionary<string, WindowData> _data = new Dictionary<string, WindowData>();
        private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "WinPositions");
        private static readonly string FileName = Path.Combine(FolderPath, AppDomain.CurrentDomain.FriendlyName + ".win");
        private static readonly TimeLimiter _timerLimiter = new(TimeSpan.FromSeconds(0.5));

        static WindowPositionController()
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            if (File.Exists(FileName))
            {
                using var file = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var dict = JsonSerializer.Deserialize<Dictionary<string, WindowData>>(file);
                foreach (var pair in dict)
                    _data.Add(pair.Key, pair.Value);
            }
        }

        public static void ControlBounds(this Window window, string windowKey = null)
        {
            windowKey ??= window.GetType().FullName;
            _keys.Add(window, windowKey);

            WindowData windowData;
            if (_data.ContainsKey(windowKey))
                windowData = _data[windowKey];
            else
            {
                windowData = new WindowData
                {
                    State = window.WindowState,
                    Size = new Size(window.Width, window.Height),
                    Location = new Point(window.Left, window.Top)
                };
                _data.Add(windowKey, windowData);
            }

            window.WindowState = windowData.State;
            window.Width = windowData.Size.Width;
            window.Height = windowData.Size.Height;
            window.Left = windowData.Location.X;
            window.Top = windowData.Location.Y;

            window.SizeChanged += Window_SizeChanged;
            window.StateChanged += Window_StateChanged;
            window.LocationChanged += Window_LocationChanged;
            window.Closed += Window_Closed;
        }

        private static void Window_StateChanged(object sender, EventArgs e)
        {
            var window = (Window)sender;
            if (window.IsInitialized)
            {
                var key = _keys[window];
                _data[key].State = window.WindowState;
                Save();
            }
        }

        private static void Window_LocationChanged(object sender, EventArgs e)
        {
            var window = (Window)sender;
            if (window.IsInitialized)
            {
                var key = _keys[window];
                _data[key].Location = new Point(window.Left, window.Top);
                Save();
            }
        }

        private static void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var window = (Window)sender;
            if (window.IsInitialized)
            {
                var key = _keys[window];
                _data[key].Size = new Size(e.NewSize.Width, e.NewSize.Height);
                Save();
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            Save();

            var window = (Window)sender;

            window.SizeChanged -= Window_SizeChanged;
            window.StateChanged -= Window_StateChanged;
            window.LocationChanged -= Window_LocationChanged;
            window.Closed -= Window_Closed;

            _keys.Remove(window);
        }

        public class WindowData
        {
            public WindowState State { get; set; }
            
            public Size Size { get; set; }
            
            public Point Location { get; set; }
        }

        private static void Save()
        {
            _timerLimiter.Do(() =>
            {
                var data = JsonSerializer.Serialize(_data);
                using var file = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using var writer = new StreamWriter(file);
                writer.Write(data);
            });
        }
    }
}
