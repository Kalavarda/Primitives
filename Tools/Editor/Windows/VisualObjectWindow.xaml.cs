using System;
using System.IO;
using System.Windows;
using Kalavarda.Primitives.Visualization;
using Microsoft.Win32;

namespace Editor.Windows
{
    public partial class VisualObjectWindow
    {
        internal const string DefaultExt = ".viz";
        internal const string Filter = "All files|*.*|Visual objects|*.viz";

        private readonly VisualObject _visualObject;
        private string _fileName;

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
        }
    }
}
