namespace Kalavarda.Primitives.Abstract
{
    public interface IRandom
    {
        float Float();

        double Double();

        /// <summary>
        /// <see cref="min"/> и <see cref="max"/> - включительно
        /// </summary>
        int Int(int min, int max);
    }
}
