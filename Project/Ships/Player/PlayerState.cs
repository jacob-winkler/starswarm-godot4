using StarSwarm.Project.SWStateMachine;

namespace StarSwarm.Project.Ships.Player;

public partial class PlayerState : State
{
    public PlayerShip Ship { get; set; } = default!;

    public override void _Ready()
    {
        base._Ready();
        Ship = (PlayerShip)Owner;
    }
}