using System.Windows.Media.Imaging;
using Kalavarda.Primitives.Units.Buffs;
using Kalavarda.Primitives.WPF;

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
                ? BitmapImageCache.Instance.Get(buff.ImageUri) // TODO: кэш
                : null;
        }
    }
}
