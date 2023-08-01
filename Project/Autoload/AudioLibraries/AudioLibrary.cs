using Godot;
using System;

namespace StarSwarm.Project.Autoload.AudioLibraries;

public partial class AudioLibrary : Node2D
{
    private readonly RandomNumberGenerator _rng = new();

    public Node GetAudioPlayer(String? nodePath = null)
    {
        if (nodePath != null)
            return GetNode<Node>(nodePath);
        else
        {
            _rng.Randomize();

            return GetChild(_rng.RandiRange(0, GetChildCount() - 1));
        }
    }
}