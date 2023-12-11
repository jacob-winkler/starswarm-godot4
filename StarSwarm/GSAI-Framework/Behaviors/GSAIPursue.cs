using Godot;

namespace StarSwarm.GSAI_Framework.Behaviors;

public partial class GSAIPursue : GSAISteeringBehavior
{
    public GSAISteeringAgent Target { get; set; } = default!;
    public float PredictTimeMax { get; set; }

    public GSAIPursue(GSAISteeringAgent agent, GSAISteeringAgent target, float predictTimeMax = 1f)
        : base(agent)
    {
        Target = target;
        PredictTimeMax = predictTimeMax;
    }

    public override void _CalculateSteering(GSAITargetAcceleration acceleration)
    {
        var targetPosition = Target.Position;
        var distanceSquared = (targetPosition - Agent.Position).LengthSquared();

        var speedSquared = Agent.LinearVelocity.LengthSquared();
        var predictTime = PredictTimeMax;

        if(speedSquared > 0)
        {
            var predictTimeSquared = distanceSquared / speedSquared;
            if(predictTimeSquared < PredictTimeMax * PredictTimeMax)
                predictTime = Mathf.Sqrt(predictTimeSquared);
        }

        acceleration.Linear = ((targetPosition + (Target.LinearVelocity * predictTime)) - Agent.Position).Normalized();
        acceleration.Linear *= GetModifiedAcceleration();

        acceleration.Angular = 0;
    }

    public float GetModifiedAcceleration() => Agent.LinearAccelerationMax;
}