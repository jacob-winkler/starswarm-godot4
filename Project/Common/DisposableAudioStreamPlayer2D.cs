using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace StarSwarm.Project.Common
{
    public class DisposableAudioStreamPlayer2D : Node
    {
        public DisposableAudioStreamPlayer2D(AudioStreamPlayer2D audioPlayer)
        {
            AudioPlayer = audioPlayer;
            AddChild(AudioPlayer);
        }

        public AudioStreamPlayer2D AudioPlayer { get; set; }

        public void PlayAndDispose()
        {
            AudioPlayer.Connect("finished", this, "Dispose");
            AudioPlayer.Play();
        }

        private new void Dispose()
        {
            QueueFree();
        }
    }
}