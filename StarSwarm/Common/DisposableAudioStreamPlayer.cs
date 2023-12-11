using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace StarSwarm.Common;

public partial class DisposableAudioStreamPlayer : Node
{
    public DisposableAudioStreamPlayer(Node audioPlayer)
    {
        if(audioPlayer is AudioStreamPlayer2D stream2D)
            _audioPlayer2D = stream2D;
        else if(audioPlayer is AudioStreamPlayer stream)
            _audioPlayer = stream;
        else
            throw new ArgumentException("Bad audio stream player");

        audioPlayer.ProcessMode = ProcessModeEnum.Inherit;
        AddChild(audioPlayer);
    }

    private AudioStreamPlayer2D? _audioPlayer2D { get; set; }
    private AudioStreamPlayer? _audioPlayer { get; set; }

    public void PlayAndDispose()
    {
        if (_audioPlayer != null)
        {
            _audioPlayer.Connect("finished", new Callable(this, "Dispose"));
            _audioPlayer.Play();
        }

        if (_audioPlayer2D != null)
        {
            _audioPlayer2D.Connect("finished", new Callable(this, "Dispose"));
            _audioPlayer2D.Play();
        }
    }

    public void SetPosition(Vector2 position)
    {
        if(_audioPlayer2D != null)
            _audioPlayer2D.Position = position;
    }

    private new void Dispose()
    {
        QueueFree();
    }
}