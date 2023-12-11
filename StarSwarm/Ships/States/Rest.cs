using StarSwarm.GSAI_Framework;

namespace StarSwarm.Ships.States;

public partial class Rest<TShip> : ShipState<TShip>
    where TShip : GSAICharacterBody2D
{
    private readonly GSAITargetAcceleration _acceleration = new GSAITargetAcceleration(); 

    public override void _PhysicsProcess(double delta)
    {
        Ship.Agent.ApplySteering(_acceleration, delta);
    }
}