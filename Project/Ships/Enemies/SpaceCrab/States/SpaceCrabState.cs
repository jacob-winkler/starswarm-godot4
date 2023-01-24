using Godot;
using StarSwarm.Project.SWStateMachine;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab.States
{
    public class SpaceCrabState : State
    {
        public SpaceCrab Ship {get; set; } = new SpaceCrab();
        public async override void _Ready()
        {
            await ToSignal(Owner, "ready");
            Ship = (SpaceCrab)Owner;
        }
    }
}
