using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using StarSwarm.Project.Autoload.AudioLibraries;
using StarSwarm.Project.Common;

namespace StarSwarm.Project.Autoload;

[Tool]
public partial class AudioManager2D : Node2D
{
    public void Play(KnownAudioStream2Ds audioStream, Vector2? position = null)
    {
        var audioPlayer = GetAudioPlayer(audioStream, position);

        audioPlayer.PlayAndDispose();
    }

    public override String[] _GetConfigurationWarnings()
    {
        var warning = new List<String>();
        var enumValues = Enum.GetValues(typeof(KnownAudioStream2Ds)).OfType<object>().Select(x => x.ToString());
        if (enumValues == null)
            return warning.ToArray();
        
        foreach (var stream in enumValues)
        {
            var node = GetNode(stream?.ToString());
            if (node == null)
                warning.Add($"Missing audio node: '{stream}'");
        }

        foreach(var child in GetChildren())
        {
            if (!enumValues.Contains(child.Name.ToString()))
                warning.Add($"Missing KnownAudioStream2Ds enum value: '{child.Name}'");
        }

        return warning.ToArray();
    }

    private DisposableAudioStreamPlayer GetAudioPlayer(KnownAudioStream2Ds audioStream, Vector2? position)
    {
        var audioPlayer = GetNode<Node>(audioStream.ToString());

        if (audioPlayer is AudioLibrary library)
        {
            audioPlayer = library.GetAudioPlayer();
        }

        audioPlayer = audioPlayer.Duplicate();
        var disposableAudioPlayer = new DisposableAudioStreamPlayer((AudioStreamPlayer2D)audioPlayer);

        AddChild(disposableAudioPlayer);

        if(position != null)
            disposableAudioPlayer.SetPosition(position.Value);

        return disposableAudioPlayer;
    }
}
