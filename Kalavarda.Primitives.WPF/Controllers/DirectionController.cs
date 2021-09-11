using System;
using System.Windows;
using System.Windows.Media;
using Kalavarda.Primitives.Geometry;

namespace Kalavarda.Primitives.WPF.Controllers
{
    public class DirectionController
    {
        private readonly AngleF _angle;
        private readonly RotateTransform _rotateTransform;

        public DirectionController(FrameworkElement control, AngleF angle)
        {
            _angle = angle ?? throw new ArgumentNullException(nameof(angle));

            _rotateTransform = new RotateTransform();
            control.RenderTransform = _rotateTransform;
            _rotateTransform.CenterX = control.Width / 2;
            _rotateTransform.CenterY = control.Height / 2;

            angle.Changed += Angle_Changed;
            Angle_Changed();
        }

        private void Angle_Changed()
        {
            _rotateTransform.Angle = _angle.ValueInDegrees;
        }
    }
}
