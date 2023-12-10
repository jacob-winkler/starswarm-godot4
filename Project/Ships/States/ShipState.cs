using Godot;
using StarSwarm.Project.SWStateMachine;

namespace StarSwarm.Project.Ships.States
{
    public partial class ShipState<TShip> : State
        where TShip : CharacterBody2D
    {
        public TShip Ship {get; set; } = default!;
        public override void _Ready()
        {
            base._Ready();
            Ship = (TShip)Owner;
        }
    }
}
