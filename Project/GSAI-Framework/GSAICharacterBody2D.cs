using Godot;
using StarSwarm.Project.GSAI_Framework.Agents;

namespace StarSwarm.Project.GSAI_Framework;

public partial class GSAICharacterBody2D : CharacterBody2D
{
    public GSAIKinematicBody2DAgent Agent { get; set; } = default!;

    public GSAICharacterBody2D() {
        Agent = new GSAIKinematicBody2DAgent();
        Agent.Initialize(this);
    }
}
