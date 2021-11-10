using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Kalavarda.Primitives.Visualization;
using Microsoft.Win32;

namespace Editor.Windows
{
    public partial class VisualObjectWindow
    {
        internal const string DefaultExt = ".viz";
        internal const string Filter = "Visual objects|*.viz|All files|*.*";

        private readonly VisualObject _visualObject;
        private string _fileName;

        private State SelectedState => _lbStates.SelectedItem as State;

        public VisualObjectWindow()
        {
            InitializeComponent();
        }

        public VisualObjectWindow(VisualObject visualObject, string fileName = null): this()
        {
            _visualObject = visualObject ?? throw new ArgumentNullException(nameof(visualObject));
            _fileName = fileName;

            _tbId.Text = _visualObject.Id;
            if (!string.IsNullOrEmpty(fileName))
                Title = Path.GetFileNameWithoutExtension(fileName);

            _lbStates.ItemsSource = _visualObject.States;

            TuneControls();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                var saveDialog = new SaveFileDialog
                {
                    DefaultExt = DefaultExt,
                    Filter = Filter
                };
                if (saveDialog.ShowDialog() != true)
                    return;

                _fileName = saveDialog.FileName;
            }

            var serializer = new BinarySerializer();
            using var file = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                serializer.Serialize(_visualObject, file);

            MessageBox.Show("Данные сохранены", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ListBox_States_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TuneControls();
        }

        private void TuneControls()
        {
            _miDelete.IsEnabled = SelectedState != null;
            _miRename.IsEnabled = SelectedState != null;
        }

        private void MenuItem_Add_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var i = 1;
                var newName = $"State {i}";
                while (_visualObject.States.Any(st => st.Name.Equals(newName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    i++;
                    newName = $"State {i}";
                }

                var state = new State { Name = newName };
                _visualObject.Add(state);

                var window = new StateWindow(_visualObject, state) { Owner = this };
                window.ShowDialog();
                _lbStates.ItemsSource = _visualObject.States;
                _lbStates.SelectedItem = state;
            }
            catch (Exception exception)
            {
                App.ShowError(exception);
            }
        }

        private void MenuItem_Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Удалить \"{SelectedState.Name}\"?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _visualObject.Remove(SelectedState);
            _lbStates.ItemsSource = _visualObject.States;
        }

        private void MenuItem_Rename_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new TextWindow(SelectedState.Name, "Введите название", newName => CheckName(SelectedState, newName)) { Owner = this };
            if (window.ShowDialog() == true)
            {
                SelectedState.Name = window.Text;
                _lbStates.ItemsSource = null;
                _lbStates.ItemsSource = _visualObject.States;
                _lbStates.SelectedItem = SelectedState;
            }
        }

        private Exception CheckName(State state, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return new Exception("Название не может быть пустым");

            if (_visualObject.States
                .Where(st => st != state)
                .Any(st => st.Name.Equals(newName, StringComparison.InvariantCultureIgnoreCase)))
                return new Exception("Состояние с таким названием уже есть");

            return null;
        }

        private void ListBoxStates_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedState == null)
                return;

            var state = SelectedState;

            var window = new StateWindow(_visualObject, state) { Owner = this };
            window.ShowDialog();
            _lbStates.ItemsSource = null;
            _lbStates.ItemsSource = _visualObject.States;
            _lbStates.SelectedItem = state;
        }
    }
}
