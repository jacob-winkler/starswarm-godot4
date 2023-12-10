using System;
using Godot;

namespace StarSwarm.Project.GSAI_Framework.Behaviors;

public partial class GSAIMatchOrientation : GSAISteeringBehavior
{
    public Boolean UseZ { get; set; }
    public GSAIAgentLocation Target { get; set; } = default!;
    public float AlignmentTolerance { get; set; }
    public float DecelerationRadius { get; set; }
    public float TimeToReach { get; set; } = 0.1f;

    public GSAIMatchOrientation(GSAISteeringAgent agent, GSAIAgentLocation target, Boolean useZ = false)
        : base(agent)
    { 
        UseZ = useZ;
        Target = target;
    }

    public void MatchOrientation(GSAITargetAcceleration acceleration, float desiredOrientation)
    {
        var rotation = Mathf.Wrap(desiredOrientation - Agent.Orientation, -Mathf.Pi, Mathf.Pi);
        var rotationSize = Mathf.Abs(rotation);

        if (rotationSize <= AlignmentTolerance)
        {
            acceleration.SetZero();
        }
        else
        {
            var desired_rotation = Agent.AngularSpeedMax;

            if (rotationSize <= DecelerationRadius)
            {
                desired_rotation *= rotationSize / DecelerationRadius;
            }

            desired_rotation *= rotation / rotationSize;

            acceleration.Angular = ((desired_rotation - Agent.AngularVelocity) / TimeToReach);

            var limitedAcceleration = Mathf.Abs(acceleration.Angular);
            if (limitedAcceleration > Agent.AngularAccelerationMax)
            {
                acceleration.Angular *= (Agent.AngularAccelerationMax / limitedAcceleration);
            }

            acceleration.Linear = Vector3.Zero;
        }
    }

    public override void _CalculateSteering(GSAITargetAcceleration acceleration)
    {
        MatchOrientation(acceleration, Target.Orientation);
    }
}