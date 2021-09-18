namespace Kalavarda.Primitives.Abstract
{
    public interface IRandom
    {
        /// <summary>
        /// [0..1]
        /// </summary>
        float Float();

        /// <summary>
        /// [0..1]
        /// </summary>
        double Double();

        /// <summary>
        /// <see cref="min"/> и <see cref="max"/> - включительно
        /// </summary>
        int Int(int min, int max);

        /// <summary>
        /// Испытать удачу с шансом 1/n
        /// </summary>
        bool Chance(int n);
    }
}
