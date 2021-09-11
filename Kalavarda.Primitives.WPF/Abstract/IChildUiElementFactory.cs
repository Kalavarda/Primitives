using System.Windows;
using Kalavarda.Primitives.Skills;

namespace Kalavarda.Primitives.WPF.Abstract
{
    public interface IChildUiElementFactory
    {
        UIElement Create(IChildItem childItem);
    }
}
