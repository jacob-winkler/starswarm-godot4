using System;
using Godot;

namespace StarSwarm.Project.GSAI_Framework.Behaviors;

public partial class GSAIFace : GSAIMatchOrientation
{
    public GSAIFace(GSAISteeringAgent agent, GSAIAgentLocation target, Boolean useZ = false)
        : base(agent, target, useZ)
    { }

    public void Face(GSAITargetAcceleration acceleration, Vector3 targetPosition)
    {
        var toTarget = targetPosition - Agent.Position;
        var distanceSquared = toTarget.LengthSquared();

        if(distanceSquared < Agent.ZeroLinearSpeedThreshold)
            acceleration.SetZero();
        else
        {
            var orientation = UseZ ? 
                GSAIUtils.Vector3ToAngle(toTarget) :
                GSAIUtils.Vector2ToAngle(GSAIUtils.ToVector2(toTarget));

            MatchOrientation(acceleration, orientation);
        }
    }

    public override void _CalculateSteering(GSAITargetAcceleration acceleration) => Face(acceleration, Target.Position);
}