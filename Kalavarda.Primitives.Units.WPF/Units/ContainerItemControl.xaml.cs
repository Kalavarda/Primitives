using System.Windows.Media.Imaging;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Units.Items;

namespace Kalavarda.Primitives.Units.WPF.Units
{
    public partial class ContainerItemControl
    {
        private Item _item;

        public Item Item
        {
            get => _item;
            private set
            {
                if (_item == value)
                    return;

                _item = value;

                if (_item == value)
                {
                    if (_item is IHasImage hasImage)
                        _image.Source = new BitmapImage(hasImage.ImageUri);
                }
            }
        }

        public ContainerItemControl()
        {
            InitializeComponent();
            DataContextChanged += BagItemControl_DataContextChanged;
        }

        private void BagItemControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Item = (Item)DataContext;
        }
    }
}
