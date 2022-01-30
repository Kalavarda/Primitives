using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.WPF.Controllers
{
    public class SinglePositionsController
    {
        private readonly IDictionary<IHasBounds, UIElement> _uiElements = new Dictionary<IHasBounds, UIElement>();

        public void Add(IHasBounds obj, UIElement uiElement)
        {
            _uiElements.Add(obj, uiElement);
            obj.PositionChanged += Obj_PositionChanged;
            Obj_PositionChanged(obj);
        }

        private void Obj_PositionChanged(IHasBounds obj)
        {
            var objControl = _uiElements[obj];
            objControl.Do(() =>
            {
                Canvas.SetLeft(objControl, obj.Bounds.Position.X - obj.Bounds.Width / 2);
                Canvas.SetTop(objControl, obj.Bounds.Position.Y - obj.Bounds.Height / 2);
            });
        }
    }
}
