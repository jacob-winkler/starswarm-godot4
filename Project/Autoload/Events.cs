using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Events : Node
{
    [Signal]
    public delegate void Damaged();

    [Signal]
    public delegate void UpgradeChosen();

    [Signal]
    public delegate void PlayerSpawned(PlayerShip player);

    [Signal]
    public delegate void NodeSpawned();


    // Enemy signals
    [Signal]
    public delegate void TargetAggroed(PhysicsBody2D target);
}
