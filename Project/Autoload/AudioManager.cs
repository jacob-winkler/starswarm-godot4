using System.Collections.Generic;
using Godot;
using StarSwarm.Project.Autoload.AudioLibraries;

namespace StarSwarm.Project.Autoload
{
    public class AudioManager : Node2D
    {
        [Export]
        public Dictionary<KnownAudioStreams, PackedScene> AudioDictionary { get; set; } = new Dictionary<KnownAudioStreams, PackedScene>();

        private readonly Queue<Node> _disposableAudioPlayers = new Queue<Node>();

        public void Play(KnownAudioStreams audioStream, Vector2? position = null)
        {
            var audioPlayer = GetAudioPlayer(audioStream);
            var audioPlayerType = audioPlayer.GetType();

            if(audioPlayerType == null)
                GD.PushError($"Failed to find audio player type for audio stream '{audioStream}'");

            if (audioPlayerType == typeof(AudioStreamPlayer2D))
            {
                var spatialAudioPlayer = (AudioStreamPlayer2D)audioPlayer;

                if(position != null)
                    spatialAudioPlayer.GlobalPosition = position.Value;

                spatialAudioPlayer.Play();
            }
            else if (audioPlayerType == typeof(AudioStreamPlayer))
            {
                ((AudioStreamPlayer)audioPlayer).Play();
            }
        }

        private Node GetAudioPlayer(KnownAudioStreams audioStream)
        {
            var audioPlayer = AudioDictionary[audioStream].Instance();
            var audioPlayerType = audioPlayer.GetType();

            if(audioPlayerType == null)
                GD.PushError($"Failed to find audio player type for audio stream '{audioStream}'");

            if (audioPlayerType == typeof(AudioLibrary))
            {
                audioPlayer = ((AudioLibrary)audioPlayer).GetAudioPlayer().Duplicate();
            }

            _disposableAudioPlayers.Enqueue(audioPlayer);
            audioPlayer.Connect("finished", this, "DisposeAudioPlayers");

            AddChild(audioPlayer);
            return audioPlayer;
        }

        private void DisposeAudioPlayers()
        {
            var audioPlayer = _disposableAudioPlayers.Dequeue();
            audioPlayer.QueueFree();
        }
    }
}
