using Godot;

public class VFX : Node2D
{
    private const float TRAIL_VELOCITY_THRESHOLD = 200.0f;
    public CPUParticles2D ShipTrail { get; set; } = new CPUParticles2D();

    public override void _Ready()
    {
        base._Ready();
        ShipTrail = GetNode<CPUParticles2D>("MoveTrail");
    }

    public void MakeTrail(float currentSpeed)
    {
        ShipTrail.Emitting = currentSpeed > TRAIL_VELOCITY_THRESHOLD;
    }

}
