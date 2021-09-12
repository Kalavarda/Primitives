using System;
using System.Globalization;

namespace Kalavarda.Primitives.Utils
{
    public static class StringExtensions
    {
        private static readonly CultureInfo CultInfo = new CultureInfo("Ru-ru");

        static StringExtensions()
        {
            CultInfo.NumberFormat.NumberDecimalSeparator = ".";
        }

        public static string ToStr(this float value)
        {
            if (MathF.Abs(value) < 0.0001)
                return "0";

            if (MathF.Abs(value) < 100)
            {
                var s = MathF.Round(value, 1).ToString(CultInfo).Trim();
                if (s.StartsWith("."))
                    s = "0" + s;
                return s;
            }

            return MathF.Round(value).ToString("### ### ###", CultInfo).Trim();
        }

        public static string ToStr(this decimal value)
        {
            return Math.Round(value, 2).ToString("### ### ###").Trim();
        }
    }
}
