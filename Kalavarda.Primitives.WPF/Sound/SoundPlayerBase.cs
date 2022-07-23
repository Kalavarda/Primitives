using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Kalavarda.Primitives.Sound;

namespace Kalavarda.Primitives.WPF.Sound
{
    public abstract class SoundPlayerBase : ISoundPlayer
    {
        private readonly string _soundsFolder;
        private readonly ICollection<PlayerTuple> _mediaPlayers = new List<PlayerTuple>();
        private readonly MediaPlayer _dispatcherObject = new();

        protected SoundPlayerBase(string soundsFolder)
        {
            _soundsFolder = soundsFolder ?? throw new ArgumentNullException(nameof(soundsFolder));
        }

        public void Play(string soundKey)
        {
            _dispatcherObject.Do(() =>
            {
                PlayFile(GetFileName(soundKey));
            });
        }

        private void PlayFile(string fileName)
        {
            var mediaPlayer = GetMediaPlayer(fileName);
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        protected abstract string GetFileName(string soundKey);

        private MediaPlayer GetMediaPlayer(string fileName)
        {
            var tuple = _mediaPlayers.FirstOrDefault(t => t.FileName == fileName && t.IsFree);
            if (tuple != null)
                return tuple.Player;

            var mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(GetResourceFullFileName(fileName)));
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded1;
            _mediaPlayers.Add(new PlayerTuple(fileName, mediaPlayer));
            return mediaPlayer;
        }

        private void MediaPlayer_MediaEnded1(object sender, EventArgs e)
        {
            var mediaPlayer = (MediaPlayer)sender;
            _mediaPlayers.First(t => t.Player == mediaPlayer).IsFree = true;
        }

        private class PlayerTuple
        {
            public string FileName { get; }

            public MediaPlayer Player { get; }

            public bool IsFree { get; set; }

            public PlayerTuple(string fileName, MediaPlayer player)
            {
                FileName = fileName;
                Player = player;
            }
        }
        public string GetResourceFullFileName(string fileName)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _soundsFolder, fileName);
        }
    }
}
