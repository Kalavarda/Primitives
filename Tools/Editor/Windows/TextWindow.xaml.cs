using System;
using System.Windows;

namespace Editor.Windows
{
    public partial class TextWindow
    {
        private readonly Func<string, Exception> _checker;

        public string Text => string.IsNullOrEmpty(_tb.Text) ? string.Empty : _tb.Text.Trim();

        public TextWindow()
        {
            InitializeComponent();

            Loaded += (sender, e) => _tb.Focus();
        }

        public TextWindow(string text, string title, Func<string, Exception> checker): this()
        {
            _checker = checker;
            Title = title;
            _tb.Text = text;
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_checker != null)
                {
                    var error = _checker(Text);
                    if (error != null)
                        App.ShowError(error);
                    else
                        DialogResult = true;
                }
            }
            catch (Exception exception)
            {
                App.ShowError(exception);
            }
        }
    }
}
