using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using StarSwarm.Project.Autoload.AudioLibraries;
using StarSwarm.Project.Common;

namespace StarSwarm.Project.Autoload
{
    [Tool]
    public class AudioManager : Node
    {
        public void Play(KnownAudioStreams audioStream, PauseModeEnum pauseModeEnum = PauseModeEnum.Stop)
        {
            var audioPlayer = GetAudioPlayer(audioStream);
            audioPlayer.PauseMode = pauseModeEnum;

            audioPlayer.PlayAndDispose();
        }

        public override String _GetConfigurationWarning()
        {
            var warning = String.Empty;
            var enumValues = Enum.GetValues(typeof(KnownAudioStreams)).OfType<object>().Select(x => x.ToString());
            
            foreach (var stream in enumValues)
            {
                var node = GetNode(stream.ToString());
                if (node == null)
                    warning += $"Missing audio node: '{stream}'\n";
            }

            foreach(var child in GetChildren())
            {
                if(!enumValues.Contains(((Node)child).Name))
                    warning += $"Missing KnownAudioStreams enum value: '{((Node)child).Name}'\n";
            }

            return warning.TrimEnd('\n');
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
}