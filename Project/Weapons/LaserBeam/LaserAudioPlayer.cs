using Godot;
using System;

public class LaserAudioPlayer : LoopingAudioStreamPlayer2D
{
    public Tween Tween { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        Tween = GetNode<Tween>("Tween");
        Tween.Connect("tween_completed", this, "AudioFinished");
    }

    public override void End()
    {
        Tween.StopAll();
        Tween.InterpolateProperty(this, "volume_db", 0, -80f, 0.3f, Tween.TransitionType.Sine,
                Tween.EaseType.In);
        Tween.Start();
    }

    private void AudioFinished(Godot.Object incomingObject, NodePath key)
    {
        Stop();
        Ending = true;
        VolumeDb = 0;
    }
}
