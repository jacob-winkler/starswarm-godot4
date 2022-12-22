using Godot;
using System;

public class LoopingAudioStreamPlayer2D : AudioStreamPlayer2D
{
    [Export]
    public AudioStream SoundStart;
    [Export]
    public AudioStream SoundLoop;
    [Export]
    public AudioStream SoundTail;

    public bool Ending = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("finished", this, "OnFinished");
    }

    public void Start()
    {
        Stream = SoundStart;
        Ending = false;
        Play();
    }

    public void End()
    {
        Stream = SoundTail;
        Ending = true;
    }

    public void OnFinished()
    {
        if(Stream == SoundStart)
        {
            Stream = SoundLoop;
            Play();
        }
    }
}
