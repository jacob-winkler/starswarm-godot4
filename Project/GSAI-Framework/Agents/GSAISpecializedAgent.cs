using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.GSAI_Framework.Agents;

public partial class GSAISpecializedAgent : GSAISteeringAgent
{
    public bool CalculateVelocities { get; set; } = true;

    public bool ApplyLinearDrag { get; set; } = true;

    public bool ApplyAngularDrag { get; set; } = true;

    public float LinearDragPercentage { get; set; } = 0.0F;

    public float AngularDragPercentage { get; set; } = 0.0F;

    protected float _lastOrientation;

    protected bool _appliedSteering = false;

    public virtual void ApplySteering(GSAITargetAcceleration acceleration, double delta)
    { }
}
