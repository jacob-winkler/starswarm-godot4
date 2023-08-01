using Godot;

public partial class LoopingAudioStreamPlayer2D : AudioStreamPlayer2D
{
    [Export]
    public AudioStream SoundStart = default!;
    [Export]
    public AudioStream SoundLoop = default!;
    [Export]
    public AudioStream SoundTail = default!;

    public bool Ending = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("finished", new Callable(this, "OnFinished"));
    }

    public void Start()
    {
        Stream = SoundStart;
        Ending = false;
        Play();
    }

    public virtual void End()
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
