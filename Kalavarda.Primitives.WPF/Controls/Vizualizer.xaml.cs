﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Kalavarda.Primitives.Visualization;

namespace Kalavarda.Primitives.WPF.Controls
{
    public partial class Vizualizer
    {
        private int _nextFrameNumber;
        private IVisualObject _visualObject;
        private DispatcherTimer _timer;
        private MediaPlayer _mediaPlayer;
        private double _volume;

        public IVisualObject VisualObject
        {
            get => _visualObject;
            set
            {
                if (_visualObject == value)
                    return;

                if (_visualObject != null)
                {
                    _visualObject.StateChanged -= VisualObjectChanged;
                    _visualObject.AngleChanged -= VisualObjectChanged;
                    
                    _timer.Tick -= Timer_Tick;
                    _timer.Stop();
                    _timer = null;
                }

                _visualObject = value;

                if (_visualObject != null)
                {
                    _visualObject.StateChanged += VisualObjectChanged;
                    _visualObject.AngleChanged += VisualObjectChanged;

                    _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                    _timer.Tick += Timer_Tick;
                    VisualObjectChanged(_visualObject);
                }
                else
                {
                    _image.Visibility = Visibility.Collapsed;
                    _image.Source = null;
                }
            }
        }

        /// <summary>
        /// 0..1
        /// </summary>
        public double Volume
        {
            get => _volume;
            set
            {
                _volume = value;

                if (_mediaPlayer != null)
                    _mediaPlayer.Volume = value;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var state = _visualObject?.CurrentState;
            if (state == null)
                return;

            var view = GetView();
            if (view == null)
                return;

            ShowFrame(view);

            _nextFrameNumber++;

            if (_nextFrameNumber >= view.Frames.Length)
            {
                if (state.Looping)
                    _nextFrameNumber = 0;
                else
                    _timer.Stop();
            }
        }

        private void VisualObjectChanged(IReadonlyVisualObject @object)
        {
            _nextFrameNumber = 0;

            var view = GetView();
            if (view?.Frames == null)
            {
                _image.Visibility = Visibility.Collapsed;
                return;
            }

            if (view.Frames.Length > 1)
            {
                _timer.Interval = TimeSpan.FromSeconds(view.DurationSec / view.Frames.Length);
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                ShowFrame(view);
            }

            PlaySound();
        }

        private void PlaySound()
        {
            var state = _visualObject?.CurrentState;

            if (state?.Sound == null)
                return;

            if (_mediaPlayer != null)
                _mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;

            _mediaPlayer = new MediaPlayer { Volume = Volume };
            _mediaPlayer.Open(SoundUriFactory.Instance.GetUri(state.Sound));
            if (state.Looping)
                _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            _mediaPlayer.Play();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            var state = _visualObject?.CurrentState;
            if (state != null && state.Looping)
                PlaySound();
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
            return _visualObject?.CurrentState?.GetView(_visualObject.CurrentAngle);
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

    internal class SoundUriFactory
    {
        private readonly Dictionary<StateSound, Uri> _cache = new Dictionary<StateSound, Uri>();

        public static SoundUriFactory Instance = new SoundUriFactory();
        
        private SoundUriFactory() { }

        public Uri GetUri(StateSound sound)
        {
            if (sound?.RawData == null)
                throw new ArgumentNullException(nameof(sound));

            if (_cache.TryGetValue(sound, out var uri))
                return uri;

            // TODO: clear temporary files before application closing

            var fileName = Path.GetTempFileName().Replace(".tmp", ".mp3");
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                file.Write(sound.RawData);

            uri = new Uri(fileName);
            _cache.Add(sound, uri);
            return uri;
        }
    }
}
