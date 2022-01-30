using System;
using System.IO;

namespace Kalavarda.Primitives.Visualization
{
    public interface IBinarySerializer
    {
        void Serialize<T>(T obj, Stream stream);

        T Deserialize<T>(Stream stream);
    }

    public class BinarySerializer : IBinarySerializer
    {
        public void Serialize(VisualObject visualObject, Stream stream)
        {
            if (visualObject == null) throw new ArgumentNullException(nameof(visualObject));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrEmpty(visualObject.Id))
                throw new Exception($"{nameof(visualObject.Id)} cannot be empty");

            using var writer = new BinaryWriter(stream);
            writer.Write(visualObject.Id);
            writer.Write(visualObject.States.Length);
            for (var i = 0; i < visualObject.States.Length; i++)
                Serialize(visualObject.States[i], writer);
        }

        public void Serialize<T>(T obj, Stream stream)
        {
            if (typeof(T) == typeof(VisualObject))
            {
                Serialize<VisualObject>((VisualObject)(object)obj, stream);
                return;
            }

            throw new NotImplementedException();
        }

        public VisualObject DeserializeVisualObject(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using var reader = new BinaryReader(stream);
            var result = new VisualObject { Id = reader.ReadString() };

            result.States = new State[reader.ReadInt32()];
            for (var i = 0; i < result.States.Length; i++)
                result.States[i] = DeserializeState(reader);

            return result;
        }

        public T Deserialize<T>(Stream stream)
        {
            if (typeof(T) == typeof(VisualObject))
                return (T)(object)DeserializeVisualObject(stream);

            throw new NotImplementedException();
        }

        private static void Serialize(State state, BinaryWriter writer)
        {
            writer.Write(state.Name);
            writer.Write(state.Looping);

            writer.Write(state.Views.Length);
            for (var i = 0; i < state.Views.Length; i++)
                Serialize(state.Views[i], writer);

            writer.Write(state.Sound != null);
            if (state.Sound != null)
                Serialize(state.Sound, writer);
        }

        private static State DeserializeState(BinaryReader reader)
        {
            var state = new State { Name = reader.ReadString() };
            state.Looping = reader.ReadBoolean();

            state.Views = new View[reader.ReadInt32()];
            for (var i = 0; i < state.Views.Length; i++)
                state.Views[i] = DeserializeView(reader);

            if (reader.ReadBoolean())
                state.Sound = DeserializeSound(reader);

            return state;
        }

        private static void Serialize(View view, BinaryWriter writer)
        {
            writer.Write(view.Angle);
            writer.Write(view.DurationSec);
            writer.Write(view.Frames.Length);
            for (var i = 0; i < view.Frames.Length; i++)
            {
                writer.Write(view.Frames[i].RawData.Length);
                writer.Write(view.Frames[i].RawData);
            }
        }

        private static View DeserializeView(BinaryReader reader)
        {
            var view = new View
            {
                Angle = reader.ReadInt32()
            };
            view.DurationSec = reader.ReadSingle();
            view.Frames = new Frame[reader.ReadInt32()];
            for (var i = 0; i < view.Frames.Length; i++)
            {
                view.Frames[i] = new Frame();
                var count = reader.ReadInt32();
                view.Frames[i].RawData = reader.ReadBytes(count);
            }
            return view;
        }

        private static void Serialize(StateSound sound, BinaryWriter writer)
        {
            writer.Write(sound.RawData.Length);
            writer.Write(sound.RawData);
        }

        private static StateSound DeserializeSound(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            return new StateSound
            {
                RawData = reader.ReadBytes(count)
            };
        }
    }
}
