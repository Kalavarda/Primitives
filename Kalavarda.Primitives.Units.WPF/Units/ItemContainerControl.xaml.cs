using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Units.Items;
using Kalavarda.Primitives.WPF;

namespace Kalavarda.Primitives.Units.WPF.Units
{
    public partial class ItemContainerControl
    {
        private IItemContainer _itemContainer;

        public IItemContainer ItemContainer
        {
            get => _itemContainer;
            set
            {
                if (_itemContainer == value)
                    return;

                _itemContainer = value;

                if (_itemContainer != null)
                {
                    _itemContainer.Changed += Bag_Changed;
                    Bag_Changed(_itemContainer);
                }
            }
        }

        private void Bag_Changed(IItemContainer container)
        {
            this.Do(() =>
            {
                _itemsControl.ItemsSource = container.Items.OfType<IHasImage>();
            });
        }

        public Item SelectedItem { get; private set; }

        public ItemContainerControl()
        {
            InitializeComponent();
        }

        private void _itemsControl_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(_itemsControl);
            var frameworkElement = (FrameworkElement)System.Windows.Media.VisualTreeHelper.HitTest(this, point).VisualHit;
            while (!(frameworkElement is ContainerItemControl) && frameworkElement.Parent != null)
                frameworkElement = frameworkElement.Parent as FrameworkElement;
            SelectedItem = frameworkElement is ContainerItemControl bagItemControl ? bagItemControl.Item : null;
        }

        public event Action<ContextMenu, Item> ContextMenuOpening;

        public event Action<Item> UseDefaultAction;

        private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (SelectedItem != null)
                ContextMenuOpening?.Invoke(_menu, SelectedItem);
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedItem != null)
                UseDefaultAction?.Invoke(SelectedItem);
        }
    }
}
