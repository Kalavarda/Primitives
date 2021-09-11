using System;
using System.Windows;
using System.Windows.Controls;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.WPF.Controllers
{
    public class PositionController
    {
        private readonly FrameworkElement _frameworkElement;

        public PositionController(FrameworkElement frameworkElement, IHasPosition hasPosition)
        {
            _frameworkElement = frameworkElement ?? throw new ArgumentNullException(nameof(frameworkElement));
            hasPosition.Position.Changed += Position_Changed;
            Position_Changed(hasPosition.Position);
        }

        private void Position_Changed(Kalavarda.Primitives.Geometry.PointF p)
        {
            _frameworkElement.Do(() =>
            {
                Canvas.SetLeft(_frameworkElement, p.X - _frameworkElement.ActualWidth / 2);
                Canvas.SetTop(_frameworkElement, p.Y - _frameworkElement.ActualHeight / 2);
            });
        }
    }
}
