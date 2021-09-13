using System;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Utils
{
    public class RandomImpl: IRandom
    {
        public static IRandom Instance = new RandomImpl();
        
        private static readonly Random Rand = new Random();

        private RandomImpl()
        {

        }

        public float Float()
        {
            return (float)Rand.NextDouble();
        }

        public double Double()
        {
            return Rand.NextDouble();
        }

        public int Int(int min, int max)
        {
            return Rand.Next(min, max + 1);
        }
    }
}
