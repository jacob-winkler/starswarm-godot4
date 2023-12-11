using Godot;
using StarSwarm.SWStateMachine;

namespace StarSwarm.Ships.States;

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
