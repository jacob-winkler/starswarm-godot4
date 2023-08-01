using Godot;
using System;

namespace StarSwarm.Project.GSAI_Framework.Behaviors;

public partial class GSAISteeringBehavior
{
    public Boolean IsEnabled = true;
    public GSAISteeringAgent Agent { get; set; } = new GSAISteeringAgent();

    public GSAISteeringBehavior(GSAISteeringAgent agent)
    {
        this.Agent = agent;
    }

    public void CalculateSteering(GSAITargetAcceleration acceleration)
    {
        if(IsEnabled)
        {
            _CalculateSteering(acceleration);
        }
        else
        {
            acceleration.SetZero();
        }
    }

    public virtual void _CalculateSteering(GSAITargetAcceleration acceleration) => acceleration.SetZero();
}