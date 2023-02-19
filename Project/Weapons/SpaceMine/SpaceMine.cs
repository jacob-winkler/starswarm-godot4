using Godot;
using System;

public class SpaceMine : Sprite
{
    [Export]
    public float Damage { get; set; } = 100f;
    [Export]
    public float CountdownTime { get; set; } = 5;

    public Events Events { get; set; } = default!;
    public Area2D BlastRadius { get; set; } = default!;
    public AnimationPlayer CountdownAnimationPlayer { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Events = GetNode<Events>("/root/Events");
        BlastRadius = GetNode<Area2D>("BlastRadius");
        CountdownAnimationPlayer = GetNode<AnimationPlayer>("CountdownAnimation");

        CountdownAnimationPlayer.Connect("animation_finished", this, "OnDetonation");
        ArmSpaceMine();     
    }

    private void ArmSpaceMine()
    {
        var countdownAnimation = CountdownAnimationPlayer.GetAnimation("SpaceMineCountdown");
        var animationSpeed = CountdownTime / countdownAnimation.Length;
        countdownAnimation.Loop = false;
        CountdownAnimationPlayer.Play("SpaceMineCountdown", customSpeed: animationSpeed);
    }

    private void OnDetonation(String animationName)
    {
        var bodies = BlastRadius.GetOverlappingBodies();

        foreach(var body in bodies)
        {
            Events.EmitSignal("Damaged", body, Damage, this);
        }

        QueueFree();
    }
}
