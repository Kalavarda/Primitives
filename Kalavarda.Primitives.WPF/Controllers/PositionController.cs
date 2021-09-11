using System;
using System.Windows;
using System.Windows.Controls;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.WPF.Controllers
{
    public class PositionController: IDisposable
    {
        private readonly FrameworkElement _frameworkElement;
        private readonly IHasPosition _hasPosition;

        public PositionController(FrameworkElement frameworkElement, IHasPosition hasPosition)
        {
            _frameworkElement = frameworkElement ?? throw new ArgumentNullException(nameof(frameworkElement));
            _hasPosition = hasPosition ?? throw new ArgumentNullException(nameof(hasPosition));

            _hasPosition.Position.Changed += Position_Changed;
            Position_Changed(hasPosition.Position);
        }

        private void Position_Changed(Geometry.PointF p)
        {
            _frameworkElement.Do(() =>
            {
                Canvas.SetLeft(_frameworkElement, p.X - _frameworkElement.Width / 2);
                Canvas.SetTop(_frameworkElement, p.Y - _frameworkElement.Height / 2);
            });
        }

        public void Dispose()
        {
            _hasPosition.Position.Changed -= Position_Changed;
        }
    }
}
