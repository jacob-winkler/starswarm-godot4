using Godot;
using StarSwarm.Project.Autoload;
using System;
using static Godot.Animation;

public partial class SpaceMine : Node2D
{
    [Export]
    public float Damage { get; set; } = 100f;
    [Export]
    public float CountdownTime { get; set; } = 5;

    public AudioManager2D AudioManager2D { get; set; } = default!;
    public Events Events { get; set; } = default!;
    public Area2D BlastRadius { get; set; } = default!;
    public AnimationPlayer AnimationPlayer { get; set; } = default!;

    public AnimatedSprite2D Explosion { get; set; } = default!;

    private float _radiusAlpha = 0.3f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AudioManager2D = GetNode<AudioManager2D>("/root/AudioManager2D");
        Events = GetNode<Events>("/root/Events");
        BlastRadius = GetNode<Area2D>("BlastRadius");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Explosion = GetNode<AnimatedSprite2D>("Explosion");

        Explosion.Connect("animation_finished", new Callable(this, "OnExplosionFinished"));
        AnimationPlayer.Connect("animation_finished", new Callable(this, "OnDetonation"));

        ArmSpaceMine();
    }

    public override void _Draw()
    {
        base._Draw();
        DrawRadius();
    }

    private void DrawRadius()
    {
        var radius = ((CircleShape2D)GetNode<CollisionShape2D>("BlastRadius/CollisionShape2D").Shape).Radius;
        DrawArc(Position - GlobalPosition, radius, 0, Mathf.Tau, (Int32)radius / 2, new Color(Colors.White, _radiusAlpha), 2);
    }

    private void ArmSpaceMine()
    {
        var countdownAnimation = AnimationPlayer.GetAnimation("SpaceMineCountdown");
        var animationSpeed = CountdownTime / countdownAnimation.Length;
        countdownAnimation.LoopMode = LoopModeEnum.None;
        AnimationPlayer.Play("SpaceMineCountdown", customSpeed: animationSpeed);
    }

    private void OnDetonation(String animationName)
    {
        _radiusAlpha = 0;

        if(animationName != "SpaceMineCountdown")
            return;

        Explosion.Visible = true;
        AudioManager2D.Play(KnownAudioStream2Ds.SpaceMine, GlobalPosition);
        Explosion.Play("Explosion");

        foreach(var body in BlastRadius.GetOverlappingBodies())
        {
            Events.EmitSignal("Damaged", body, Damage, this);
        }

        BlastRadius.Connect("body_entered", new Callable(this, "OnBodyEnteredBlastRadius"));
    }

    private void OnBodyEnteredBlastRadius(PhysicsBody2D body)
    {
        Events.EmitSignal("Damaged", body, Damage, this);
    }

    private void OnExplosionFinished()
    {
        QueueFree();
    }
}
