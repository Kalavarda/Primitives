using System;
using System.Windows;

namespace Editor
{
    public partial class App 
    {
        public static void ShowError(Exception error)
        {
            MessageBox.Show(error.GetBaseException().Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
