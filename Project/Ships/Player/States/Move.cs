using Godot;
using StarSwarm.Project.GSAI_Framework.Agents;

namespace StarSwarm.Project.Ships.Player.States;

public partial class Move : PlayerState
{
    [Export]
    public float DragLinearCoefficient = 0.05F;

    [Export]
    public float ReverseMultiplier = 0.50F;

    [Export]
    public float DragAngularCoeff = 0.1F;

    public float AccelerationMax = 0.0F;
    public float LinearSpeedMax = 1.2F;
    public float AngularSpeedMax = 0.0F;
    public float AngularAccelerationMax = 0.0F;

    public Vector2 LinearVelocity = Vector2.Zero;
    public float AngularVelocity = 0.0F;
    public bool IsReversing = false;
    public bool CanFire = true;

    public GSAIKinematicBody2DAgent Agent { get; set; } = null!;

    public override void _Ready()
    {
        base._Ready();

        Agent = new GSAIKinematicBody2DAgent();
        Agent.Initialize((CharacterBody2D)Owner);

        Ship.Stats.Initialize();

        AccelerationMax = Ship.Stats.GetAccelerationMax();
        LinearSpeedMax = Ship.Stats.GetLinearSpeedMax();
        AngularSpeedMax = Ship.Stats.GetAngularSpeedMax();
        AngularAccelerationMax = Ship.Stats.GetAngularAccelerationMax();

        Agent.LinearAccelerationMax = AccelerationMax * ReverseMultiplier;
        Agent.LinearSpeedMax = LinearSpeedMax;
        Agent.AngularAccelerationMax = Mathf.DegToRad(AngularAccelerationMax);
        Agent.AngularSpeedMax = Mathf.DegToRad(AngularSpeedMax);
        //Agent.BoundingRadius = MathUtils.GetTriangleCircumcircleRadius(Ship.Shape.Polygon);
    }

    public override void PhysicsProcess(double delta)
    {
        LinearVelocity = LinearVelocity.LimitLength(LinearSpeedMax);
        LinearVelocity = LinearVelocity.Lerp(Vector2.Zero, DragLinearCoefficient);

        AngularVelocity = Mathf.Clamp(AngularVelocity, -Agent.AngularSpeedMax, Agent.AngularSpeedMax);
        AngularVelocity = Mathf.Lerp(AngularVelocity, 0, DragAngularCoeff);

        Ship.Velocity = LinearVelocity;
        Ship.MoveAndSlide();
        Ship.Rotation += AngularVelocity * (float)delta;
        Ship.Vfx.MakeTrail(Ship.Velocity.Length());
    }
}