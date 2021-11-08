using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Kalavarda.Primitives.Visualization;

namespace Kalavarda.Primitives.WPF.Controls
{
    public partial class Vizualizer
    {
        private int _nextFrameNumber = 0;
        private VisualData _visualData;
        private DispatcherTimer _timer;

        public VisualData VisualData
        {
            get => _visualData;
            set
            {
                if (_visualData == value)
                    return;

                if (_visualData != null)
                {
                    _visualData.StateChanged -= VisualData_Changed;
                    _visualData.AngleChanged -= VisualData_Changed;
                    
                    _timer.Tick -= Timer_Tick;
                    _timer.Stop();
                    _timer = null;
                }

                _visualData = value;

                if (_visualData != null)
                {
                    _visualData.StateChanged += VisualData_Changed;
                    _visualData.AngleChanged += VisualData_Changed;

                    _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                    _timer.Tick += Timer_Tick;
                    _timer.Start();
                }
                else
                {
                    _image.Visibility = Visibility.Collapsed;
                    _image.Source = null;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var view = GetView();
            if (view == null)
                return;

            ShowFrame(view);

            _nextFrameNumber++;

            if (_nextFrameNumber >= view.Frames.Length)
                _nextFrameNumber = 0;
        }

        private void VisualData_Changed(VisualData data)
        {
            _nextFrameNumber = 0;

            var view = GetView();
            if (view == null)
            {
                _image.Visibility = Visibility.Collapsed;
                return;
            }

            _timer.Interval = TimeSpan.FromSeconds(view.DurationSec / view.Frames.Length);
        }

        private void ShowFrame(View view)
        {
            if (view == null)
            {
                _image.Visibility = Visibility.Collapsed;
                return;
            }

            var frame = view.Frames.Skip(_nextFrameNumber).FirstOrDefault();
            if (frame == null)
            {
                _image.Visibility = Visibility.Collapsed;
                return;
            }

            _image.Visibility = Visibility.Visible;
            _image.Source = ImageSourceCache.Instance.GetImage(frame);
        }

        private View GetView()
        {
            return _visualData?.State?.GetView(_visualData.Angle);
        }

        public Vizualizer()
        {
            InitializeComponent();
        }
    }

    internal class ImageSourceCache
    {
        private readonly Dictionary<Frame, BitmapImage> _cache = new Dictionary<Frame, BitmapImage>();

        public static ImageSourceCache Instance = new ImageSourceCache();

        private ImageSourceCache() { }

        public BitmapImage GetImage(Frame frame)
        {
            if (frame == null) throw new ArgumentNullException(nameof(frame));

            if (_cache.TryGetValue(frame, out var image))
                return image;

            image = LoadImage(frame.RawData);
            _cache.Add(frame, image);
            return image;
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var image = new BitmapImage();
            using (var stream = new MemoryStream(imageData))
            {
                //stream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
