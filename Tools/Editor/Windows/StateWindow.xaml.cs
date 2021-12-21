using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Kalavarda.Primitives.Visualization;
using Microsoft.Win32;
using Frame = Kalavarda.Primitives.Visualization.Frame;

namespace Editor.Windows
{
    public partial class StateWindow
    {
        private readonly VisualObject _visualObject;
        private readonly State _state;

        private View SelectedView => _lbViews.SelectedItem as View;

        public StateWindow()
        {
            InitializeComponent();

            _sliderVolume.Value = Settings.Default.Volume;
        }

        public StateWindow(VisualObject visualObject, State state): this()
        {
            _visualObject = visualObject ?? throw new ArgumentNullException(nameof(visualObject));
            _state = state ?? throw new ArgumentNullException(nameof(state));

            Title = state.Name;
            TuneControls();
        }

        private void CheckBoxLoop_OnChecked(object sender, RoutedEventArgs e)
        {
            _state.Looping = _cbLoop.IsChecked == true;
        }

        private void ButtonSoundLoad_OnClick(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                DefaultExt = ".mp3",
                Filter = "mp3 files|*.mp3|All files|*.*"
            };
            if (openDialog.ShowDialog() != true)
                return;

            using var file = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new BinaryReader(file);
            var data = reader.ReadBytes((int)file.Length);

            _state.Sound = new StateSound
            {
                RawData = data
            };
            TuneControls();
        }

        private void ButtonSoundClear_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Очистить?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _state.Sound = null;

            TuneControls();
        }

        private void TuneControls(bool refreshViews = true)
        {
            if (refreshViews)
            {
                var si = _lbViews.SelectedItem;
                _lbViews.ItemsSource = _state.Views.OrderBy(v => v.Angle);
                if (si != null)
                    _lbViews.SelectedItem = si;
            }

            _cbLoop.IsChecked = _state.Looping;
            _btnSoundClear.Visibility = _state.Sound != null ? Visibility.Visible : Visibility.Collapsed;
            _miDeleteView.IsEnabled = SelectedView != null;
            _miAngle.IsEnabled = SelectedView != null;
            _miDuration.IsEnabled = SelectedView != null;
            _miFrames.IsEnabled = SelectedView != null;

            if (SelectedView != null)
            {
                _vizualizer.VisualObject = null;
                _vizualizer.VisualObject = _visualObject;
                _vizualizer.VisualObject.CurrentState = _state;
                _vizualizer.VisualObject.CurrentAngle = SelectedView.Angle;
            }
            else
                _vizualizer.VisualObject = null;
        }

        private void MenuItem_Add_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var view = new View();
                while (_state.Views.Any(v => v.Angle == view.Angle))
                    view.Angle++;

                _state.Add(view);
                TuneControls();
                _lbViews.SelectedItem = view;
            }
            catch (Exception exception)
            {
                App.ShowError(exception);
            }
        }

        private void MenuItem_Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Удалить \"{SelectedView}\"?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _state.Remove(SelectedView);
            TuneControls();
        }

        private void MenuItem_Angle_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new TextWindow(SelectedView.Angle.ToString(), "Введите угол поворота (град.)", newValue => CheckAngle(SelectedView, newValue)) { Owner = this };
            if (window.ShowDialog() == true)
            {
                SelectedView.Angle = int.Parse(window.Text);
                TuneControls();
            }
        }

        private void MenuItem_Duration_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new TextWindow(SelectedView.DurationSec.ToString(), "Введите длительность анимации (сек.)", newValue => CheckDuration(SelectedView, newValue)) { Owner = this };
            if (window.ShowDialog() == true)
            {
                SelectedView.DurationSec = float.Parse(window.Text);
                TuneControls();
            }
        }

        private Exception CheckAngle(View view, string newValue)
        {
            var newAngle = int.Parse(newValue);

            if (_state.Views.Where(v => v != view).Any(v => v.Angle == newAngle))
                throw new ArgumentException("Вид с таким углом уже есть");

            if (newAngle < -720 || newAngle > 720)
                throw new ArgumentException("Слишком большое значение");

            return null;
        }

        private Exception CheckDuration(View view, string newValue)
        {
            var newDurat = float.Parse(newValue);

            if (newDurat <= 0)
                throw new ArgumentException("Введите значение больше нуля");

            if (newDurat >= 1000)
                throw new ArgumentException("Слишком большое значение");

            return null;
        }

        private void MenuItem_Frames_OnClick(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "png files|*.png|All files|*.*",
                Multiselect = true
            };
            if (openDialog.ShowDialog() != true)
                return;

            SelectedView.Frames = openDialog.FileNames
                .OrderBy(fn => fn)
                .Select(fn =>
                {
                    using var file = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var reader = new BinaryReader(file);
                    return new Frame{RawData = reader.ReadBytes((int)file.Length) };
                })
                .ToArray();

            TuneControls();
        }

        private void ListBoxViews_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TuneControls(false);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _vizualizer.VisualObject = null;

            base.OnClosing(e);
        }

        private void _sliderVolume_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _vizualizer.Volume = _sliderVolume.Value;
            Settings.Default.Volume = _sliderVolume.Value;
            Settings.Default.Save();
        }
    }
}
