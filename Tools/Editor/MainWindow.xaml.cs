using System;
using System.IO;
using System.Windows;
using Editor.Windows;
using Kalavarda.Primitives.Visualization;
using Microsoft.Win32;

namespace Editor
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void VisualObject_New_OnClick(object sender, RoutedEventArgs e)
        {
            var visualObject = new VisualObject
            {
                Id = Guid.NewGuid().ToString()
            };
            var window = new VisualObjectWindow(visualObject) { Owner = this };
            window.Show();
        }

        private void VisualObject_Open_OnClick(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                DefaultExt = VisualObjectWindow.DefaultExt,
                Filter = VisualObjectWindow.Filter
            };
            if (openDialog.ShowDialog() != true)
                return;

            var serializer = new BinarySerializer();
            using var file = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var visualObject = serializer.Deserialize(file);
            var window = new VisualObjectWindow(visualObject, openDialog.FileName) { Owner = this };
            window.Show();
        }
    }
}
