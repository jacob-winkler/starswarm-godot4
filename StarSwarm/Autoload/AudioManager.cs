using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using StarSwarm.Autoload.AudioLibraries;
using StarSwarm.Common;

namespace StarSwarm.Autoload;

[Tool]
public partial class AudioManager : Node
{
    public void Play(KnownAudioStreams audioStream, ProcessModeEnum processMode = ProcessModeEnum.Pausable)
    {
        var audioPlayer = GetAudioPlayer(audioStream);
        audioPlayer.ProcessMode = processMode;

        audioPlayer.PlayAndDispose();
    }

    public override string[] _GetConfigurationWarnings()
    {
        var warning = new List<string>();
        var enumValues = Enum.GetValues(typeof(KnownAudioStream2Ds)).OfType<object>().Select(x => x.ToString());
        if (enumValues == null)
            return warning.ToArray();

        foreach (var stream in enumValues)
        {
            var node = GetNode(stream?.ToString());
            if (node == null)
                warning.Add($"Missing audio node: '{stream}'");
        }

        foreach (var child in GetChildren())
        {
            if (!enumValues.Contains(child.Name.ToString()))
                warning.Add($"Missing KnownAudioStream2Ds enum value: '{child.Name}'");
        }

        return warning.ToArray();
    }

    private DisposableAudioStreamPlayer GetAudioPlayer(KnownAudioStreams audioStream)
    {
        var audioPlayer = GetNode<Node>(audioStream.ToString());

        if (audioPlayer is AudioLibrary library)
        {
            audioPlayer = library.GetAudioPlayer();
        }

        audioPlayer = audioPlayer.Duplicate();
        var disposableAudioPlayer = new DisposableAudioStreamPlayer((AudioStreamPlayer)audioPlayer);

        AddChild(disposableAudioPlayer);
        return disposableAudioPlayer;
    }
}