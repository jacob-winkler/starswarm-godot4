using System.Collections.Generic;
using Godot;

namespace StarSwarm.Project.GSAI_Framework.Behaviors;

public partial class GSAIBlend : GSAISteeringBehavior
{
    private List<(GSAISteeringBehavior behavior, float weight)> _behaviors = new List<(GSAISteeringBehavior behavior, float weight)>();
    private GSAITargetAcceleration _accel = new GSAITargetAcceleration();

    public GSAIBlend(GSAISteeringAgent agent)
        :base(agent)
    {
    }

    // Appends a behavior to the internal array along with its `weight`.
    public void Add(GSAISteeringBehavior behavior, float weight)
    {
        behavior.Agent = Agent;
        _behaviors.Add((behavior, weight));
    }

    // Returns the behavior at the specified `index`, or an empty `Dictionary` if
    // none was found.
    public (GSAISteeringBehavior? behavior, float weight) GetBehaviorAt(int index)
    {
        if (_behaviors.Count > index)
            return _behaviors[index];
        GD.Print("Tried to get index " + index + " in array of size " + _behaviors.Count);
        return (null, 0);
    }

    public override void _CalculateSteering(GSAITargetAcceleration blendedAccel)
    {
        blendedAccel.SetZero();

        foreach (var bw in _behaviors)
        {
            bw.behavior.CalculateSteering(_accel);
            blendedAccel.AddScaledAccel(_accel, bw.weight);
        }

        blendedAccel.Linear = GSAIUtils.ClampedV3(blendedAccel.Linear, Agent.LinearAccelerationMax);
        blendedAccel.Angular = Mathf.Clamp(
            blendedAccel.Angular, -Agent.AngularAccelerationMax, Agent.AngularAccelerationMax
        );
    }
}