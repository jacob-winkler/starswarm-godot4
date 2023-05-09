using System.Collections.Generic;
using Godot;
using StarSwarm.Project.Autoload.AudioLibraries;
using StarSwarm.Project.Common;

namespace StarSwarm.Project.Autoload
{
    [Tool]
    public class AudioManager2D : Node2D
    {
        public void Play(KnownAudioStreams audioStream, Vector2? position = null)
        {
            var audioPlayer = GetAudioPlayer(audioStream);

            if(position != null)
                audioPlayer.AudioPlayer.GlobalPosition = position.Value;

            audioPlayer.PlayAndDispose();
        }

        private DisposableAudioStreamPlayer2D GetAudioPlayer(KnownAudioStreams audioStream)
        {
            var audioPlayer = GetNode<Node>(audioStream.ToString());

            if (audioPlayer is AudioLibrary library)
            {
                audioPlayer = library.GetAudioPlayer();
            }

            audioPlayer = audioPlayer.Duplicate();
            var disposableAudioPlayer = new DisposableAudioStreamPlayer2D((AudioStreamPlayer2D)audioPlayer);

            AddChild(disposableAudioPlayer);
            return disposableAudioPlayer;
        }
    }
}
