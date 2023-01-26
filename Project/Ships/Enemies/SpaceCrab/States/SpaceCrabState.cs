using Godot;
using StarSwarm.Project.SWStateMachine;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab.States
{
    public class SpaceCrabState : State
    {
        public SpaceCrab Ship {get; set; } = null!;
        public async override void _Ready()
        {
            base._Ready();
            await ToSignal(Owner, "ready");
            Ship = (SpaceCrab)Owner;
        }
    }
}
