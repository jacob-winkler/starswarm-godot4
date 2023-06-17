using Godot;

public partial class VFX : Node2D
{
    private const float TRAIL_VELOCITY_THRESHOLD = 200.0f;
    public CpuParticles2D ShipTrail { get; set; } = new CpuParticles2D();

    public override void _Ready()
    {
        base._Ready();
        ShipTrail = GetNode<CpuParticles2D>("MoveTrail");
    }

    public void MakeTrail(float currentSpeed)
    {
        ShipTrail.Emitting = currentSpeed > TRAIL_VELOCITY_THRESHOLD;
    }

}
