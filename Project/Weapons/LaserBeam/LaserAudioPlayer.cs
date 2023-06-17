using Godot;
using System;
using static Godot.Tween;

public partial class LaserAudioPlayer : LoopingAudioStreamPlayer2D
{
    public Tween Tween { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    public override void End()
    {
        if (Tween != null)
            Tween.Kill();
        Tween = CreateTween();

        Tween.TweenProperty(this, "volume_db", -80f, 0.3f)
            .SetTrans(TransitionType.Sine)
            .SetEase(EaseType.In)
            .Connect("finished", new Callable(this, "AudioFinished"));

        Tween.Play();
    }

    private void AudioFinished()
    {
        Stop();
        Ending = true;
        VolumeDb = 0;
    }
}
