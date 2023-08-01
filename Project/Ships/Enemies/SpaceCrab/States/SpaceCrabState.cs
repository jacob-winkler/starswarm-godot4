using StarSwarm.Project.SWStateMachine;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab.States;

public partial class SpaceCrabState : State
{
    public SpaceCrab Ship {get; set; } = default!;
    public override void _Ready()
    {
        base._Ready();
        Ship = (SpaceCrab)Owner;
    }
}
