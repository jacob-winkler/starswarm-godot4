using StarSwarm.Project.GSAI_Framework;

namespace StarSwarm.Project.Ships.States;

public partial class Rest<TShip> : ShipState<TShip>
    where TShip : GSAICharacterBody2D
{
    private GSAITargetAcceleration _acceleration = new GSAITargetAcceleration(); 

    public override void _PhysicsProcess(double delta)
    {
        Ship.Agent.ApplySteering(_acceleration, delta);
    }
}