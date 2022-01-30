using System;
using System.IO;
using System.Windows;
using Kalavarda.Primitives.Visualization;
using Microsoft.Win32;

namespace Editor.Windows
{
    public partial class SceneWindow
    {
        internal const string DefaultExt = ".scn";
        internal const string Filter = "Scenes|" + DefaultExt + "|All files|*.*";

        private readonly Scene _scene;
        private string _fileName;

        public SceneWindow()
        {
            InitializeComponent();
        }

        public SceneWindow(Scene scene, string fileName = null) : this()
        {
            _scene = scene ?? throw new ArgumentNullException(nameof(scene));
            _fileName = fileName;
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

            IBinarySerializer serializer = new BinarySerializer();
            using var file = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            serializer.Serialize(_scene, file);

            MessageBox.Show("Saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
