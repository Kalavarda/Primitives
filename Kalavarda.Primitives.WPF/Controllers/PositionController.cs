using System;
using System.Windows;
using System.Windows.Controls;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.WPF.Controllers
{
    public class PositionController: IDisposable
    {
        private readonly FrameworkElement _frameworkElement;
        
        public IHasPosition Object { get; set; }

        public PositionController(FrameworkElement frameworkElement, IHasPosition hasPosition)
        {
            _frameworkElement = frameworkElement ?? throw new ArgumentNullException(nameof(frameworkElement));
            Object = hasPosition ?? throw new ArgumentNullException(nameof(hasPosition));

            Object.Position.Changed += Position_Changed;
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
            Object.Position.Changed -= Position_Changed;
        }
    }
}
