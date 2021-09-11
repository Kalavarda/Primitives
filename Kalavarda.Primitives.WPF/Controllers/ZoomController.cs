using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kalavarda.Primitives.Controllers
{
    public class ZoomController
    {
        private const double ZoomSpeed = 0.25;
        private const double MaxZoom = 1000;

        private readonly Canvas _canvas;
        private readonly ScaleTransform _scaleTransform;
        private readonly TranslateTransform _translateTransform;

        public ZoomController(IInputElement inputElement, Canvas canvas, ScaleTransform scaleTransform, TranslateTransform translateTransform)
        {
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            _scaleTransform = scaleTransform ?? throw new ArgumentNullException(nameof(scaleTransform));
            _translateTransform = translateTransform ?? throw new ArgumentNullException(nameof(translateTransform));
            inputElement.MouseWheel += UiElement_MouseWheel;
        }

        private void UiElement_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var d = 1 + Math.Abs(ZoomSpeed * e.Delta / 100d);
            if (e.Delta < 0)
                d = 1 / d;

            var pointAtCanvas1 = e.GetPosition(_canvas);

            _scaleTransform.ScaleX = Math.Min(d * _scaleTransform.ScaleX, MaxZoom);
            _scaleTransform.ScaleY = _scaleTransform.ScaleX;

            var pointAtCanvas2 = e.GetPosition(_canvas);
            _translateTransform.X += (pointAtCanvas2.X - pointAtCanvas1.X) * _scaleTransform.ScaleX;
            _translateTransform.Y += (pointAtCanvas2.Y - pointAtCanvas1.Y) * _scaleTransform.ScaleY;
        }
    }
}
