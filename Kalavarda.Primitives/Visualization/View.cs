using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Kalavarda.Primitives.Visualization
{
    [DebuggerDisplay("Angle={Angle}°")]
    public class View
    {
        private static readonly Frame[] NoFrames = new Frame[0];

        /// <summary>
        /// В градусах
        /// </summary>
        public int Angle { get; set; }

        public Frame[] Frames { get; set; } = NoFrames;

        [JsonIgnore]
        public TimeSpan Duration { get; set; }

        public float DurationSec
        {
            get => (float)Duration.TotalSeconds;
            set => Duration = TimeSpan.FromSeconds(value);
        }

        public override string ToString()
        {
            return $"{Angle}°, {DurationSec} sec., {Frames.Length} frames";
        }
    }

    public class Frame
    {
        public byte[] RawData { get; set; }
    }
}
