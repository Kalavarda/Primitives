namespace Kalavarda.Primitives.Visualization
{
    public class Scene
    {
        public Layer[] Layers { get; set; }
    }

    public class Layer
    {
        public IReadonlyVisualObject VisualObjects { get; set; }
    }
}
