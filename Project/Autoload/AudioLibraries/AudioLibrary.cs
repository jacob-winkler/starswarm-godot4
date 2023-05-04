using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace StarSwarm.Project.Autoload.AudioLibraries
{
    public class AudioLibrary : Node2D
    {
        private readonly RandomNumberGenerator _rng = new RandomNumberGenerator();

        public Node GetAudioPlayer(String? nodePath = null)
        {
            if(nodePath != null)
                return GetNode<Node>(nodePath);
            else
            {
                _rng.Randomize();

                return GetChild(_rng.RandiRange(0, GetChildCount() - 1));
            }
        }
    }
}