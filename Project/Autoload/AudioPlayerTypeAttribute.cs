using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace StarSwarm.Project.Autoload
{
    public class AudioPlayerTypeAttribute : Attribute
    {
        public AudioPlayerTypeAttribute(Type audioPlayerType)
        {
            if(audioPlayerType != typeof(AudioStreamPlayer2D) || audioPlayerType != typeof(AudioStreamPlayer))
                throw new TypeLoadException("Provided audio player type is not valid.");

            AudioPlayerType = audioPlayerType;
        }

        public Type AudioPlayerType { get; set; }
    }
}