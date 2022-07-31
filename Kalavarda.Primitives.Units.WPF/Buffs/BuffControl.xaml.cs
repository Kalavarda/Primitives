using System.Windows.Media.Imaging;
using Kalavarda.Primitives.Units.Buffs;

namespace Kalavarda.Primitives.Units.WPF.Buffs
{
    public partial class BuffControl
    {
        public BuffControl()
        {
            InitializeComponent();

            DataContextChanged += BuffControl_DataContextChanged;
        }

        private void BuffControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            _image.Source = DataContext is Buff buff
                ? new BitmapImage(buff.ImageUri) // TODO: кэш
                : null;
        }
    }
}
