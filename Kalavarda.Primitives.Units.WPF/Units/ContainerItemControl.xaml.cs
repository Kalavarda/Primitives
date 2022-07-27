using System.Windows;
using System.Windows.Media.Imaging;
using Kalavarda.Primitives.Units.Items;
using Kalavarda.Primitives.WPF;

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

                if (_item != null)
                    _item.CountChanged -= Item_CountChanged;

                _item = value;

                if (_item != null)
                {
                    _image.Source = new BitmapImage(_item.ImageUri);
                    _item.CountChanged += Item_CountChanged;
                    Item_CountChanged(_item, 0, _item.Count);
                }
            }
        }

        private void Item_CountChanged(Abstract.IHasCount item, uint oldCount, uint newCount)
        {
            this.Do(() =>
            {
                if (newCount > 1)
                {
                    _tbCount.Text = newCount.ToString();
                    _borderCount.Visibility = Visibility.Visible;
                }
                else
                {
                    _borderCount.Visibility = Visibility.Collapsed;
                    _tbCount.Text = string.Empty;
                }
            });
        }

        public ContainerItemControl()
        {
            InitializeComponent();
            DataContextChanged += BagItemControl_DataContextChanged;
            Unloaded += ContainerItemControl_Unloaded;
        }

        private void ContainerItemControl_Unloaded(object sender, RoutedEventArgs e)
        {
            DataContextChanged -= BagItemControl_DataContextChanged;
            Item = null;
        }

        private void BagItemControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Item = (Item)DataContext;
        }
    }
}
