namespace Kalavarda.Primitives.Units.Interfaces
{
    public interface ILevelMultiplier
    {
        float GetRatio(ushort level);

        float GetRatio(float power);

        float GetValue(float baseValue, ushort level);

        long GetValue(int baseValue, ushort level);
    }
}
