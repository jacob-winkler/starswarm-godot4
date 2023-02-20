using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Events : Node
{
    [Signal]
    public delegate void Damaged(Node target, float amount, Node origin);

    [Signal]
    public delegate void UpgradeChosen();

    [Signal]
    public delegate void PlayerSpawned(PlayerShip player);

    [Signal]
    public delegate void NodeSpawned();
}
