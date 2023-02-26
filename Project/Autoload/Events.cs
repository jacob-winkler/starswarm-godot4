using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Events : Node
{
    // Player signals
    [Signal]
    public delegate void Damaged(Node target, float amount, Node origin);

    [Signal]
    public delegate void UpgradeChosen();

    [Signal]
    public delegate void PlayerSpawned(PlayerShip player);

    /// Enemy signals
    [Signal]
    public delegate void EnemyAdrift(PhysicsBody2D body);

    [Signal]
    public delegate void SpaceCrabDied();

    // Game signals
    [Signal]
    public delegate void NodeSpawned();

    [Signal]
    public delegate void GameMinutePassed();

    [Signal]
    public delegate void AddPoints(Int32 points);
}
